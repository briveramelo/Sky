using UnityEngine;
using System.Collections;

public class Duck : MonoBehaviour {

	public Rigidbody2D rigidbod;

	public Vector3 pixelScale;
	public Vector3 pixelScaleReversed;

	public Vector2 moveDir;

	public float moveSpeed;

	public bool bouncing;


	// Use this for initialization
	void Awake () {
		pixelScale = Vector3.one * 3.125f;
		pixelScaleReversed = new Vector3 (-3.125f,3.125f,1f);
		moveSpeed = 2f;
		rigidbod = GetComponent<Rigidbody2D> ();

		moveDir = Vector2.one.normalized * moveSpeed;

		rigidbod.velocity = moveDir;
		transform.localScale = pixelScaleReversed;

	}

	void Update(){
		VelocityBouncer ();
	}

	void VelocityBouncer(){
		if (transform.position.y>5){
			rigidbod.velocity = new Vector2 (rigidbod.velocity.x, -moveDir.y);
		}
		else if (transform.position.y<-5f){
			rigidbod.velocity = new Vector2 (rigidbod.velocity.x, moveDir.y);
		}
		else if (transform.position.x>8.8f){
			rigidbod.velocity = new Vector2 (-moveDir.x, rigidbod.velocity.y);
			transform.localScale = pixelScale;
		}
		else if (transform.position.x<-8.8f){
			rigidbod.velocity = new Vector2 (moveDir.x, rigidbod.velocity.y);
			transform.localScale = pixelScaleReversed;
		}
	}

	/*void TransformReverser(){
		if (rigidbod.velocity.x>0){
		}
		else {
		}
	}*/
}
