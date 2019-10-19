using UnityEngine;
using System.Collections;
using GenericFunctions;

public class Pelican : Bird
{
    [SerializeField] private Animator _pelicanAnimator;

    protected override BirdType _myBirdType => BirdType.Pelican;
    private int _currentTarIn;
    private Vector3[] _setPositions;

    private Vector3 TargetPosition => Constants.BalloonCenter.position + _sideMultiplier * _setPositions[_currentTarIn].x * Vector3.right + Vector3.up * _setPositions[_currentTarIn].y;

    protected override void Awake()
    {
        _pelicanAnimator.SetInteger(0, Random.Range(0, 2));
        base.Awake();
        float yAbove = 2;
        var yBelow = -2.2f;
        var resolution = 0.1f;
        var totalPoints = (int) ((yAbove - yBelow) / resolution);
        _setPositions = new Vector3[totalPoints];
        for (var i = 0; i < totalPoints; i++)
        {
            float iFloat = i;
            var xPoint = -1 * Mathf.Cos(2f * Mathf.PI * (iFloat / totalPoints)) + 1f;
            var yPoint = -2.1f * Mathf.Cos(2f * Mathf.PI * (iFloat / (totalPoints * 2)));
            var thisVector = new Vector3(xPoint, yPoint, 0f);
            _setPositions[i] = thisVector;
        }

        StartCoroutine(SwoopAround());
    }

    private bool _isDiving;
    private int _sideMultiplier;
    private float _moveSpeed = 2f;

    //Move from one checkpoint to another
    private IEnumerator SwoopAround()
    {
        _pelicanAnimator.SetInteger(0, PelAnimState.Flapping);
        _currentTarIn = 0;
        _sideMultiplier = transform.position.x < 0 ? 1 : -1;

        while (_currentTarIn < _setPositions.Length)
        {
            _rigbod.velocity = GetVelocity();
            var xFromJai = Constants.JaiTransform.position.x - transform.position.x;
            transform.FaceForward(xFromJai > 0);

            if (Vector3.Distance(transform.position, TargetPosition) < 0.2f)
            {
                _currentTarIn++;
                if (_pelicanAnimator.GetInteger(0) == PelAnimState.Flapping && _setPositions[_currentTarIn].y > 1.2f)
                {
                    StartCoroutine(TriggerDiveAnimation());
                }

                if (_currentTarIn > _setPositions.Length)
                {
                    break;
                }
            }

            yield return null;
        }

        StartCoroutine(DiveBomb(_sideMultiplier < 0));
    }

    private IEnumerator TriggerDiveAnimation()
    {
        float timeSinceStartedDiving = 0;
        _pelicanAnimator.SetInteger(0, PelAnimState.Diving);
        timeSinceStartedDiving = Time.time;
        while (true)
        {
            if (Time.time - timeSinceStartedDiving > 1f)
            {
                _currentTarIn = _setPositions.Length + 1;
                break;
            }

            yield return new WaitForEndOfFrame();
        }
    }

    private Vector2 GetVelocity()
    {
        return (TargetPosition - transform.position).normalized * _moveSpeed;
    }

    //plunge to (un)certain balloon-popping glory
    private IEnumerator DiveBomb(bool goingRight)
    {
        _pelicanAnimator.SetInteger(0, PelAnimState.Down);
        var diveAngle = goingRight ? -80f : 260f;
        _rigbod.velocity = ConvertAnglesAndVectors.ConvertAngleToVector2(diveAngle) * 6f;
        transform.FaceForward(_rigbod.velocity.x > 0);
        while (transform.position.y > -Constants.WorldDimensions.y - 1f)
        {
            yield return null;
        }

        _rigbod.velocity = Vector2.zero;
        _birdCollider.enabled = false;
        yield return new WaitForSeconds(2f);
        StartCoroutine(SwoopAround());
        while (transform.position.y < -Constants.WorldDimensions.y)
        {
            yield return null;
        }

        _birdCollider.enabled = true;
    }

    private static class PelAnimState
    {
        public const int Flapping = 0;
        public const int Diving = 1;
        public const int Down = 2;
    }
}