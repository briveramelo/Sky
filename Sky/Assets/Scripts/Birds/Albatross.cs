using UnityEngine;
using System.Collections;
using GenericFunctions;

public class Albatross : MonoBehaviour {

	public Rigidbody2D rigbod;
	public Transform jaiTransform;
	public float moveSpeed;

	// Use this for initialization
	void Awake () {
		jaiTransform = GameObject.Find ("Jai").transform;
		rigbod = GetComponent<Rigidbody2D> ();
		moveSpeed = .69f;
	}
	
	// Update is called once per frame
	void Update () {
		rigbod.velocity = (jaiTransform.position - transform.position + Constants.balloonOffset).normalized * moveSpeed;
		transform.Face4ward(rigbod.velocity.x<0);
	}
}
