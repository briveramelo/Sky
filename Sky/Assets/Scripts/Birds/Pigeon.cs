using UnityEngine;
using System.Collections;

public class Pigeon : MonoBehaviour {

	public float moveSpeed;
	public Vector3 pixelScale;
	public Vector3 pixelScaleReversed;
	public Rigidbody2D rigidbod;
	
	public Vector2[] duckMoveDirections;
	
	
	// Use this for initialization
	void Awake () {
		pixelScale = Vector3.one * 3.125f;
		pixelScaleReversed = new Vector3 (-3.125f,3.125f,1f);
		moveSpeed = 2f;
		rigidbod = GetComponent<Rigidbody2D> ();
		
		rigidbod.velocity = Vector2.right * moveSpeed;
		transform.localScale = pixelScaleReversed;
	}
}
