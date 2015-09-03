using UnityEngine;
using System.Collections;

public class Balloon : MonoBehaviour {

	public Animator balloonAnimator;
	public AudioSource popNoise;
	public BalloonBasket balloonBasketScript;
	public Rigidbody2D balloonBasketBody;
	public float dropForce;
	public bool popping;
	public CircleCollider2D balloonCollider;
	public ScreenShake screenShakeScript;

	// Use this for initialization
	void Awake () {
		balloonAnimator = GetComponent<Animator> ();
		popNoise = GetComponent<AudioSource> ();
		balloonBasketScript = GameObject.Find ("BalloonBasket").GetComponent<BalloonBasket> ();
		screenShakeScript = GameObject.Find ("mainCam").GetComponent<ScreenShake> ();
		balloonBasketBody = GameObject.Find ("BalloonBasket").GetComponent<Rigidbody2D> ();
		dropForce = 5f;
		popping = false;
		balloonCollider = GetComponent<CircleCollider2D> ();
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.layer == 16){//bird layer
			StartCoroutine (SlowTime());
			StartCoroutine(PopBalloon());
			StartCoroutine(balloonBasketScript.Invincibility());
			StartCoroutine(screenShakeScript.CameraShake());
		}
	}

	public IEnumerator SlowTime(){
		Time.timeScale = 0.75f;
		yield return new WaitForSeconds (.5f);
		Time.timeScale = 1f;
		yield return null;
	}

	public IEnumerator PopBalloon(){
		if (!popping){
			popping = true;
			popNoise.Play ();
			balloonAnimator.SetInteger("AnimState",1);
			balloonBasketBody.AddForce (Vector2.down * dropForce);
			while (popNoise.isPlaying){
				yield return null;
			}
			Destroy (gameObject);
		}
		yield return null;
	}
}
