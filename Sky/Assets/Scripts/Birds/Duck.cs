using UnityEngine;
using System.Collections;

public class Duck : MonoBehaviour {

	public float moveSpeed;
	public float currentSpeed;
	public Rigidbody2D rigidbod;

	// Use this for initialization
	void Awake () {
		moveSpeed = 2f;
		currentSpeed = moveSpeed;
		rigidbod = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.x > 6) {
			currentSpeed = -moveSpeed;
			transform.localScale = new Vector3 (1,1,1);
		}
		else if (transform.position.x < -6){
			currentSpeed = moveSpeed;
			transform.localScale = new Vector3 (-1,1,1);
		}
		rigidbod.velocity = Vector2.right * currentSpeed;
	}
}
