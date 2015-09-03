using UnityEngine;
using System.Collections;
using PixelArtRotation;

public class Spear : MonoBehaviour {

	public bool flying; //is the spear flying through the air?
	public Rigidbody2D rigbod;
	public Vector2 hitVelocity; //speed the spear hits the bird
	public float bounceForce; //force at which the spear bounces back from the bird
	public Vector3 stockPosition; //position the spear should return to when Jai reels it back
	public Transform jaiTransform; //jai's transform
	public int theSetAngle; //angle the spear releases as determined by the finger swipe direction
	public float[] setAngles; //4 preset angles based on Jai's throwing sprites
	public PixelRotation pixelRotationScript; //allows for pixel perfect sprite rotations
	public int angleCount; //the integer for accessing the proper setAngle + rotationSpeeds when Jai prepares to throw
	public float[] rotationSpeeds; //speed at which Jai needs to rotate the spear to set its angle before throwing
	public Jai jaiScript; //Our boy, Jai's Script
	public Joyfulstick joyfulstickScript; //joystickScript;
	public float holdingDistance;
	public float time2Destroy;
	public float time2Reappear;
	public BoxCollider2D spearTipCollider;
	public string spearString;

	// Use this for initialization
	void Awake () {
		bounceForce = 20f;
		holdingDistance = .1f;
		time2Destroy = 1f;
		time2Reappear = .9f;
		flying = false;
		spearString = "Prefabs/Gear/Spear";
		spearTipCollider = GetComponent<BoxCollider2D> ();
		spearTipCollider.enabled = false;
		pixelRotationScript = GetComponent<PixelRotation> ();
		pixelRotationScript.Angle = 0;
		joyfulstickScript = GameObject.Find ("StickHole").GetComponent<Joyfulstick> ();
		jaiScript = GameObject.Find ("Jai").GetComponent<Jai> ();
		jaiTransform = jaiScript.transform;
		jaiScript.spearScript = this;
		joyfulstickScript.spearScript = this;
		stockPosition = transform.position - jaiTransform.position;
	}

	void Update(){
		if (!flying){
			transform.position = jaiTransform.position + stockPosition;
		}
	}

	public IEnumerator FlyFree(Vector2 throwDir, float throwForce){
		if (!flying){
			flying = true;

			theSetAngle = Mathf.RoundToInt (Mathf.Atan2 (throwDir.y, throwDir.x) * Mathf.Rad2Deg)-90;
			pixelRotationScript.Angle = theSetAngle;
			spearTipCollider.enabled = true;
			StartCoroutine(NewSpear());
			Destroy (gameObject, time2Destroy);
			yield return new WaitForSeconds (.45f);
			gameObject.AddComponent<Rigidbody2D> ();
			rigbod = GetComponent<Rigidbody2D> ();
			rigbod.AddForce (throwDir * throwForce);
		}
		yield return null;
	}

	public IEnumerator NewSpear(){
		yield return new WaitForSeconds (time2Reappear);
		GameObject spear = Instantiate (Resources.Load (spearString), jaiTransform.position + stockPosition, Quaternion.identity) as GameObject;
		spear.transform.parent = GameObject.Find ("BalloonBasket").transform;
		spear.transform.localScale = Vector3.one;
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.layer == 16) { //if it hits a bird
			hitVelocity = rigbod.velocity;
			StartCoroutine(HurtBird(col.gameObject,hitVelocity));
		}
	}

	public IEnumerator HurtBird(GameObject bird, Vector2 gutVel){
		GetHurt getHurtScript = bird.GetComponent<GetHurt> ();
		if (getHurtScript.health>1){ //bounce on hurting
			rigbod.velocity = Vector2.zero;
			rigbod.AddForce(-gutVel.normalized * bounceForce);
		}
		else{ //lose half speed on killing
			rigbod.velocity = gutVel * .5f;
		}
		StartCoroutine (getHurtScript.TakeDamage (gutVel));
		yield return null;
	}

}
