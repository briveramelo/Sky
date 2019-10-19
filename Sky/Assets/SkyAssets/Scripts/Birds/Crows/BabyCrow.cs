using UnityEngine;
using System.Collections;
using GenericFunctions;

public class BabyCrow : Bird
{
    [SerializeField] private Animator _babyCrowAnimator;

    private Vector2[] basketOffsets = new Vector2[]
    {
        new Vector2(-.8f, 0.1f),
        new Vector2(.8f, 0.1f),
    };

    private Vector2 _moveDir;
    private float _dist2Target;

    private const float _triggerShiftDistance = 0.3f;
    private const float _moveSpeed = 2f;

    private int _currentShift;
    private int _shiftsHit;
    private const int _maxShifts = 5;
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

    private float CorrectSpeed => _dist2Target < 0.4f ? Mathf.Lerp(_rigbod.velocity.magnitude, 0f, Time.deltaTime * 2f) : _moveSpeed;

    private enum AnimState
    {
        Flying = 0,
        Looking = 1
    }

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
            _dist2Target = Vector2.Distance(jaiPos + (Vector3) basketOffsets[BasketOffsetIndex], pos);
            _moveDir = (jaiPos + (Vector3) basketOffsets[BasketOffsetIndex] - pos).normalized;
            _rigbod.velocity = _moveDir * CorrectSpeed;

            if (_dist2Target < _triggerShiftDistance && _shiftsHit == _currentShift)
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
        _shiftsHit++;
        _babyCrowAnimator.SetInteger(0, (int) AnimState.Looking);
        yield return StartCoroutine(LookBackAndForth(transform.position.x < Constants.BalloonCenter.position.x));
        _babyCrowAnimator.SetInteger(0, (int) AnimState.Flying);
        _currentShift++;
        BasketOffsetIndex++;
    }

    private IEnumerator FlyAway()
    {
        _dist2Target = Vector2.Distance(1.2f * Constants.WorldDimensions.x * Vector3.right, transform.position);

        while (_dist2Target > _triggerShiftDistance)
        {
            var pos = transform.position;
            _dist2Target = Vector2.Distance(1.2f * Constants.WorldDimensions.x * Vector3.right, pos);
            _moveDir = (1.2f * Constants.WorldDimensions.x * Vector3.right - pos).normalized;
            _rigbod.velocity = _moveDir * _moveSpeed;
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

    protected override void DieUniquely()
    {
        Incubator.Instance.SpawnNextBird(BirdType.Crow);
        base.DieUniquely();
    }
}