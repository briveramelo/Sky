using UnityEngine;
using System.Collections;

public class Duck : MonoBehaviour {

	public float moveSpeed;
	public float currentSpeed;
	public Vector3 pixelScale;
	public Vector3 pixelScaleReversed;
	public Rigidbody2D rigidbod;

	// Use this for initialization
	void Awake () {
		pixelScale = Vector3.one * 3.125f;
		pixelScaleReversed = new Vector3 (-3.125f,3.125f,1f);
		moveSpeed = 2f;
		currentSpeed = moveSpeed;
		rigidbod = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.x > 6) {
			currentSpeed = -moveSpeed;
			transform.localScale = pixelScale;
		}
		else if (transform.position.x < -6){
			currentSpeed = moveSpeed;
			transform.localScale = pixelScaleReversed;
		}
		rigidbod.velocity = Vector2.right * currentSpeed;
	}
}
