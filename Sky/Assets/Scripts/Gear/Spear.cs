using UnityEngine;
using System.Collections;
using PixelArtRotation;
using GenericFunctions;

public class Spear : MonoBehaviour {

	public PixelRotation pixelRotationScript; //allows for pixel perfect sprite rotations
	public PixelPerfectSprite pixelPerfectSpriteScript;
	public Jai jaiScript; //Our boy, Jai's Script
	public Joyfulstick joyfulstickScript; //joystickScript;

	public Rigidbody2D rigbod; //the spear's rigidbody
	public Transform spearTipParentTransform;
	public Transform spearTipTransform;
	public Transform jaiTransform; //jai's transform

	public CircleCollider2D spearTipCollider;
	
	public Vector3[] throwAdjustmentVector;

	public float bounceForce; //force at which the spear bounces back from the bird

	public int theSetAngle; //angle the spear releases as determined by the finger swipe direction

	public bool flying; //is the spear flying through the air?
	public bool throwing; //has throwing the spear commenced?
	public bool firstTime;

	// Use this for initialization
	void Awake () {
		bounceForce = 5f;
		flying = true;
		throwing = false;
		firstTime = true;
		spearTipParentTransform = transform.GetChild (0);
		spearTipTransform = transform.GetChild (0).GetChild(0);
		spearTipCollider = spearTipTransform.GetComponent<CircleCollider2D> ();
		pixelRotationScript = GetComponent<PixelRotation> ();
		pixelPerfectSpriteScript = GetComponent<PixelPerfectSprite> ();
		pixelRotationScript.Angle = 0;
		joyfulstickScript = GameObject.Find ("StickHole").GetComponent<Joyfulstick> ();
		jaiScript = GameObject.Find ("Jai").GetComponent<Jai> ();
		jaiTransform = jaiScript.transform;
		jaiScript.spearScript = GetComponent<Spear>();
		joyfulstickScript.spearScript = GetComponent<Spear>();
		throwAdjustmentVector = new Vector3[]{ 
			new Vector3 (0f, .085f*4f,0f),
			new Vector3 (0f, .085f*4f,0f)
		};
		transform.parent = jaiTransform.parent;
	}


	public IEnumerator TiltAround (){
		//transform.Face4ward(jaiTransform.localScale.x>0);
		while (flying){
			if (firstTime){
				firstTime = false;
			}
			else{
				 theSetAngle = ConvertAnglesAndVectors.ConvertVector2SpearAngle(rigbod.velocity,rigbod.velocity.y);
			}
			pixelRotationScript.Angle = theSetAngle;
			spearTipParentTransform.rotation = Quaternion.Euler(0f,0f,theSetAngle);
			yield return null;
		}
		yield return null;
	}

	public IEnumerator FlyFree(Vector2 throwDir, float throwForce, int lowOrHighThrow){
		if (!throwing){
			throwing = true;
			theSetAngle = ConvertAnglesAndVectors.ConvertVector2SpearAngle(throwDir,throwDir.y);
			pixelRotationScript.Angle = theSetAngle;
			transform.position = jaiTransform.position + throwAdjustmentVector[lowOrHighThrow-1];
			spearTipCollider.enabled = true;
			StartCoroutine(NewSpear());
			Destroy (gameObject, Constants.timeToDestroySpear);
			yield return new WaitForSeconds (Constants.timeToThrowSpear/2);
			transform.parent = null;
			pixelPerfectSpriteScript.enabled = true;
			gameObject.AddComponent<Rigidbody2D> ();
			rigbod = GetComponent<Rigidbody2D> ();
			StartCoroutine (TiltAround());
			rigbod.AddForce (throwDir * throwForce);
		}
		yield return null;
	}

	public IEnumerator NewSpear(){
		yield return new WaitForSeconds (Constants.timeToThrowSpear);
		GameObject spear = Instantiate (Resources.Load (Constants.spearPrefab), jaiTransform.position + Constants.stockSpearPosition, Quaternion.identity) as GameObject;
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.layer == Constants.birdLayer || col.gameObject.layer == Constants.tentacleLayer) { //if it hits a bird
			StartCoroutine (HurtThings(col,rigbod.velocity));
		}
	}

	public IEnumerator HurtThings(Collider2D col, Vector2 gutVel){
		bool hitIt = true;
		GetHurt getHurtScript = col.GetComponent<GetHurt> ();
		foreach (Collider2D spearTip in getHurtScript.spearColliders){
			if (spearTipCollider == spearTip){
				hitIt = false;
				break;
			}
		}
		if (hitIt){
			StartCoroutine (getHurtScript.TakeDamage(gutVel,spearTipCollider, true));
			if (getHurtScript.health>0){ //bounce on hurting
				rigbod.velocity = 0.2f * Vector2.Reflect(gutVel,(transform.position-col.bounds.ClosestPoint (transform.position)).normalized);
			}
			else{ //lose 20% speed on killing
				rigbod.velocity = gutVel * .8f;
			}
		}
	
		yield return null;
	}

	void OnDestroy (){
		StopAllCoroutines ();
	}

}
