using UnityEngine;
using System.Collections;
using PixelArtRotation;
using GenericFunctions;

public interface IThrowable {
	IEnumerator FlyFree(Vector2 throwDir, float throwForce, int lowOrHighThrow);
}

public class SpearItems{
	public SpearItems(Collider2D spearCollider, Vector2 spearVelocity, int birdsHit){
		this.spearCollider = spearCollider;
		this.spearVelocity = spearVelocity;
		this.birdsHit = birdsHit;
	}
	public SpearItems(){}
	Collider2D spearCollider; public Collider2D SpearCollider{get{return spearCollider;}}
	Vector2 spearVelocity; public Vector2 SpearVelocity{get{return spearVelocity;}}
	int birdsHit; public int BirdsHit {get{return birdsHit;}}
}

public class Spear : MonoBehaviour, IThrowable {

	private static int totalSpearCount;
	private int mySpearNumber;

	[SerializeField] private PixelRotation pixelRotationScript; //allows for pixel perfect sprite rotations
	[SerializeField] private PixelPerfectSprite pixelPerfectSpriteScript;

	[SerializeField] private Transform spearTipParentTransform;
	[SerializeField] private Transform spearTipTransform;
	[SerializeField] private CircleCollider2D spearTipCollider;

	private SpearItems myItems;

	private int birdsHit;

	private Rigidbody2D rigbod; //the spear's rigidbody, created only upon throwing

	private Vector2[] throwAdjustmentVector = new Vector2[]{ 
		new Vector2 (0f, .085f*4f),
		new Vector2 (0f, .085f*4f)
	};

	private float bounceForce = 5f; //force at which the spear bounces back from the bird

	private int theSetAngle; //angle the spear releases as determined by the finger swipe direction

	void Start () {
		mySpearNumber=totalSpearCount;
		totalSpearCount++;
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

	//physics checks for bird/tentacle layers only
	void OnTriggerEnter2D(Collider2D col){
		DeliverDamage(col);
	}

	void DeliverDamage(Collider2D col){
		birdsHit++;
		myItems = new SpearItems(spearTipCollider, rigbod.velocity, birdsHit);
		ScoreSheet.Streaker.ReportHit(mySpearNumber);

		IHurtable hurtInterface = col.GetComponent<IHurtable> ();
		hurtInterface.GetHurt(myItems);

		Bird bird = col.GetComponent<Bird>();
		//Deliver damage and redirect the spear as a bounce
		rigbod.velocity = bird.MyBirdStats.Health>0 ? 
			Vector2.Reflect(myItems.SpearVelocity,(transform.position-col.bounds.ClosestPoint (transform.position))) * 0.2f :
			myItems.SpearVelocity * .8f;

		Physics2D.IgnoreCollision(spearTipCollider, col);
	}

	void OnDestroy (){
		StopAllCoroutines ();
	}
}
