using UnityEngine;
using System.Collections;
using GenericFunctions;

public interface ITentacleToSensor {
	void ToggleSensor(bool active);
	bool JaiInRange{get;}
}

public class TentaclesSensor : MonoBehaviour, ITentacleToSensor {

	[SerializeField] private Tentacles tentaclesScript; private ISensorToTentacle sensorToTentacle;
	[SerializeField] private Collider2D sensor;
	private bool jaiInRange; public bool JaiInRange{get{return jaiInRange;}}

	void Awake () {
		sensorToTentacle = (ISensorToTentacle)tentaclesScript;
		transform.position = Vector3.zero;
	}
		
	void OnTriggerEnter2D(Collider2D enterer){
		if (enterer.gameObject.layer==Constants.basketLayer){ //rise against the basket
			if (!enterer.isTrigger){
				jaiInRange = true;
				StartCoroutine (sensorToTentacle.GoForTheKill());
			}
		}
	}

	void OnTriggerExit2D(Collider2D exiter){
		if (exiter.gameObject.layer==Constants.basketLayer){ //return to the depths
			if (!exiter.isTrigger){
				jaiInRange = false;
				StartCoroutine (sensorToTentacle.ResetPosition(false));
			}
		}
	}

	void ITentacleToSensor.ToggleSensor(bool active){
		sensor.enabled = active;
	}
}
