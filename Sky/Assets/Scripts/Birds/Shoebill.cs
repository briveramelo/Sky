using UnityEngine;
using System.Collections;
using GenericFunctions;
public class Shoebill : Bird {

	IBumpable basket;
	Vector2[] fall = new Vector2[]{
		new Vector2(1,-.75f).normalized,
		new Vector2(-1,-.75f).normalized
	};
	bool canHitBasket = true;
	bool flying = true;
	const float standardSpeed = 3f;
	float moveSpeed {
		get{
			float xDist = Constants.basketTransform.position.x - transform.position.x;
			if (Mathf.Abs(xDist)<2f){
				if ((movingRight && xDist>0) || (!movingRight && xDist<0)){
					return 10f;
				}
				else{
					return 0.5f;
				}
			}
			else{
				return standardSpeed;
			}
		}
	}
	bool movingRight;
	int movingSign{get{return movingRight ? 1 :-1;}}
	float sinPeriodShift;

	protected override void Awake(){
		basket =FindObjectOfType<BasketEngine>().GetComponent<IBumpable>();
		sinPeriodShift = Random.Range(0,5f);
        movingRight = transform.position.x < Constants.jaiTransform.position.x;
        rigbod.velocity = Vector2.right * movingSign * 0.01f;
		birdStats = new BirdStats(BirdType.Shoebill);
		base.Awake();
	}
	
	void Update () {
		if (flying){
			rigbod.velocity = (Vector2.up * FindYVelocity() + Vector2.right * FindXVelocity());
		}
	}

	float FindYVelocity(){
		float yDistAway = transform.position.y-Constants.basketTransform.position.y;
		float periodLength = 4f;
		float sinOffset = 1f * Mathf.Sin(2*Mathf.PI * (1/(periodLength)) * (Time.timeSinceLevelLoad + sinPeriodShift));
		return Mathf.Clamp(-yDistAway,-1,1) + sinOffset;
	}

	float FindXVelocity(){
		bool outOfBounds = Mathf.Abs(transform.position.x)>(Constants.WorldDimensions.x+.2f);
		bool movingAway = movingSign == ((int)Mathf.Sign(rigbod.velocity.x));
		bool properSide = movingSign == ((int)Mathf.Sign(transform.position.x));
		bool reverseDirection = outOfBounds && movingAway && properSide;
		if (reverseDirection){
			movingRight = !movingRight;
		}

		return Mathf.Lerp(rigbod.velocity.x,movingSign*moveSpeed,Time.deltaTime);
	}

	//for basket collision only
	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.layer == Constants.jaiLayer){
			if(canHitBasket){
				StartCoroutine(Fall());
				StartCoroutine (Bool.Toggle(boolState=>canHitBasket=boolState,3f));
				GameCamera.Instance.ShakeTheCamera();
                basket.Bump(1.5f * new Vector2 (rigbod.velocity.x,rigbod.velocity.y * 5f).normalized);
			}
		}
	}

	IEnumerator Fall(){
		Vector2 targetVelocity = (rigbod.velocity.x >0f ?fall[0] : fall[1]) * standardSpeed * 0.3f;
		StartCoroutine(Bool.Toggle(boolState=>flying=boolState,2f));
		while (!flying){
			rigbod.velocity = Vector2.Lerp(rigbod.velocity,targetVelocity,Time.deltaTime);
			yield return null;
		}
	}
}
