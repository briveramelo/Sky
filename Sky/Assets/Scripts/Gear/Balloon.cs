using UnityEngine;
using System.Collections;
using GenericFunctions;

public class Balloon : MonoBehaviour {


	public Basket basketScript;

	public CircleCollider2D balloonCollider;
	public Animator balloonAnimator;

	public AudioSource popNoise;

	public int balloonNumber;
	public float moveSpeed;
	public float popTime;

	public bool popping;
	public bool moving;

	// Use this for initialization
	void Awake () {
		basketScript = GameObject.Find ("BalloonBasket").GetComponent<Basket> ();
		balloonCollider = GetComponent<CircleCollider2D>();
		balloonAnimator = GetComponent<Animator> ();
		popNoise = GetComponent<AudioSource> ();
		moveSpeed = 0.75f;
		popTime = 30f;
		popping = false;
		if (name.Contains("PinkBalloon")){
			balloonNumber = 0;
		}
		else if (name.Contains("TealBalloon")){
			balloonNumber = 1;
		}
		else if (name.Contains("GreyBalloon")){
			balloonNumber = 2;
		}
		if (!transform.parent){
			gameObject.layer = Constants.balloonFloatingLayer;
			moving = true;
			transform.Face4ward(true);
			StartCoroutine (MoveUp ());
		}
	}

	public IEnumerator MoveUp(){
		float startTime = Time.realtimeSinceStartup;
		while (moving){
			transform.position += Vector3.up * moveSpeed * Time.deltaTime;
			if (Time.realtimeSinceStartup-startTime>popTime){
				Destroy (gameObject);
			}
			yield return null;
		}
		yield return null;
	}

	public IEnumerator LocalPop(){
		balloonAnimator.SetInteger("AnimState",1);
		Destroy (transform.GetChild(0).gameObject);
		popNoise.Play ();
		Destroy (gameObject,Constants.time2Destroy);
		yield return null;
	}

	void OnTriggerEnter2D(Collider2D col){
		if (!popping && (col.gameObject.layer == Constants.birdLayer)){//bird layer pops free balloon
			popping = true;
			if (!transform.parent){
				StartCoroutine(LocalPop());
			}
			else {
				StartCoroutine (basketScript.BeginPopping(balloonNumber));
			}
		}
	}

	void OnDestroy(){
		StopAllCoroutines ();
	}
}
