using UnityEngine;
using System.Collections;

public class BirdOfParadise : MonoBehaviour {

	public Rigidbody2D rigidbod;
	
	public Vector3 pixelScale;
	public Vector3 pixelScaleReversed;
	
	public Vector2[] duckMoveDirections;
	
	public float moveSpeed;
	
	// Use this for initialization
	void Awake () {
		pixelScale = Vector3.one * 3.125f;
		pixelScaleReversed = new Vector3 (-3.125f,3.125f,1f);
		moveSpeed = 4f;
		rigidbod = GetComponent<Rigidbody2D> ();
		
		rigidbod.velocity = Vector2.right * moveSpeed;
		transform.localScale = pixelScaleReversed;
	}
}
