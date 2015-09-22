using UnityEngine;
using System.Collections;
using GenericFunctions;

public class BirdOfParadise : MonoBehaviour {

	public Rigidbody2D rigidbod;
	
	public Vector2[] duckMoveDirections;
	
	public float moveSpeed;
	
	// Use this for initialization
	void Awake () {
		moveSpeed = 3f;
		rigidbod = GetComponent<Rigidbody2D> ();
		
		rigidbod.velocity = Vector2.right * moveSpeed;
		transform.Face4ward(false);
	}
}
