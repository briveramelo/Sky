using UnityEngine;
using System.Collections;

public class Albatross : MonoBehaviour {

	public Rigidbody2D rigbod;
	public Transform jaiTransform;

	public Vector3 offSet;
	public Vector3 pixelScale;
	public Vector3 pixelScaleReversed;

	public Vector2 moveDir;

	public float moveSpeed;


	// Use this for initialization
	void Awake () {
		jaiTransform = GameObject.Find ("Jai").transform;
		rigbod = GetComponent<Rigidbody2D> ();
		moveSpeed = .79f;
		offSet = new Vector3 (0.1f, 1.3f, 0f);
		pixelScale = Vector3.one * 3.125f;
		pixelScaleReversed = new Vector3 (-3.125f,3.125f,1f);
	}
	
	// Update is called once per frame
	void Update () {
		rigbod.velocity = (jaiTransform.position - transform.position + offSet).normalized * moveSpeed;
		if (rigbod.velocity.x>0){
			transform.localScale = pixelScaleReversed;
		}
		else{
			transform.localScale = pixelScale;
		}
	}
}
