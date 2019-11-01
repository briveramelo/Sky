using UnityEngine;
using System.Collections;
using PixelArtRotation;
using GenericFunctions;

public interface IMurderToCrow
{
    void InitializeCrow(bool isKiller);
    void TakeFlight(Vector2 crowPosition);
    bool ReadyToFly { get; }
}

public class Crow : Bird, IMurderToCrow
{
    private static class CrowStates
    {
        public const int Flapping = 0;
        public const int Gliding = 1;
    }
    
    #region Initialize Variables
    protected override BirdType MyBirdType => BirdType.Crow;

    [SerializeField] private Murder _murder;
    [SerializeField] private PixelRotation _pixelRotationScript;
    [SerializeField] private Animator _crowAnimator;

    private ICrowToMurder _murderInterface;
    
    private Vector2 _startPosition;
    private Vector2 _moveDir;

    private float _moveSpeed = 4.5f;
    private float _turnDistance = 2.5f;
    private float _commitDistance = 4f;
    private float _resetDistance = 15f;
    private float _requestNextCrowDistance = 4.5f;
    private float _currentDistance;

    private bool _isKiller;
    private bool _readyToFly = true;
    private bool _hasRequestedNext;

    protected override void Awake()
    {
        base.Awake();
        _murderInterface = _murder;
    }
    
    #endregion

    #region IMurderToCrow Interface

    void IMurderToCrow.InitializeCrow(bool isKiller)
    {
        _isKiller = isKiller;
        _commitDistance = _isKiller ? _commitDistance : 3f;
    }

    void IMurderToCrow.TakeFlight(Vector2 crowPosition)
    {
        SetPosition(crowPosition);
        _hasRequestedNext = false;
        _readyToFly = false;
        _birdCollider.enabled = true;
        StartCoroutine(TargetBalloons());
        StartCoroutine(RequestNextCrow());
    }

    bool IMurderToCrow.ReadyToFly => _readyToFly;

    private void SetPosition(Vector2 crowPosition)
    {
        _startPosition = crowPosition;
        transform.position = crowPosition;
    }

    #endregion

    private IEnumerator RequestNextCrow()
    {
        while (_currentDistance > _requestNextCrowDistance)
        {
            yield return null;
        }

        yield return null;
        _murderInterface.SendNextCrow();
        _hasRequestedNext = true;
    }

    private IEnumerator TargetBalloons()
    {
        _currentDistance = 10f;
        while (_currentDistance > _commitDistance)
        {
            var balloonPos = Constants.BalloonCenter.position;
            var pos = transform.position;
            _currentDistance = Vector3.Distance(balloonPos, pos);
            _moveDir = (balloonPos - pos).normalized;
            Swoop();
            yield return null;
        }

        yield return StartCoroutine(BeeLine());
        StartCoroutine(TriggerReset());
        if (!_isKiller)
        {
            StartCoroutine(TurnAwayFromBalloons());
        }
    }

    private IEnumerator BeeLine()
    {
        while (_currentDistance > _turnDistance)
        {
            _currentDistance = Vector3.Distance(Constants.BalloonCenter.position, transform.position);
            yield return null;
        }
    }

    private IEnumerator TurnAwayFromBalloons()
    {
        _crowAnimator.SetInteger(Constants.AnimState, CrowStates.Gliding);
        var rotationSpeed = Bool.TossCoin() ? 4 : -4;
        var angleDelta = 0;
        var startAngle = ConvertAnglesAndVectors.ConvertVector2IntAngle(_rigbod.velocity, false);
        while (Mathf.Abs(angleDelta) < 60)
        {
            angleDelta += rotationSpeed;
            _moveDir = ConvertAnglesAndVectors.ConvertAngleToVector2(startAngle + angleDelta).normalized;
            Swoop();
            yield return null;
        }

        _crowAnimator.SetInteger(Constants.AnimState, CrowStates.Flapping);
    }

    #region Swoop Helper Functions

    private void Swoop()
    {
        _rigbod.velocity = Constants.SpeedMultiplier * _moveSpeed * _moveDir;
        _pixelRotationScript.Angle = ConvertAnglesAndVectors.ConvertVector2IntAngle(_rigbod.velocity);
        transform.FaceForward(_rigbod.velocity.x > 0);
    }

    private IEnumerator TriggerReset()
    {
        while (_currentDistance < _resetDistance)
        {
            _currentDistance = Vector3.Distance(Constants.BalloonCenter.position, transform.position);
            yield return null;
        }

        _rigbod.velocity = Constants.SpeedMultiplier * Vector2.zero;
        _birdCollider.enabled = false;
        transform.position = _startPosition;
        _readyToFly = true;
    }

    #endregion

    protected override void DieUniquely()
    {
        _murderInterface.ReportCrowDown(this);
        if (!_hasRequestedNext)
        {
            _murderInterface.SendNextCrow();
        }

        base.DieUniquely();
    }
}