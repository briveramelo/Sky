using UnityEngine;
using System.Collections;
using GenericFunctions;

public class Shoebill : Bird
{
    protected override BirdType MyBirdType => BirdType.Shoebill;
    private IBumpable _basket;
    private bool _canCollide = true;
    private bool _flying = true;
    private bool _rightIsTarget;
    private bool _movingRight;
    private int _movingSign => _movingRight ? 1 : -1;
    private float _sinPeriodShift;

    private float _lastXPosition;
    private float _xEdge;

    protected override void Awake()
    {
        base.Awake();
        _xEdge = ScreenSpace.WorldEdge.x + 0.2f;
        _sinPeriodShift = Random.Range(0f, 5f);
        _movingRight = transform.position.x < Constants.JaiTransform.position.x;
        _rigbod.velocity = Constants.SpeedMultiplier * 0.01f * _movingSign * Vector2.right;
    }

    private void Start()
    {
        _basket = FindObjectOfType<BasketEngine>().GetComponent<IBumpable>();
    }

    private void Update()
    {
        if (_flying)
        {
            _rigbod.velocity = Constants.SpeedMultiplier * FindYVelocity() * Vector2.up + Vector2.right * FindXVelocity();
        }
    }
    
    //for _basket collision only
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (_canCollide && col.gameObject.layer == Layers.JaiLayer)
        {
            GameCamera.Instance.ShakeTheCamera();
            var vel = _rigbod.velocity;
            _basket.Bump(1.5f * new Vector2(vel.x, vel.y * 5f).normalized);
            StartCoroutine(Bool.Toggle(boolState => _canCollide = boolState, 4f));
            StartCoroutine(Fall());
        }
    }

    private float FindYVelocity()
    {
        var yDistAway = transform.position.y - Constants.BasketTransform.position.y;
        var periodLength = 1f;
        var sinOffset = 1f * Mathf.Sin(2 * Mathf.PI * (1 / periodLength) * (Time.timeSinceLevelLoad + _sinPeriodShift));
        return Mathf.Clamp(-yDistAway, -1, 1) + sinOffset;
    }

    private float MoveSpeed
    {
        get
        {
            var xDist = Mathf.Abs((_rightIsTarget ? _xEdge : -_xEdge) - transform.position.x);
            return Mathf.Clamp(xDist, Mathf.Sign(xDist) * .5f, Mathf.Sign(xDist) * 5f);
        }
    }

    private float FindXVelocity()
    {
        var pos = transform.position;
        var outOfBounds = Mathf.Abs(pos.x) > _xEdge;
        var movingAway = _movingSign == (int) Mathf.Sign(_rigbod.velocity.x);
        var properSide = _movingSign == (int) Mathf.Sign(pos.x);

        var reverseDirection = outOfBounds && movingAway && properSide;
        if (reverseDirection)
        {
            _movingRight = !_movingRight;
            _birdCollider.enabled = false;//disallow off screen hits
        }

        var worldEdge = ScreenSpace.WorldEdge;
        if (Mathf.Abs(_lastXPosition) > worldEdge.x && Mathf.Abs(pos.x) < worldEdge.x)
        {
            _birdCollider.enabled = true;
        }

        if (_lastXPosition < -worldEdge.x / 2f && pos.x > -worldEdge.x / 2f)
        {
            _rightIsTarget = true;
        }
        else if (_lastXPosition > worldEdge.x / 2f && pos.x < worldEdge.x / 2f)
        {
            _rightIsTarget = false;
        }

        _lastXPosition = pos.x;
        return Mathf.Lerp(_rigbod.velocity.x, _movingSign * MoveSpeed, 0.0167f);
    }

    private IEnumerator Fall()
    {
        StartCoroutine(Bool.Toggle(boolState => _flying = boolState, 2f));
        var vel = _rigbod.velocity;
        _rigbod.velocity = Constants.SpeedMultiplier * 2.5f * new Vector2(-vel.x, -Mathf.Abs(vel.y)).normalized;
        while (!_flying)
        {
            _rigbod.velocity = Constants.SpeedMultiplier * Vector2.Lerp(_rigbod.velocity, Vector2.zero, 0.025f);
            yield return null;
        }
    }
}