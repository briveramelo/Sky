using UnityEngine;
using System.Collections;
using BRM.DebugAdapter;
using BRM.DebugAdapter.Interfaces;
using PixelArtRotation;
using GenericFunctions;

public interface IMurderToCrow
{
    void InitializeCrow(bool isKiller);
    void TakeFlight(Vector2 crowPosition);
    bool ReadyToFly { get; }
}

//the crow follows this behavior
//1. Move toward the balloons
//2a. If killer and close enough, move in a straight line
//2b. If not killer, bend away from the balloons
//3. Carry on and reset

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

    private IDebug _debugger = new UnityDebugger {Enabled = false};
    private ICrowToMurder _murderInterface;

    private Transform _target;
    private Vector2 _startPosition;

    private const float _moveSpeed = 4.5f /4f;
    private const float _turnDistance = 2.5f/4;
    private const float _distanceBeforeDiving = 4f/4;
    private const float _resetDistance = 15f/4;
    private const float _requestNextCrowDistance = 4.5f/4;
    private float _distFromBalloons => Vector3.Distance(_target.position, transform.position);

    private bool _isKiller;
    private bool _readyToFly = true;
    private bool _hasRequestedNext;
    private float _distanceBeforeDeciding;

    protected override void Awake()
    {
        base.Awake();
        _murderInterface = _murder;
    }

    private void Start()
    {
        _target = EasyAccess.BalloonCenter;
    }

    #endregion

    #region IMurderToCrow Interface

    void IMurderToCrow.InitializeCrow(bool isKiller)
    {
        _isKiller = isKiller;
        _distanceBeforeDeciding = _isKiller ? _distanceBeforeDiving : 3f/4;
    }

    void IMurderToCrow.TakeFlight(Vector2 crowPosition)
    {
        _startPosition = crowPosition;
        transform.position = crowPosition;
        
        _hasRequestedNext = false;
        _readyToFly = false;
        _birdCollider.enabled = true;
        StartCoroutine(TargetBalloons(_target));
        StartCoroutine(RequestNextCrow());
    }

    bool IMurderToCrow.ReadyToFly => _readyToFly;

    #endregion

    private IEnumerator RequestNextCrow()
    {
        while (_distFromBalloons > _requestNextCrowDistance)
        {
            yield return null;
        }

        yield return null;
        _murderInterface.SendNextCrow();
        _hasRequestedNext = true;
    }

    private IEnumerator TargetBalloons(Transform target)
    {
        _debugger.LogFormat("targeting ", name);
        while (_distFromBalloons > _distanceBeforeDeciding)
        {
            var balloonPos = target.position;
            var pos = transform.position;
            var moveDir = (balloonPos - pos);
            PointAndMoveInMoveDir(moveDir);
            yield return null;
        }
        _debugger.LogFormat("moving straight ", name);
        yield return StartCoroutine(MoveStraightInLastDirection());
        _debugger.LogFormat("triggering reset ", name);
        StartCoroutine(TriggerReset());
        if (!_isKiller)
        {
            _debugger.LogFormat("turning away {0}", name);
            StartCoroutine(TurnAwayFromBalloons());
        }
    }

    private IEnumerator MoveStraightInLastDirection()
    {
        while (_distFromBalloons > _turnDistance)
        {
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
            var moveDir = ConvertAnglesAndVectors.ConvertAngleToVector2(startAngle + angleDelta);
            PointAndMoveInMoveDir(moveDir);
            yield return null;
        }
        _debugger.LogFormat("finished turning away {0}", name);
        _crowAnimator.SetInteger(Constants.AnimState, CrowStates.Flapping);
    }

    #region PointAndMoveInMoveDir Helper Functions

    private void PointAndMoveInMoveDir(Vector2 moveDir)
    {
        _rigbod.velocity = _moveSpeed * moveDir.normalized;
        _pixelRotationScript.Angle = ConvertAnglesAndVectors.ConvertVector2IntAngle(_rigbod.velocity);
        transform.FaceForward(_rigbod.velocity.x > 0);
    }

    private IEnumerator TriggerReset()
    {
        while (_distFromBalloons < _resetDistance)
        {
            yield return null;
        }
        _debugger.LogFormat("resetting {0}", name);

        _rigbod.velocity = Vector2.zero;
        _birdCollider.enabled = false;
        transform.position = _startPosition;
        _readyToFly = true;
    }

    #endregion

    protected override void OnDeath()
    {
        _murderInterface.ReportCrowDown(this);
        if (!_hasRequestedNext)
        {
            _murderInterface.SendNextCrow();
        }

        base.OnDeath();
    }
}