using UnityEngine;
using System.Collections;
using PixelArtRotation;
using GenericFunctions;

public class Spear : Weapon {

    int spearNumber;
    protected override int weaponNumber => spearNumber;
    private Rigidbody2D rigbod; //the spear's rigidbody, created only upon throwing
	[SerializeField] PixelRotation pixelRotationScript; //allows for pixel perfect sprite rotations
	[SerializeField] PixelPerfectSprite pixelPerfectSpriteScript;

	[SerializeField] Transform spearTipParentTransform;

	private Vector2[] throwAdjustmentVector = { 
		new Vector2 (0f, .085f*4f),
		new Vector2 (0f, .085f*4f)
	};

	const float bounceForce = 5f; //force at which the spear bounces back from the bird
    const float throwForce = 1400f; //Force with which Jai throws the spear

    protected override Vector2 MyVelocity => rigbod.velocity;

    void Start () {
        spearNumber = timesUsed;
        SetSpearAngle(Vector2.up);
		transform.parent = Constants.jaiTransform.parent;
	}

	void SetSpearAngle(Vector2 direction){
		int theSetAngle = ConvertAnglesAndVectors.ConvertVector2SpearAngle(direction);
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

    protected override void UseMe(Vector2 swipeDir){
        base.UseMe(swipeDir);
        swipeDir = swipeDir.normalized;
        transform.parent = null;
		transform.position = (Vector2)Constants.jaiTransform.position + throwAdjustmentVector[0];
		SetSpearAngle(swipeDir);
		attackCollider.enabled = true;
		rigbod = gameObject.AddComponent<Rigidbody2D> ();
		rigbod.AddForce (swipeDir * throwForce);
		StartCoroutine (TiltAround());
		Destroy (gameObject, 3f);
	}

	protected override void DeliverDamage(Collider2D col){
        base.DeliverDamage(col);

        Bird bird = col.GetComponent<Bird>();
		//Deliver damage and redirect the spear as a bounce
		//rigbod.velocity = bird.MyBirdStats.Health>0 ? 
		//	Vector2.Reflect(MyWeaponStats.Velocity,(transform.position-col.bounds.ClosestPoint (transform.position))) * 0.2f :
		//	MyWeaponStats.Velocity * .8f;

		Physics2D.IgnoreCollision(attackCollider, col);
	}

	void OnDestroy (){
		StopAllCoroutines ();
	}
}