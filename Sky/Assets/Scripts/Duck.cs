using UnityEngine;
using System.Collections;

public class Duck : MonoBehaviour {

	public float moveSpeed;
	public float currentSpeed;

	// Use this for initialization
	void Awake () {
		moveSpeed = 2f;
		currentSpeed = moveSpeed;
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.x > 6) {
			currentSpeed = -moveSpeed;
			transform.localScale = new Vector3 (4,4,1);
		}
		else if (transform.position.x < -6){
			currentSpeed = moveSpeed;
			transform.localScale = new Vector3 (-4,4,1);
		}
		rigidbody2D.velocity = Vector2.right * currentSpeed;
	}
}
