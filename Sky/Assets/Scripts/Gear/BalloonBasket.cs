using UnityEngine;
using System.Collections;

public class BalloonBasket : MonoBehaviour {

	public float floatForce;
	public float horizonBand;
	public float distAway;
	public float maxBalloonSpeed;
	public Rigidbody2D rigidbod;
	public BoxCollider2D basketCollider;
	public int balloonCount;

	// Use this for initialization
	void Start () {
		balloonCount = 3;
		maxBalloonSpeed = 2f;
		rigidbod = GetComponent<Rigidbody2D> ();
		basketCollider = GameObject.Find ("Basket").GetComponent<BoxCollider2D> ();
		Physics2D.IgnoreLayerCollision (14, 13); //ignore basket and spear collision
		Physics2D.IgnoreLayerCollision (16, 16); //ignore birds hitting birds
	}

	public IEnumerator Invincibility(){
		balloonCount--;
		if (balloonCount<1){
			rigidbod.gravityScale = 0;
			basketCollider.enabled = false;
			StartCoroutine (EndGame());
		}
		Physics2D.IgnoreLayerCollision (16, 17, true);//ignore birds and balloons for a second;
		yield return new WaitForSeconds(1f);
		Physics2D.IgnoreLayerCollision (16, 17, false); //resume
	}

	public IEnumerator SlowTime(float slowDuration, float timeScale){
		StartCoroutine (WaitForRealSeconds (slowDuration));
		Time.timeScale = timeScale;
		yield return null;
	}

	public IEnumerator WaitForRealSeconds(float slowDuration){
		float startTime = Time.realtimeSinceStartup;
		while (Time.realtimeSinceStartup - startTime < slowDuration){
			yield return null;
		}
		Time.timeScale = 1f;
		yield return null;
	}

	public IEnumerator EndGame(){
		yield return new WaitForSeconds (1.5f);
		UnityEditor.EditorApplication.isPlaying = false;
	}
	
}
