using UnityEngine;
using System.Collections;

public class Albatross : MonoBehaviour {

	public Vector2 moveDir;
	public Rigidbody2D rigbod;
	public Transform balloonBasketTransform;
	public float moveSpeed;
	public Vector3 offSet;
	public Vector3 pixelScale;
	public Vector3 pixelScaleReversed;

	// Use this for initialization
	void Awake () {
		balloonBasketTransform = GameObject.Find ("BalloonBasket").transform;
		rigbod = GetComponent<Rigidbody2D> ();
		moveSpeed = .79f;
		offSet = new Vector3 (0.1f, 0.8f, 0f);
		pixelScale = Vector3.one * 3.125f;
		pixelScaleReversed = new Vector3 (-3.125f,3.125f,1f);
	}
	
	// Update is called once per frame
	void Update () {
		rigbod.velocity = (balloonBasketTransform.position - transform.position + offSet).normalized * moveSpeed;
		if (rigbod.velocity.x>0){
			transform.localScale = pixelScaleReversed;
		}
		else{
			transform.localScale = pixelScale;
		}
	}
}
