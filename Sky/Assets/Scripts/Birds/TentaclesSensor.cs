using UnityEngine;
using System.Collections;
using GenericFunctions;

public class TentaclesSensor : MonoBehaviour {

	public GameObject tentacles;
	public Tentacles tentaclesScript;
	public BoxCollider2D tentacleSensorBoxCollider;
	
	void Awake () {
		tentacles = Instantiate (Resources.Load (Constants.tentaclePrefab), new Vector3 (0f,-0.5f - Constants.worldDimensions.y,0f), Quaternion.identity) as GameObject;
		tentaclesScript = tentacles.GetComponent<Tentacles> ();
		tentaclesScript.tentaclesSensorScript = this;
		tentacleSensorBoxCollider = GetComponent<BoxCollider2D> ();
		tentacleSensorBoxCollider.size = new Vector2 (Constants.worldDimensions.x * 2f, Constants.worldDimensions.y * .9375f);
		tentacleSensorBoxCollider.offset = new Vector2 (0f, -Constants.worldDimensions.y);
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
			if (exit.gameObject.layer==Constants.basketLayer){ //return to the depths
				StartCoroutine (tentaclesScript.ResetPosition());
			}
		}
	}

	public IEnumerator StopThem(){
		StopAllCoroutines ();
		yield return null;
	}
}
