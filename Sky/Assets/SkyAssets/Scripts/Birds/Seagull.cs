using UnityEngine;
using System.Collections;
using GenericFunctions;

public class Seagull : Bird
{
    [SerializeField] private GameObject _pooNugget;

    private bool _movingRight;

    private const float _moveSpeed = 3f;
    private static int _activePooCams;
    private static int _totalSeagulls = 0;

    private int _mySeagullNumber;
    private Vector2 _targetCenterPosition;
    private float _xSpread = 4;
    private bool _startedMovingRight;
    private float _shift;
    private float _startTime;

    private Vector2[] _targetPositions =
    {
        new Vector2(0f, 2.25f),
        new Vector2(.25f, 2.35f),
        new Vector2(-.5f, 2.25f),
        new Vector2(.5f, 2.25f),
        new Vector2(-.25f, 2.35f),
        new Vector2(0f, 2.35f),
    };

    #region WakeUp

    protected override void Awake()
    {
        base.Awake();

        InitializeThisSeagull();
        StartCoroutine(GetIntoPlace());
        StartCoroutine(Poop());
    }

    private void InitializeThisSeagull()
    {
        _mySeagullNumber = _totalSeagulls;
        _targetCenterPosition = _targetPositions[_mySeagullNumber % _targetPositions.Length];
        _movingRight = transform.position.x < _targetCenterPosition.x;
        _startedMovingRight = _movingRight;
        transform.FaceForward(!_movingRight);
    }

    #endregion

    private IEnumerator GetIntoPlace()
    {
        while (true)
        {
            _rigbod.velocity = (_targetCenterPosition - (Vector2) transform.position).normalized * _moveSpeed;
            if (Vector2.Distance(_targetCenterPosition, transform.position) < 0.1f)
            {
                StartCoroutine(SwoopOverhead());
                break;
            }

            yield return null;
        }
    }

    #region SwoopOverhead

    private IEnumerator SwoopOverhead()
    {
        _rigbod.velocity = Vector2.zero;
        _rigbod.isKinematic = true;
        _startTime = Time.time;
        StartCoroutine(LerpShift());
        while (true)
        {
            transform.FaceForward(GetXVelocity() < 0);
            transform.position = new Vector2(GetXPosition(), GetYPosition());
            yield return null;
        }
    }

    private IEnumerator LerpShift()
    {
        var targetShift = -22.5f;
        float xVel = 0;
        while (Mathf.Abs(_shift - targetShift) > 0.1f)
        {
            _shift = Mathf.SmoothDamp(_shift, targetShift, ref xVel, 2f);
            yield return new WaitForEndOfFrame();
        }

        _shift = targetShift;
    }

    private float GetYPosition()
    {
        return 0.6f * Mathf.Sin(2f * Mathf.PI / 5f * (Time.time - _startTime)) + _targetCenterPosition.y;
    }

    private float GetYVelocity()
    {
        return 0.6f * 2f * Mathf.PI / 5f * Mathf.Sin(2f * Mathf.PI / 5f * (Time.time - _startTime)) + _targetCenterPosition.y;
    }

    private float GetXPosition()
    {
        return (_startedMovingRight ? 1 : -1) * _xSpread * Mathf.Sin(2f * Mathf.PI / (5f * 2f) * (Time.time - _startTime) + Mathf.Deg2Rad * _shift) + _targetCenterPosition.x;
    }

    private float GetXVelocity()
    {
        return (_startedMovingRight ? 1 : -1) * 2f * Mathf.PI / (5f * 2f) * _xSpread * Mathf.Sign(Mathf.Cos(2f * Mathf.PI / (5f * 2f) * (Time.time - _startTime) + Mathf.Deg2Rad * _shift));
    }

    #endregion


    public static void LogPooCam(bool hit)
    {
        _activePooCams += hit ? 1 : -1;
    }

    private IEnumerator Poop()
    {
        var minPoopTimeDelay = 1f;
        var pooDistanceRange = new Vector2(1f, 1.5f) * 0.8f;
        yield return new WaitForSeconds(minPoopTimeDelay);
        while (true)
        {
            var xDist = Mathf.Abs(transform.position.x - Constants.JaiTransform.position.x);
            var lastTimePooped = 0f;
            if (xDist > pooDistanceRange[0] && xDist < pooDistanceRange[1] && Mathf.Sign(GetXVelocity()) == Mathf.Sign(-transform.position.x + Constants.JaiTransform.position.x))
            {
                if (_activePooCams < 5)
                {
                    if (Time.time > lastTimePooped + minPoopTimeDelay)
                    {
                        Instantiate(_pooNugget, transform.position, Quaternion.identity).GetComponent<PooNugget>().InitializePooNugget(new Vector2(GetXVelocity(), GetYVelocity()));
                        lastTimePooped = Time.time;
                        break;
                    }
                }
            }

            yield return null;
        }

        StartCoroutine(Poop());
    }
}