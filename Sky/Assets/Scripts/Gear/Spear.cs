using UnityEngine;
using System.Collections;
using PixelArtRotation;
using GenericFunctions;

public class Spear : MonoBehaviour {

	public PixelRotation pixelRotationScript; //allows for pixel perfect sprite rotations
	public Jai jaiScript; //Our boy, Jai's Script
	public Joyfulstick joyfulstickScript; //joystickScript;

	public Rigidbody2D rigbod; //the spear's rigidbody
	public Transform spearTipParentTransform;
	public Transform spearTipTransform;
	public Transform jaiTransform; //jai's transform

	public CircleCollider2D spearTipCollider;

	public string spearString;

	public Vector3[] throwAdjustmentVector;
	public Vector3 stockPosition; //position the spear should return to when Jai reels it back
	
	public float time2Destroy;
	public float time2Reappear;
	public float bounceForce; //force at which the spear bounces back from the bird

	public int theSetAngle; //angle the spear releases as determined by the finger swipe direction

	public bool flying; //is the spear flying through the air?
	public bool throwing; //has throwing the spear commenced?

	// Use this for initialization
	void Awake () {
		bounceForce = 20f;
		time2Destroy = 3f;
		time2Reappear = Constants.timeToThrow;
		flying = false;
		throwing = false;
		spearString = "Prefabs/Gear/Spear";
		spearTipParentTransform = transform.GetChild (0);
		spearTipTransform = transform.GetChild (0).GetChild(0);
		spearTipCollider = spearTipTransform.GetComponent<CircleCollider2D> ();
		pixelRotationScript = GetComponent<PixelRotation> ();
		pixelRotationScript.Angle = 0;
		joyfulstickScript = GameObject.Find ("StickHole").GetComponent<Joyfulstick> ();
		jaiScript = GameObject.Find ("Jai").GetComponent<Jai> ();
		jaiTransform = jaiScript.transform;
		jaiScript.spearScript = GetComponent<Spear>();
		joyfulstickScript.spearScript = GetComponent<Spear>();
		stockPosition = transform.position - jaiTransform.position;
		throwAdjustmentVector = new Vector3[]{ 
			new Vector3 (0f, .26f,0f) * 2f,
			new Vector3 (0f, .31f,0f) * 2f
		};
		Invoke ("SetParent", 0.01f);
	}

	void SetParent(){
		transform.parent = jaiTransform;
		transform.localScale = Vector3.one;
	}

	void Update(){
		if (flying){
			pixelRotationScript.Angle = ConvertAnglesAndVectors.ConvertVector2SpearAngle(rigbod.velocity,rigbod.velocity.y);
			spearTipParentTransform.rotation = Quaternion.Euler(0f,0f,ConvertAnglesAndVectors.ConvertVector2SpearAngle(rigbod.velocity,rigbod.velocity.y));
		}
	}

	public IEnumerator FlyFree(Vector2 throwDir, float throwForce, int lowOrHighThrow){
		if (!throwing){
			throwing = true;
			theSetAngle = ConvertAnglesAndVectors.ConvertVector2SpearAngle(throwDir,throwDir.y);
			pixelRotationScript.Angle = theSetAngle;
			transform.position = jaiTransform.position + throwAdjustmentVector[lowOrHighThrow-1];
			spearTipCollider.enabled = true;
			StartCoroutine(NewSpear());
			Destroy (gameObject, time2Destroy);
			yield return new WaitForSeconds (Constants.timeToThrow/2);
			flying = true;
			jaiTransform.DetachChildren();
			gameObject.AddComponent<Rigidbody2D> ();
			rigbod = GetComponent<Rigidbody2D> ();
			rigbod.AddForce (throwDir * throwForce);
		}
		yield return null;
	}

	public IEnumerator NewSpear(){
		yield return new WaitForSeconds (time2Reappear);
		GameObject spear = Instantiate (Resources.Load (spearString), jaiTransform.position + stockPosition, Quaternion.identity) as GameObject;
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.layer == 16) { //if it hits a bird
			StartCoroutine(HurtBird(col,rigbod.velocity));
		}
		else if (col.gameObject.layer == 19){ //tentacle
			StartCoroutine (HurtTentacle(col,rigbod.velocity));
		}
	}

	void Bounce(){

	}

	public IEnumerator HurtBird(Collider2D birdCol, Vector2 gutVel){
		GetHurt getHurtScript = birdCol.GetComponent<GetHurt> ();
		StartCoroutine (getHurtScript.TakeDamage (gutVel,spearTipCollider));
		if (getHurtScript.health>0){ //bounce on hurting
			rigbod.velocity = 0.2f * Vector2.Reflect(gutVel,(transform.position-birdCol.bounds.ClosestPoint (transform.position)).normalized);
		}
		else{ //lose 20% speed on killing
			rigbod.velocity = gutVel * .8f;
		}
		yield return null;
	}

	public IEnumerator HurtTentacle(Collider2D tentaCol, Vector2 gutVel){
		Tentacles tentaclesScript = tentaCol.GetComponent<Tentacles> ();
		StartCoroutine (tentaclesScript.TakeDamage (gutVel,spearTipCollider));
		if (tentaclesScript.health>0){ //bounce on hurting
			rigbod.velocity = 0.2f * Vector2.Reflect(gutVel,(transform.position-tentaCol.bounds.ClosestPoint (transform.position)).normalized);
		}
		else{ //lose 20% speed on killing
			rigbod.velocity = gutVel * .8f;
		}
		yield return null;
	}

}
