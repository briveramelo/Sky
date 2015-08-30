using UnityEngine;
using System.Collections;

public class BalloonBasket : MonoBehaviour {

	public float floatForce;
	public float horizonBand;
	public float distAway;
	public float maxBalloonSpeed;

	// Use this for initialization
	void Start () {
		maxBalloonSpeed = 2f;
		floatForce = 1f;
		horizonBand = 0f;
	}

	// Update is called once per frame
	void Update () {
		distAway = -.03f*Mathf.Pow (transform.position.y,3f);
		if (Mathf.Abs (rigidbody2D.velocity.y)<maxBalloonSpeed) {
			if (Mathf.Abs (transform.position.y) > horizonBand) {
				rigidbody2D.AddForce (Vector2.up * floatForce * distAway);
			}
		}
	}
}
