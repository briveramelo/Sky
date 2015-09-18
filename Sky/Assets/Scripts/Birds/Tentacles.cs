using UnityEngine;
using System.Collections;
using GenericFunctions;

public class Tentacles : MonoBehaviour {

	public CircleCollider2D dummyCollider;
	public TimeEffects timeEffectsScript;
	public Joyfulstick joyfulStickScript;
	public Basket basketScript;
	public Collider2D tentaCollider;
	public GetHurt getHurtScript;
	public ScreenShake screenShakeScript;
	public TentaclesSensor tentaclesSensorScript;

	public Transform jaiTransform;

	public Rigidbody2D basketBody;
	public Rigidbody2D rigbod;

	public Vector3 jaiCorrection;
	
	public float descendSpeed;
	public float attackSpeed;
	public float resetSpeed;
	public float boundaryHeight;
	public float homeHeight;
	public float deathHeight;

	public int stabsTaken;
	public int stabs2Retreat;

	public bool holding;
	public bool resetting;
	public bool hurt;
	public bool attacking;
	public bool killedHim;

	void Awake(){
		stabs2Retreat = 6;
		descendSpeed = .75f;
		attackSpeed = 1.5f;
		resetSpeed = 1f;
		boundaryHeight = -2f;
		jaiCorrection = new Vector3 (0f,-.3f,0f);
		homeHeight = Constants.tentacleHomeSpot.y + Constants.tentacleTipOffset.y;
		deathHeight = homeHeight - 1.2f;
		rigbod = GetComponent<Rigidbody2D> ();
		tentaCollider = GetComponent<Collider2D> ();
		getHurtScript = GetComponent<GetHurt> ();
		basketBody = GameObject.Find ("BalloonBasket").GetComponent<Rigidbody2D> ();
		basketScript = GameObject.Find ("BalloonBasket").GetComponent<Basket> ();
		timeEffectsScript = GameObject.Find ("Dummy").GetComponent<TimeEffects> ();
		joyfulStickScript = GameObject.Find ("StickHole").GetComponent<Joyfulstick> ();
		screenShakeScript = GameObject.Find ("mainCam").GetComponent<ScreenShake> ();
		jaiTransform = GameObject.Find ("Jai").transform;
		jaiTransform.GetComponent<Jai> ().tentaclesScript = this;
	}

	void FaceTowardYou(){
		if ((jaiTransform.position.x - transform.position.x)>0){
			transform.localScale = Constants.Pixel625(true);
		}
		else{
			transform.localScale = Constants.Pixel625(false);
		}
	}

	void FaceAwayFromYou(){
		if ((jaiTransform.position.x - transform.position.x)<0){
			transform.localScale = Constants.Pixel625(true);
		}
		else{
			transform.localScale = Constants.Pixel625(false);
		}
	}

	public IEnumerator GoForTheKill(){ 
		tentaCollider.enabled = true;
		attacking = true;
		resetting = false;
		while (!holding && attacking && !resetting){
			rigbod.velocity = (jaiTransform.position + jaiCorrection - (transform.position + Constants.tentacleTipOffset)).normalized * attackSpeed;
			FaceTowardYou();
			yield return null;
		}
		attacking = false;
		yield return null;
	}

	public IEnumerator ResetPosition(){
		resetting = true;
		attacking = false;
		while (!attacking && resetting){
			rigbod.velocity = (Constants.tentacleHomeSpot - (transform.position + Constants.tentacleTipOffset)).normalized * resetSpeed;
			FaceTowardYou();
			if ((transform.position.y + Constants.tentacleTipOffset.y)<homeHeight){
				resetting = false;
				rigbod.velocity = Vector2.zero;
			}
			yield return null;
		}
		resetting = false;
		yield return null;
	}

	void OnTriggerEnter2D(Collider2D col){
		if (attacking && col.gameObject.layer == Constants.jaiLayer){
			StartCoroutine (SnatchAndGrab());
		}
	}

	//prevent him from moving, throwing spears
	//only allow him to stab
	//start the descent
	public IEnumerator SnatchAndGrab(){
		holding = true;
		resetting = false;
		hurt = false;
		attacking = false;
		joyfulStickScript.beingHeld = true;
		timeEffectsScript.SlowTime (0.4f, 0.5f);
		StartCoroutine ( screenShakeScript.CameraShake ());
		StartCoroutine (PullBackTheKill());
		yield return null;
	}

	public IEnumerator TakeStabs(){
		stabsTaken++;
		StartCoroutine (getHurtScript.TakeDamage(Vector2.zero,dummyCollider,false));
		if (stabsTaken>stabs2Retreat){
			holding = false;
			resetting = true;
			hurt = true;
			attacking = false;
			StartCoroutine (RetreatToDefeat());
			joyfulStickScript.beingHeld = false;
		}
		yield return null;
	}

	public IEnumerator PullBackTheKill(){
		stabsTaken = 0;
		while (holding){
			rigbod.velocity = Vector2.down * descendSpeed;
			basketBody.velocity = Vector2.down * descendSpeed;
			if ((transform.position.y + Constants.tentacleTipOffset.y)<deathHeight){ //kill him
				StartCoroutine (StripTheBalloons());
				basketBody.velocity = Vector2.zero;
				holding = false;
				resetting = false;
				hurt = false;
				attacking = false;
				killedHim = true;
			}
			yield return null;
		}
		yield return null;
	}

	public IEnumerator RetreatToDefeat(){
		stabsTaken = 0;
		while (resetting){
			rigbod.velocity = (Constants.tentacleHomeSpot - (transform.position + Constants.tentacleTipOffset)).normalized * resetSpeed;
			FaceAwayFromYou();
			if ((transform.position.y + Constants.tentacleTipOffset.y)<deathHeight){
				holding = false;
				resetting = false;
				attacking = false;
				rigbod.velocity = Vector2.zero;
				tentaCollider.enabled = false;
			}
			yield return null;
		}
		yield return new WaitForSeconds (3f);
		hurt = false;
	}

	public IEnumerator StripTheBalloons(){
		foreach (Balloon balloonScript in basketScript.balloonScripts){ //float the balloons
			if (balloonScript){
				balloonScript.transform.parent = null;
				balloonScript.moving = true;
				StartCoroutine (balloonScript.MoveUp());
			}
		}
		basketScript.balloonCount = 0;
		StartCoroutine (basketScript.CheckForEndTimes());
		yield return null;
	}

	public IEnumerator StopThemAll(){
		StopAllCoroutines ();
		yield return null;
	}

	void OnDestroy(){
		if (tentaclesSensorScript){
			Destroy (tentaclesSensorScript.gameObject);
		}
	}
}
