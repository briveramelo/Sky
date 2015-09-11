using UnityEngine;
using System.Collections;
using GenericFunctions;

public class Albatross : MonoBehaviour {

	public Rigidbody2D rigbod;
	public Transform jaiTransform;


	public Vector2 moveDir;

	public float moveSpeed;


	// Use this for initialization
	void Awake () {
		jaiTransform = GameObject.Find ("Jai").transform;
		rigbod = GetComponent<Rigidbody2D> ();
		moveSpeed = .79f;
	}
	
	// Update is called once per frame
	void Update () {
		rigbod.velocity = (jaiTransform.position - transform.position + Constants.balloonOffset).normalized * moveSpeed;
		if (rigbod.velocity.x>0){
			transform.localScale = Constants.Pixel625(false);
		}
		else{
			transform.localScale = Constants.Pixel625(true);
		}
	}
}
