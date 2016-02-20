using UnityEngine;
using System.Collections;
using PixelArtRotation;
using GenericFunctions;

public class Spear : MonoBehaviour, IThrowable {

	public static IThrowable Instance;

	[SerializeField] private PixelRotation pixelRotationScript; //allows for pixel perfect sprite rotations
	[SerializeField] private PixelPerfectSprite pixelPerfectSpriteScript;

	[SerializeField] private Transform spearTipParentTransform;
	[SerializeField] private Transform spearTipTransform;
	[SerializeField] private CircleCollider2D spearTipCollider;
	
	private Rigidbody2D rigbod; //the spear's rigidbody, created only upon throwing

	private Vector2[] throwAdjustmentVector = new Vector2[]{ 
		new Vector2 (0f, .085f*4f),
		new Vector2 (0f, .085f*4f)
	};

	private float bounceForce = 5f; //force at which the spear bounces back from the bird

	private int theSetAngle; //angle the spear releases as determined by the finger swipe direction

	void Start () {
		Instance = this;

		SetSpearAngle(Vector2.up);
		transform.parent = Constants.jaiTransform.parent;
	}

	#region IThrowable
	IEnumerator IThrowable.FlyFree(Vector2 throwDir, float throwForce, int lowOrHighThrow){
		SetSpearAngle(throwDir);
		transform.position = (Vector2)Constants.jaiTransform.position + throwAdjustmentVector[lowOrHighThrow-1];
		StartCoroutine(Jai.SpearGenerator.PullOutNewSpear());
		Destroy (gameObject, 3f);
		yield return new WaitForSeconds (Constants.time2ThrowSpear/2);
		transform.parent = null;
		spearTipCollider.enabled = true;
		rigbod = gameObject.AddComponent<Rigidbody2D> ();
		rigbod.AddForce (throwDir * throwForce);
		StartCoroutine (TiltAround());
		yield return null;
	}
	#endregion

	void SetSpearAngle(Vector2 direction){
		theSetAngle = ConvertAnglesAndVectors.ConvertVector2SpearAngle(direction);
		pixelRotationScript.Angle = theSetAngle;
		spearTipParentTransform.rotation = Quaternion.Euler(0f,0f,theSetAngle);
	}

	IEnumerator TiltAround (){
		pixelPerfectSpriteScript.enabled = true;
		yield return null;
		while (true){
			SetSpearAngle(rigbod.velocity);
			yield return null;
		}
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.layer == Constants.birdLayer || col.gameObject.layer == Constants.tentacleLayer) { //if it hits a bird
			DeliverDamage(col);
		}
	}

	void DeliverDamage(Collider2D col){
		Vector2 gutVel = rigbod.velocity;
		IHurtable hurtInterface = col.GetComponent<IHurtable> ();

		//Deliver damage and redirect the spear as a bounce
		hurtInterface.TakeDamage(gutVel,spearTipCollider);
		rigbod.velocity = hurtInterface.MyBirdStats.Health>0 ? 
			Vector2.Reflect(gutVel,(transform.position-col.bounds.ClosestPoint (transform.position)).normalized) * 0.2f :
			gutVel * .8f;
		Physics2D.IgnoreCollision(spearTipCollider, col);
	}

	void OnDestroy (){
		StopAllCoroutines ();
	}
}
