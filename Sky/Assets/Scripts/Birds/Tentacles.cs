using UnityEngine;
using System.Collections;
using GenericFunctions;

public class Tentacles : MonoBehaviour {

	public Transform tipTransform;
	public BoxCollider2D bottomOfTheWorldCollider;
	public CircleCollider2D dummyCollider;
	public Joyfulstick joyfulStickScript;
	public Basket basketScript;
	public Collider2D tentaCollider;
	public GetHurt getHurtScript;
	public ScreenShake screenShakeScript;
	public TentaclesSensor tentaclesSensorScript;
	public WorldEffects worldEffectsScript;

	public Transform basketTransform;

	public Rigidbody2D basketBody;
	public Rigidbody2D rigbod;

	public Vector3 jaiCorrection;

	public Vector2 homeSpot;
	
	public float descendSpeed;
	public float attackSpeed;
	public float resetSpeed;
	public float deathHeight;
	public float resetHeight;

	public int stabsTaken;
	public int stabs2Retreat;

	public bool holding;
	public bool resetting;
	public bool hurt;
	public bool attacking;
	public bool killedHim;

	void Awake(){
		bottomOfTheWorldCollider = GameObject.Find ("BottomWall").GetComponent<BoxCollider2D> ();
		tipTransform = transform.GetChild (0);
		stabs2Retreat = 6;
		descendSpeed = .75f;
		attackSpeed = 1.5f;
		resetSpeed = 1f;
		jaiCorrection = new Vector3 (0f,-.3f,0f);
		homeSpot = new Vector2 (0f,-.75f - Constants.worldDimensions.y);
		resetHeight = .5f + homeSpot.y;
		deathHeight = .25f + homeSpot.y;
		rigbod = GetComponent<Rigidbody2D> ();
		tentaCollider = GetComponent<Collider2D> ();
		getHurtScript = GetComponent<GetHurt> ();
		basketBody = GameObject.Find ("BalloonBasket").GetComponent<Rigidbody2D> ();
		basketScript = GameObject.Find ("BalloonBasket").GetComponent<Basket> ();
		worldEffectsScript = GameObject.Find ("WorldBounds").GetComponent<WorldEffects> ();
		joyfulStickScript = GameObject.Find ("StickHole").GetComponent<Joyfulstick> ();
		screenShakeScript = GameObject.Find ("mainCam").GetComponent<ScreenShake> ();
		basketTransform = GameObject.Find ("Basket").transform;
		GameObject.Find ("Jai").GetComponent<Jai> ().tentaclesScript = this;
	}

	void FaceTowardYou(bool toward){
		transform.Face4ward (toward ? (basketTransform.position.x - transform.position.x) > 0 : (basketTransform.position.x - transform.position.x) < 0);
	}

	public IEnumerator GoForTheKill(){ 
		tentaCollider.enabled = true;
		attacking = true;
		resetting = false;
		while (!holding && attacking && !resetting){
			rigbod.velocity = (basketTransform.position - tipTransform.position).normalized * attackSpeed;
			FaceTowardYou(true);
			yield return null;
		}
		attacking = false;
		yield return null;
	}

	public IEnumerator ResetPosition(){
		resetting = true;
		attacking = false;
		while (!attacking && resetting){
			rigbod.velocity = ((Vector3)homeSpot - tipTransform.position).normalized * resetSpeed;
			FaceTowardYou(true);
			if (tipTransform.position.y<resetHeight){
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
		worldEffectsScript.SlowTime (0.4f, 0.5f);
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
			joyfulStickScript.basketBody.AddForce(Vector2.down * 5f);
		}
		yield return null;
	}

	public IEnumerator PullBackTheKill(){
		stabsTaken = 0;
		rigbod.velocity = Vector2.zero;
		basketBody.velocity = Vector2.zero;
		bottomOfTheWorldCollider.enabled = false;
		while (holding){
			rigbod.velocity = Vector2.down * descendSpeed;
			basketBody.velocity = Vector2.down * descendSpeed;
			if (tipTransform.position.y<deathHeight){ //kill him
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
		bottomOfTheWorldCollider.enabled = true;
		yield return null;
	}

	public IEnumerator RetreatToDefeat(){
		stabsTaken = 0;
		while (resetting){
			rigbod.velocity = ((Vector3)homeSpot - tipTransform.position).normalized * resetSpeed;
			FaceTowardYou(false);
			if (tipTransform.position.y<deathHeight){
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
		if (bottomOfTheWorldCollider!=null){
			bottomOfTheWorldCollider.enabled = true;
		}
		if (tentaclesSensorScript!=null){
			Destroy (tentaclesSensorScript.gameObject);
		}
	}
}
