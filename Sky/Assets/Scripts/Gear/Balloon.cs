using UnityEngine;
using System.Collections;

public class Balloon : MonoBehaviour {


	public BalloonBasket balloonBasketScript;
	public bool popping;
	public int balloonNumber;

	// Use this for initialization
	void Awake () {
		balloonBasketScript = GameObject.Find ("BalloonBasket").GetComponent<BalloonBasket> ();
		popping = false;
		if (name == "PinkBalloon"){
			balloonNumber = 0;
		}
		else if (name == "TealBalloon"){
			balloonNumber = 1;
		}
		else if (name == "GreyBalloon"){
			balloonNumber = 2;
		}
	}

	void OnTriggerEnter2D(Collider2D col){
		if (!popping && col.gameObject.layer == 16){//bird layer
			popping = true;
			foreach (Balloon balloonScript in balloonBasketScript.balloonScripts){
				if (balloonScript){
					balloonScript.popping = true;
				}
			}
			StartCoroutine (balloonBasketScript.BeginPopping(balloonNumber));
		}
	}
}
