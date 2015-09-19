using UnityEngine;
using System.Collections;
using GenericFunctions;

public class TentaclesSensor : MonoBehaviour {

	public GameObject tentacles;
	public Tentacles tentaclesScript;
	
	void Awake () {
		tentacles = Instantiate (Resources.Load (Constants.tentaclePrefab), Constants.tentacleHomeSpot, Quaternion.identity) as GameObject;
		tentaclesScript = tentacles.GetComponent<Tentacles> ();
		tentaclesScript.tentaclesSensorScript = this;
	}
	
	void OnTriggerStay2D(Collider2D enter){
		if (!tentaclesScript.holding && !tentaclesScript.attacking && !tentaclesScript.hurt){
			if (enter.gameObject.layer==Constants.basketLayer){ //rise against the basket
				StartCoroutine (tentaclesScript.GoForTheKill());
			}
		}
	}

	void OnTriggerExit2D(Collider2D exit){
		if (tentaclesScript.attacking){
			if (exit.gameObject.layer==Constants.basketLayer){ //rise against the basket
				StartCoroutine (tentaclesScript.ResetPosition());
			}
		}
	}

	public IEnumerator StopThem(){
		StopAllCoroutines ();
		yield return null;
	}
}
