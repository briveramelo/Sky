using UnityEngine;
using System.Collections;
using GenericFunctions;

public class Shoebill : Bird {
	private IBumpable basket;
	private bool canHitBasket = true;
	private bool flying = true;
	private bool rightIsTarget;
	private bool movingRight;
	private int movingSign => movingRight ? 1 :-1;
	private float sinPeriodShift;

	protected override void Awake(){
		base.Awake();
		basket =FindObjectOfType<BasketEngine>().GetComponent<IBumpable>();
		sinPeriodShift = Random.Range(0f,5f);
        movingRight = transform.position.x < Constants.jaiTransform.position.x;
        rigbod.velocity = Vector2.right * movingSign * 0.01f;
	}

	private void Update () {
		if (flying){
			rigbod.velocity = (Vector2.up * FindYVelocity() + Vector2.right * FindXVelocity());
		}
	}

	private float FindYVelocity(){
		float yDistAway = transform.position.y-Constants.basketTransform.position.y;
		float periodLength = 4f;
		float sinOffset = 1f * Mathf.Sin(2*Mathf.PI * (1/(periodLength)) * (Time.timeSinceLevelLoad + sinPeriodShift));
		return Mathf.Clamp(-yDistAway,-1,1) + sinOffset;
	}

	private float moveSpeed {
		get{
            float xDist = Mathf.Abs(( rightIsTarget ? xEdge : -xEdge) - transform.position.x);
            return Mathf.Clamp(xDist, Mathf.Sign(xDist)*.5f, Mathf.Sign(xDist) * 5f);
		}
	}

	private float lastXPosition;
	private float xEdge = Constants.WorldDimensions.x * 1.2f;

	private float FindXVelocity(){
		bool outOfBounds = Mathf.Abs(transform.position.x)>xEdge;
		bool movingAway = movingSign == ((int)Mathf.Sign(rigbod.velocity.x));
		bool properSide = movingSign == ((int)Mathf.Sign(transform.position.x));

		bool reverseDirection = outOfBounds && movingAway && properSide;
		if (reverseDirection){
			movingRight = !movingRight;
            birdCollider.enabled = false;
		}
        if (Mathf.Abs(lastXPosition)>Constants.WorldDimensions.x && Mathf.Abs(transform.position.x) < Constants.WorldDimensions.x) {
            birdCollider.enabled = true;
        }

        if (lastXPosition<(-Constants.WorldDimensions.x/2f) && transform.position.x>-Constants.WorldDimensions.x/2f) {
            rightIsTarget = true;
        }
        else if (lastXPosition>(Constants.WorldDimensions.x/2f) && transform.position.x<Constants.WorldDimensions.x/2f) {
            rightIsTarget = false;
        }
        lastXPosition = transform.position.x;
		return Mathf.Lerp(rigbod.velocity.x,movingSign*moveSpeed,Time.deltaTime);
	}

	//for basket collision only
	private void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.layer == Constants.jaiLayer){
			if(canHitBasket){
				GameCamera.Instance.ShakeTheCamera();
                basket.Bump(1.5f * new Vector2 (rigbod.velocity.x,rigbod.velocity.y * 5f).normalized);
				StartCoroutine (Bool.Toggle(boolState=>canHitBasket=boolState,4f));
				StartCoroutine(Fall());
			}
		}
	}

	private IEnumerator Fall(){
		StartCoroutine(Bool.Toggle(boolState=>flying=boolState,2f));
        rigbod.velocity = new Vector2 (-rigbod.velocity.x, -Mathf.Abs(rigbod.velocity.y)).normalized * 2.5f;
		while (!flying){
            rigbod.velocity = Vector2.Lerp(rigbod.velocity,Vector2.zero,Time.deltaTime * 1.5f);
			yield return null;
		}
	}
}
