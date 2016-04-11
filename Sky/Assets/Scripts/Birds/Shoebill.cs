using UnityEngine;
using System.Collections;
using GenericFunctions;
public class Shoebill : Bird {

	IDisableable basket;
	Vector2[] fall = new Vector2[]{
		new Vector2(1,-.5f).normalized,
		new Vector2(-1,-.5f).normalized
	};
	bool canHitBasket = true;
	bool flying = true;
	const float moveSpeed = 2f;
	bool movingRight;
	int movingSign{get{return movingRight ? 1 :-1;}}
	float sinPeriodShift;

	protected override void Awake(){
		basket =FindObjectOfType<BasketEngine>().GetComponent<IDisableable>();
		sinPeriodShift = Random.Range(0,5f);

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
		float periodLength = 3.95f;
		float sinOffset = .825f * Mathf.Sin(2*Mathf.PI * (1/(periodLength)) * (Time.timeSinceLevelLoad + sinPeriodShift));
		return Mathf.Clamp(-yDistAway,-1,1) + sinOffset;
	}

	float FindXVelocity(){
		float xDistAway = transform.position.x-Constants.basketTransform.position.x;

		if (Mathf.Abs(xDistAway)>4.2f && movingSign==((int)Mathf.Sign(rigbod.velocity.x)) && movingSign==((int)Mathf.Sign(xDistAway))){
			movingRight = !movingRight;
		}

		return Mathf.Lerp(rigbod.velocity.x,movingSign*moveSpeed,Time.deltaTime);
	}

	//for basket collision only
	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.layer == Constants.jaiLayer){
			if(canHitBasket){
				StartCoroutine (Fall());
				StartCoroutine (Bool.Toggle(boolState=>canHitBasket=boolState,3f));
				GameCamera.Instance.ShakeTheCamera();
				basket.DisableMovement();
			}
		}
	}

	IEnumerator Fall(){
		Vector2 fallVelocity = rigbod.velocity.x >0f ?fall[0] : fall[1];
		StartCoroutine(Bool.Toggle(boolState=>flying=boolState,2f));
		while (!flying){
			rigbod.velocity = fallVelocity;
			yield return null;
		}
	}
}
