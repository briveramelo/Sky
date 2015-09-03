using UnityEngine;
using System.Collections;

public class BalloonBasket : MonoBehaviour {

	public float floatForce;
	public float horizonBand;
	public float distAway;
	public float maxBalloonSpeed;
	public Rigidbody2D rigidbod;
	public int balloonCount;

	// Use this for initialization
	void Start () {
		balloonCount = 3;
		maxBalloonSpeed = 2f;
		floatForce = 1f;
		horizonBand = 0f;
		rigidbod = GetComponent<Rigidbody2D> ();
	}

	public IEnumerator Invincibility(){
		balloonCount--;
		if (balloonCount<1){
			StartCoroutine (EndGame());
		}
		Physics2D.IgnoreLayerCollision (16, 17, true);//ignore birds and balloons for a second;
		yield return new WaitForSeconds(1f);
		Physics2D.IgnoreLayerCollision (16, 17, false); //resume
	}

	public IEnumerator EndGame(){
		yield return new WaitForSeconds (1.5f);
		UnityEditor.EditorApplication.isPlaying = false;
	}

	// Update is called once per frame
	void Update () {
		distAway = -.03f*Mathf.Pow (transform.position.y,3f);
		if (Mathf.Abs (rigidbod.velocity.y)<maxBalloonSpeed) {
			if (Mathf.Abs (transform.position.y) > horizonBand) {
				rigidbod.AddForce (Vector2.up * floatForce * distAway);
			}
		}
	}
}
