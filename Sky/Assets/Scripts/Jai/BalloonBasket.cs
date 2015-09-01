using UnityEngine;
using System.Collections;

public class BalloonBasket : MonoBehaviour {

	public float floatForce;
	public float horizonBand;
	public float distAway;
	public float maxBalloonSpeed;
	public Rigidbody2D rigidbod;

	// Use this for initialization
	void Start () {
		maxBalloonSpeed = 2f;
		floatForce = 1f;
		horizonBand = 0f;
		rigidbod = GetComponent<Rigidbody2D> ();
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
