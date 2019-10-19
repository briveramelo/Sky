using UnityEngine;
using System.Collections;
using GenericFunctions;

public class Shoebill : Bird {
	private IBumpable _basket;
	private bool _canHitBasket = true;
	private bool _flying = true;
	private bool _rightIsTarget;
	private bool _movingRight;
	private int MovingSign => _movingRight ? 1 :-1;
	private float _sinPeriodShift;
	
	private float _lastXPosition;
	private float _xEdge = Constants.WorldDimensions.x * 1.2f;

	protected override void Awake(){
		base.Awake();
		_sinPeriodShift = Random.Range(0f,5f);
        _movingRight = transform.position.x < Constants.JaiTransform.position.x;
        _rigbod.velocity = Vector2.right * MovingSign * 0.01f;
	}

	private void Start()
	{
		_basket =FindObjectOfType<BasketEngine>().GetComponent<IBumpable>();
	}

	private void Update () {
		if (_flying){
			_rigbod.velocity = (Vector2.up * FindYVelocity() + Vector2.right * FindXVelocity());
		}
	}

	private float FindYVelocity(){
		float yDistAway = transform.position.y-Constants.BasketTransform.position.y;
		float periodLength = 4f;
		float sinOffset = 1f * Mathf.Sin(2*Mathf.PI * (1/(periodLength)) * (Time.timeSinceLevelLoad + _sinPeriodShift));
		return Mathf.Clamp(-yDistAway,-1,1) + sinOffset;
	}

	private float MoveSpeed {
		get{
            float xDist = Mathf.Abs(( _rightIsTarget ? _xEdge : -_xEdge) - transform.position.x);
            return Mathf.Clamp(xDist, Mathf.Sign(xDist)*.5f, Mathf.Sign(xDist) * 5f);
		}
	}

	private float FindXVelocity(){
		bool outOfBounds = Mathf.Abs(transform.position.x)>_xEdge;
		bool movingAway = MovingSign == ((int)Mathf.Sign(_rigbod.velocity.x));
		bool properSide = MovingSign == ((int)Mathf.Sign(transform.position.x));

		bool reverseDirection = outOfBounds && movingAway && properSide;
		if (reverseDirection){
			_movingRight = !_movingRight;
            _birdCollider.enabled = false;
		}
        if (Mathf.Abs(_lastXPosition)>Constants.WorldDimensions.x && Mathf.Abs(transform.position.x) < Constants.WorldDimensions.x) {
            _birdCollider.enabled = true;
        }

        if (_lastXPosition<(-Constants.WorldDimensions.x/2f) && transform.position.x>-Constants.WorldDimensions.x/2f) {
            _rightIsTarget = true;
        }
        else if (_lastXPosition>(Constants.WorldDimensions.x/2f) && transform.position.x<Constants.WorldDimensions.x/2f) {
            _rightIsTarget = false;
        }
        _lastXPosition = transform.position.x;
		return Mathf.Lerp(_rigbod.velocity.x,MovingSign*MoveSpeed,Time.deltaTime);
	}

	//for _basket collision only
	private void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.layer == Constants.JaiLayer){
			if(_canHitBasket){
				GameCamera.Instance.ShakeTheCamera();
                _basket.Bump(1.5f * new Vector2 (_rigbod.velocity.x,_rigbod.velocity.y * 5f).normalized);
				StartCoroutine (Bool.Toggle(boolState=>_canHitBasket=boolState,4f));
				StartCoroutine(Fall());
			}
		}
	}

	private IEnumerator Fall(){
		StartCoroutine(Bool.Toggle(boolState=>_flying=boolState,2f));
        _rigbod.velocity = new Vector2 (-_rigbod.velocity.x, -Mathf.Abs(_rigbod.velocity.y)).normalized * 2.5f;
		while (!_flying){
            _rigbod.velocity = Vector2.Lerp(_rigbod.velocity,Vector2.zero,Time.deltaTime * 1.5f);
			yield return null;
		}
	}
}
