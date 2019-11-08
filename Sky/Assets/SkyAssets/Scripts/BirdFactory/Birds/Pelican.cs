using UnityEngine;
using System.Collections;
using GenericFunctions;

public class Pelican : Bird
{
    private static class PelAnimState
    {
        public const int Flapping = 0;
        public const int Diving = 1;
        public const int Down = 2;
    }
    
    [SerializeField] private Animator _pelicanAnimator;

    protected override BirdType MyBirdType => BirdType.Pelican;
    private int _currentTarIn;
    private bool _isDiving;
    private int _sideMultiplier;
    private float _moveSpeed = 2f;
    private Vector3[] _setPositions;

    private Vector3 TargetPosition => Constants.BalloonCenter.position + new Vector3(_sideMultiplier * _setPositions[_currentTarIn].x, _setPositions[_currentTarIn].y);

    protected override void Awake()
    {
        _pelicanAnimator.SetInteger(Constants.AnimState, Random.Range(0, 2));
        base.Awake();
        float yDistAbove = 2/4f;
        var yDistBelow = -2.2f/4f;
        var resolution = 0.1f;
        var totalPoints = (int) ((yDistAbove - yDistBelow) / resolution);
        _setPositions = new Vector3[totalPoints];
        for (var i = 0; i < totalPoints; i++)
        {
            float iFloat = i;
            var xPoint = -1f * Mathf.Cos(2f * Mathf.PI * (iFloat / totalPoints)) + 1f;
            xPoint *= 0.25f;
            var yPoint = -2.1f * Mathf.Cos(2f * Mathf.PI * (iFloat / (totalPoints * 2)));
            yPoint *= 0.25f;
            var thisVector = new Vector3(xPoint, yPoint, 0f);
            _setPositions[i] = thisVector;
        }

        StartCoroutine(SwoopAround());
    }

    //Move from one checkpoint to another
    private IEnumerator SwoopAround()
    {
        _pelicanAnimator.SetInteger(Constants.AnimState, PelAnimState.Flapping);
        _currentTarIn = 0;
        _sideMultiplier = transform.position.x < 0 ? 1 : -1;

        while (_currentTarIn < _setPositions.Length)
        {
            _rigbod.velocity = GetVelocity();
            var xFromJai = Constants.JaiTransform.position.x - transform.position.x;
            transform.FaceForward(xFromJai > 0);

            if (Vector3.Distance(transform.position, TargetPosition) < 0.03f)//todo: determine better feel for transitioning between targets when Jai is moving
            {
                _currentTarIn++;
                if (_currentTarIn >= _setPositions.Length)
                {
                    break;
                }
            }

            yield return null;
        }

        StartCoroutine(DiveBomb(_sideMultiplier < 0));
    }

    private Vector2 GetVelocity()
    {
        return Constants.SpeedMultiplier * _moveSpeed * (TargetPosition - transform.position).normalized;
    }

    //plunge to (un)certain balloon-popping glory
    private IEnumerator DiveBomb(bool goingRight)
    {
        _pelicanAnimator.SetInteger(Constants.AnimState, PelAnimState.Diving);
        var diveAngle = goingRight ? -80f : 260f;
        _rigbod.velocity = Constants.SpeedMultiplier * 6f * ConvertAnglesAndVectors.ConvertAngleToVector2(diveAngle);
        transform.FaceForward(_rigbod.velocity.x > 0);
        while (transform.position.y > -ScreenSpace.WorldEdge.y - 0.15f)
        {
            yield return null;
        }

        _rigbod.velocity = Vector2.zero;
        _birdCollider.enabled = false;
        yield return new WaitForSeconds(2f);
        StartCoroutine(SwoopAround());
        while (transform.position.y < -ScreenSpace.WorldEdge.y)
        {
            yield return null;
        }

        _birdCollider.enabled = true;
    }
}