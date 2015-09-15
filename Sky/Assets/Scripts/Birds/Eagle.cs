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
	
	public int attacks;

	public float moveSpeed;
	public float attackSpeed;
	public float xWorldBounds;
	public float xSpace;


	// Use this for initialization
	void Awake () {
		pixelRotationScript = GetComponent<PixelRotation> ();
		moveSpeed = 6.5f;
		attackSpeed = 11f;
		attacks = 3;
		xWorldBounds = 8.9f;
		xSpace = 9f;
		rigbod = GetComponent<Rigidbody2D> ();
		jaiTransform = GameObject.Find ("Jai").transform;
		startPos = new Vector2[]{
			new Vector2(-9.5f,-5.8f),
			new Vector2(9.5f,-5.8f)
		};
		moveDir = new Vector2[]{
			new Vector2(1120,650).normalized * moveSpeed,
			new Vector2(-1120,650).normalized * moveSpeed,
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
		transform.localScale = Constants.Pixel625(true);
		pixelRotationScript.Angle = ConvertAnglesAndVectors.ConvertVector2IntAngle (moveDir [0],moveDir[0].x);
		yield return new WaitForSeconds(4f);
		StartCoroutine (UpLeft ());
	}

	public IEnumerator UpLeft(){
		transform.position = startPos [1];
		rigbod.velocity = moveDir [1];
		transform.localScale = Constants.Pixel625(false);
		pixelRotationScript.Angle = ConvertAnglesAndVectors.ConvertVector2IntAngle (moveDir [1],moveDir[1].x);
		yield return new WaitForSeconds(6f);
		rigbod.velocity = Vector2.zero;
		StartCoroutine (Strike ());
	}

	public IEnumerator Strike(){
		while (Mathf.Abs(xSpace)>xWorldBounds){
			xSpace = jaiTransform.position.x + Random.Range (-30, 31) * .1f;
		}

		transform.position = new Vector3 (xSpace, 6f, 0f);
		attackDir = (jaiTransform.position - transform.position + Constants.balloonOffset).normalized;
		rigbod.velocity = attackDir * attackSpeed;
		if (attackDir.x>0){
			transform.localScale = Constants.Pixel625(true);

		}
		else{
			transform.localScale = Constants.Pixel625(false);
		}
		pixelRotationScript.Angle = ConvertAnglesAndVectors.ConvertVector2IntAngle (attackDir,attackDir.x);

		attacks--;
		if (attacks>0){
			StartCoroutine (InitiateAttack(5f));
		}
		else{
			StartCoroutine (SelfDestruct(6f));
		}
		yield return null;
	}

	void OnDestroy(){
		StopAllCoroutines ();
	}

	public IEnumerator SelfDestruct(float destroyTime){
		yield return new WaitForSeconds (destroyTime);
		Destroy (gameObject);
	}


}
