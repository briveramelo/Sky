using UnityEngine;
using System.Collections;
using GenericFunctions;

public class Seagull : Bird
{
    [SerializeField] private GameObject _pooNugget;

    protected override BirdType MyBirdType => BirdType.Seagull;

    private const float _moveSpeed = 0.75f;
    private const float _ySpread = 0.15f;
    
    private static int _activePooCams;
    private static int _totalSeagulls = 0;

    private Vector2 _targetCenterPosition;
    private float _xSpread => ScreenSpace.WorldEdge.x * 0.8f;
    private float _shift;
    private float _startTime;
    private int _mySeagullNumber;
    private bool _startedMovingRight;
    private bool _movingRight;

    private Vector2[] _targetPositions;

    #region WakeUp

    protected override void Awake()
    {
        base.Awake();

        var screenHeight = ScreenSpace.WorldEdge.y;
        var screenHeightDrop = 0.18f + _ySpread;
        _targetPositions = new[]
        {
            new Vector2(0f, screenHeight - screenHeightDrop),
            new Vector2(.25f/4, screenHeight - screenHeightDrop),
            new Vector2(-.5f/4, screenHeight - screenHeightDrop),
            new Vector2(.5f/4, screenHeight - screenHeightDrop),
            new Vector2(-.25f/4, screenHeight - screenHeightDrop),
            new Vector2(0f, screenHeight - screenHeightDrop)
        };
        InitializeThisSeagull();
        StartCoroutine(GetIntoPlace());
        StartCoroutine(TargetPoop(EasyAccess.JaiTransform));
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
            _rigbod.velocity = _moveSpeed * (_targetCenterPosition - (Vector2) transform.position).normalized;
            if (Vector2.Distance(_targetCenterPosition, transform.position) < 0.03f)
            {
                StartCoroutine(SwoopOverhead());
                yield break;
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
        return _ySpread * Mathf.Sin(2f * Mathf.PI / 5f * (Time.time - _startTime)) + _targetCenterPosition.y;
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

    private IEnumerator TargetPoop(Transform target)
    {
        const float minPoopTimeDelay = 1f;
        var pooDistanceRange = 0.4f * new Vector2(1f, 1.5f);
        yield return new WaitForSeconds(minPoopTimeDelay);
        while (true)
        {
            var xDist = Mathf.Abs(transform.position.x - target.position.x);
            var lastTimePooped = 0f;
            if (xDist > pooDistanceRange[0] && xDist < pooDistanceRange[1] && Mathf.Sign(GetXVelocity()) == Mathf.Sign(target.position.x - transform.position.x))
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

        StartCoroutine(TargetPoop(target));
    }
}