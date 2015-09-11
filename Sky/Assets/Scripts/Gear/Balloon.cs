using UnityEngine;
using System.Collections;
using GenericFunctions;

public class Balloon : MonoBehaviour {


	public Basket basketScript;

	public CircleCollider2D balloonCollider;

	public int balloonNumber;
	public float moveSpeed;

	public bool popping;
	public bool moving;

	// Use this for initialization
	void Awake () {
		basketScript = GameObject.Find ("BalloonBasket").GetComponent<Basket> ();
		balloonCollider = GetComponent<CircleCollider2D>();
		moveSpeed = 0.75f;
		popping = false;
		moving = true;
		if (name.Contains("PinkBalloon")){
			balloonNumber = 0;
		}
		else if (name.Contains("TealBalloon")){
			balloonNumber = 1;
		}
		else if (name.Contains("GreyBalloon")){
			balloonNumber = 2;
		}
		if (!transform.parent){
			gameObject.layer = 18;
			transform.localScale = Constants.Pixel625(true);
			StartCoroutine (MoveUp ());
		}
	}

	public IEnumerator MoveUp(){
		while (moving){
			transform.position += Vector3.up * moveSpeed * Time.deltaTime;
			yield return null;
		}
		yield return null;
	}

	void OnTriggerEnter2D(Collider2D col){
		if (!popping && col.gameObject.layer == 16){//bird layer
			popping = true;
			foreach (Balloon balloonScript in basketScript.balloonScripts){
				if (balloonScript){
					balloonScript.popping = true;
				}
			}
			StartCoroutine (basketScript.BeginPopping(balloonNumber));
		}
	}
}
