using UnityEngine;
using System.Collections;
using GenericFunctions;

public class BabyCrow : Bird
{
    private static class AnimState
    {
        public const int Flying = 0;
        public const int Looking = 1;
    }
    
    [SerializeField] private Animator _babyCrowAnimator;
    protected override BirdType MyBirdType => BirdType.BabyCrow;
    
    private const float _triggerShiftDistance = 0.05f;
    private const float _moveSpeed = 2f;
    private const int _maxShifts = 5;
    
    private Vector2[] _basketOffsets = 
    {
        new Vector2(-.25f, 0.0f),
        new Vector2(.25f, 0.0f),
    };
    

    private Vector2 _moveDir;
    private float _dist2Target;
    private bool _isLooking;
    private int _currentShift;
    private int _shiftsHit;
    private int _basketOffsetIndex;

    private int BasketOffsetIndex
    {
        get => _basketOffsetIndex;
        set
        {
            _basketOffsetIndex = value;
            if (_basketOffsetIndex > 1)
            {
                _basketOffsetIndex = 0;
            }
        }
    }

    private bool _isCloseEnoughToShift => _dist2Target < _triggerShiftDistance;
    private float ClosenessCorrectedSpeed => _isLooking && _isCloseEnoughToShift ? Mathf.Lerp(_rigbod.velocity.magnitude, 0f, 0.033f) : _moveSpeed;

    protected override void Awake()
    {
        base.Awake();
        StartCoroutine(ApproachShifts());
    }

    private IEnumerator ApproachShifts()
    {
        transform.FaceForward(transform.position.x < Constants.BalloonCenter.position.x);
        while (_currentShift < _maxShifts)
        {
            var pos = transform.position;
            var jaiPos = Constants.JaiTransform.position;
            _dist2Target = Vector2.Distance(jaiPos + (Vector3) _basketOffsets[BasketOffsetIndex], pos);
            _moveDir = (jaiPos + (Vector3) _basketOffsets[BasketOffsetIndex] - pos).normalized;
            _rigbod.velocity = Constants.SpeedMultiplier * ClosenessCorrectedSpeed * _moveDir;

            if (_isCloseEnoughToShift && _shiftsHit == _currentShift)
            {
                StartCoroutine(ShiftSpots());
            }

            yield return null;
        }

        _rigbod.velocity = Vector2.zero;
        StartCoroutine(FlyAway());
    }

    private IEnumerator ShiftSpots()
    {
        _isLooking = true;
        _shiftsHit++;
        _babyCrowAnimator.SetInteger(Constants.AnimState, AnimState.Looking);
        yield return StartCoroutine(LookBackAndForth(transform.position.x < Constants.BalloonCenter.position.x));
        _babyCrowAnimator.SetInteger(Constants.AnimState, AnimState.Flying);
        _currentShift++;
        BasketOffsetIndex++;
        _isLooking = false;
    }

    private IEnumerator FlyAway()
    {
        Vector2 targetPoint = (Constants.ScreenSizeWorldUnits.x + 0.2f) * Vector2.right;
        
        while (Vector2.Distance(targetPoint, transform.position) > _triggerShiftDistance)
        {
            _moveDir = (targetPoint - (Vector2)transform.position).normalized;
            _rigbod.velocity = Constants.SpeedMultiplier * _moveSpeed * _moveDir;
            yield return null;
        }

        Destroy(gameObject);
    }

    private IEnumerator LookBackAndForth(bool faceDir)
    {
        for (var i = 0; i < 7; i++)
        {
            transform.FaceForward(faceDir);
            yield return new WaitForSeconds(Random.Range(0.33f, .75f));
            faceDir = !faceDir;
        }
    }

    protected override void OnDeath()
    {
        BirdFactory.Instance.CreateNextBird(BirdType.Crow);
        base.OnDeath();
    }
}