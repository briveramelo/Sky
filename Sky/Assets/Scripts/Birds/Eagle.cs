using UnityEngine;
using System.Collections;
using PixelArtRotation;
using GenericFunctions;

public class Eagle : MonoBehaviour {

	public PixelRotation pixelRotationScript;

	public Rigidbody2D rigbod;
	public Transform jaiTransform;
	public SpriteRenderer eagleSprite;

	public Vector3 attackDir;

	public Vector2[] startPos;
	public Vector2[] moveDir;

	public float moveSpeed;
	public float attackSpeed;
	public float xSpace;


	// Use this for initialization
	void Awake () {
		pixelRotationScript = GetComponent<PixelRotation> ();
		moveSpeed = 5f;
		attackSpeed = 9f;
		xSpace = Constants.worldDimensions.x * 1.3f;
		rigbod = GetComponent<Rigidbody2D> ();
		jaiTransform = GameObject.Find ("Jai").transform;
		startPos = new Vector2[]{
			new Vector2(-Constants.worldDimensions.x,-Constants.worldDimensions.y) * 1.2f,
			new Vector2(Constants.worldDimensions.x,-Constants.worldDimensions.y) * 1.2f
		};
		moveDir = new Vector2[]{
			new Vector2(Constants.screenDimensions.x,Constants.screenDimensions.y).normalized * moveSpeed,
			new Vector2(-Constants.screenDimensions.x,Constants.screenDimensions.y).normalized * moveSpeed,
		};
		eagleSprite = GetComponent<SpriteRenderer> ();
		eagleSprite.enabled = false;
		StartCoroutine (InitiateAttack (1f));
	}

	public IEnumerator InitiateAttack(float waitTime){
		yield return new WaitForSeconds(waitTime);
		eagleSprite.enabled = true;
		StartCoroutine (UpRight ());
	}

	public IEnumerator UpRight(){
		transform.position = startPos [0];
		rigbod.velocity = moveDir [0];
		transform.Face4ward(true);
		pixelRotationScript.Angle = ConvertAnglesAndVectors.ConvertVector2IntAngle (moveDir [0],moveDir[0].x);
		yield return new WaitForSeconds(4f);
		StartCoroutine (UpLeft ());
	}

	public IEnumerator UpLeft(){
		transform.position = startPos [1];
		rigbod.velocity = moveDir [1];
		transform.Face4ward(false);
		pixelRotationScript.Angle = ConvertAnglesAndVectors.ConvertVector2IntAngle (moveDir [1],moveDir[1].x);
		yield return new WaitForSeconds(6f);
		rigbod.velocity = Vector2.zero;
		StartCoroutine (Strike ());
	}

	public IEnumerator Strike(){
		while (Mathf.Abs(xSpace)>Constants.worldDimensions.x){
			xSpace = jaiTransform.position.x + Random.Range (-Constants.worldDimensions.x, Constants.worldDimensions.x) * .15f;
		}

		transform.position = new Vector3 (xSpace, Constants.worldDimensions.y * 1.2f, 0f);
		attackDir = (jaiTransform.position - transform.position + Constants.balloonOffset).normalized;
		rigbod.velocity = attackDir * attackSpeed;
		transform.Face4ward(attackDir.x>0);
		pixelRotationScript.Angle = ConvertAnglesAndVectors.ConvertVector2IntAngle (attackDir,attackDir.x);

		StartCoroutine (InitiateAttack(5f));
		yield return null;
	}

	void OnDestroy(){
		StopAllCoroutines ();
	}


}
