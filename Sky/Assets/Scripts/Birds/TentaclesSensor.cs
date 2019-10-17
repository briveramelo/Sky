using UnityEngine;

public interface IToggleable {
	void ToggleSensor(bool active);
}
public interface IJaiDetected{
	bool JaiInRange{get;}
}
public class TentaclesSensor : MonoBehaviour, IToggleable, IJaiDetected {

	[SerializeField] Tentacles tentaclesScript; 
	[SerializeField] Collider2D sensor;
	ISensorToTentacle tentacle;
	
	private bool jaiInRange; 
	bool IJaiDetected.JaiInRange => jaiInRange;

	void Awake () {
		tentacle = tentaclesScript;
	}
		
	void OnTriggerEnter2D(){
		jaiInRange = true;
		StartCoroutine (tentacle.GoForTheKill());
	}

	void OnTriggerExit2D(){
		jaiInRange = false;
		StartCoroutine (tentacle.ResetPosition(false));
	}

	void IToggleable.ToggleSensor(bool active){
		sensor.enabled = active;
	}
}
