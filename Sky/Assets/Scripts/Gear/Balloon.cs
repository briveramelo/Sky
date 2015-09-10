using UnityEngine;
using System.Collections;

public class Balloon : MonoBehaviour {


	public Basket basketScript;

	public int balloonNumber;

	public bool popping;

	// Use this for initialization
	void Awake () {
		basketScript = GameObject.Find ("BalloonBasket").GetComponent<Basket> ();
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
			foreach (Balloon balloonScript in basketScript.balloonScripts){
				if (balloonScript){
					balloonScript.popping = true;
				}
			}
			StartCoroutine (basketScript.BeginPopping(balloonNumber));
		}
	}
}
