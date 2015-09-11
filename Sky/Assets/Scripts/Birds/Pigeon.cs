using UnityEngine;
using System.Collections;
using GenericFunctions;

public class Pigeon : MonoBehaviour {

	public Rigidbody2D rigidbod;

	public Vector2[] duckMoveDirections;

	public float moveSpeed;

	// Use this for initialization
	void Awake () {
		moveSpeed = 2f;
		rigidbod = GetComponent<Rigidbody2D> ();
		
		rigidbod.velocity = Vector2.right * moveSpeed;
		transform.localScale = Constants.Pixel625(false);
	}
}
