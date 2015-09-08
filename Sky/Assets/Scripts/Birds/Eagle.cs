using UnityEngine;
using System.Collections;

public class Eagle : MonoBehaviour {

	public Vector2[] startPos;
	public Vector2[] moveDir;
	public float moveSpeed;
	public float attackSpeed;
	public Rigidbody2D rigbod;
	public Transform balloonBasketTransform;
	public int attacks;
	public float xWorldBounds;
	public float xSpace;
	public Vector3 offSet;
	public Vector3 attackDir;
	public Vector3 pixelScale;
	public Vector3 pixelScaleReversed;

	// Use this for initialization
	void Awake () {
		moveSpeed = 6.5f;
		attackSpeed = 11f;
		attacks = 3;
		xWorldBounds = 8.9f;
		xSpace = 9f;
		pixelScale = Vector3.one * 3.125f;
		pixelScaleReversed = new Vector3 (-3.125f,3.125f,1f);
		rigbod = GetComponent<Rigidbody2D> ();
		balloonBasketTransform = GameObject.Find ("BalloonBasket").transform;
		offSet = new Vector3 (0.1f, 0.8f, 0f);
		startPos = new Vector2[]{
			new Vector2(-8.9f,-5f),
			new Vector2(8.9f,-5f)
		};
		moveDir = new Vector2[]{
			new Vector2(1120,650).normalized * moveSpeed,
			new Vector2(-1120,650).normalized * moveSpeed,
		};
		StartCoroutine (InitiateAttack (1f));
	}

	public IEnumerator InitiateAttack(float waitTime){
		yield return new WaitForSeconds(waitTime);
		StartCoroutine (UpRight ());
	}

	public IEnumerator UpRight(){
		transform.position = startPos [0];
		rigbod.velocity = moveDir [0];
		transform.localScale = pixelScale;
		yield return new WaitForSeconds(4f);
		StartCoroutine (UpLeft ());
	}

	public IEnumerator UpLeft(){
		transform.position = startPos [1];
		rigbod.velocity = moveDir [1];
		transform.localScale = pixelScaleReversed;
		yield return new WaitForSeconds(6f);
		rigbod.velocity = Vector2.zero;
		StartCoroutine (Strike ());
	}
	
	public IEnumerator Strike(){
		while (Mathf.Abs(xSpace)>xWorldBounds){
			xSpace = balloonBasketTransform.position.x + Random.Range (-30, 31) * .1f;
		}

		transform.position = new Vector3 (xSpace, 6f, 0f);
		attackDir = (balloonBasketTransform.position - transform.position + offSet).normalized;
		rigbod.velocity = attackDir * attackSpeed;
		if (attackDir.x>0){
			transform.localScale = pixelScale;
		}
		else{
			transform.localScale = pixelScaleReversed;
		}
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
