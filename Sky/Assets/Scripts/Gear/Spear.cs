using UnityEngine;
using System.Collections;
using PixelArtRotation;

public class Spear : MonoBehaviour {

	public bool flying; //is the spear flying through the air?
	public bool throwing; //has throwing the spear commenced?
	public Rigidbody2D rigbod; //the spear's rigidbody
	public Vector2 hitVelocity; //speed the spear hits the bird
	public float bounceForce; //force at which the spear bounces back from the bird
	public Vector3 stockPosition; //position the spear should return to when Jai reels it back
	public Transform jaiTransform; //jai's transform
	public int theSetAngle; //angle the spear releases as determined by the finger swipe direction
	public PixelRotation pixelRotationScript; //allows for pixel perfect sprite rotations
	public Jai jaiScript; //Our boy, Jai's Script
	public Joyfulstick joyfulstickScript; //joystickScript;
	public float time2Destroy;
	public float time2Reappear;
	public BoxCollider2D spearTipCollider;
	public string spearString;
	public Vector3[] throwAdjustmentVector;

	// Use this for initialization
	void Awake () {
		bounceForce = 20f;
		time2Destroy = 3f;
		time2Reappear = .9f;
		flying = false;
		throwing = false;
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
		throwAdjustmentVector = new Vector3[]{ 
			new Vector3 (0f, .26f,0f),
			new Vector3 (0f, .31f,0f)
		};
	}

	void Update(){
		if (flying){
			pixelRotationScript.Angle = ConvertVector2Angle(rigbod.velocity);
		}
	}

	int ConvertVector2Angle(Vector2 inputVector){
		return Mathf.RoundToInt (Mathf.Atan2 (inputVector.y, inputVector.x) * Mathf.Rad2Deg)-90;
	}

	public IEnumerator FlyFree(Vector2 throwDir, float throwForce, int lowOrHighThrow){
		if (!throwing){
			throwing = true;
			theSetAngle = ConvertVector2Angle(throwDir);
			pixelRotationScript.Angle = theSetAngle;
			transform.position = jaiTransform.position + throwAdjustmentVector[lowOrHighThrow-1];
			spearTipCollider.enabled = true;
			StartCoroutine(NewSpear());
			Destroy (gameObject, time2Destroy);
			yield return new WaitForSeconds (.45f);
			flying = true;
			gameObject.AddComponent<Rigidbody2D> ();
			rigbod = GetComponent<Rigidbody2D> ();
			rigbod.AddForce (throwDir * throwForce);
		}
		yield return null;
	}

	public IEnumerator NewSpear(){
		yield return new WaitForSeconds (time2Reappear);
		GameObject spear = Instantiate (Resources.Load (spearString), jaiTransform.position + stockPosition, Quaternion.identity) as GameObject;
		spear.transform.parent = GameObject.Find ("Jai").transform;
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
