using UnityEngine;

public interface IToggleable {
	void ToggleSensor(bool active);
}
public interface IJaiDetected{
	bool JaiInRange{get;}
}
public class TentaclesSensor : MonoBehaviour, IToggleable, IJaiDetected {

	[SerializeField] private Tentacles tentaclesScript; 
	[SerializeField] private Collider2D sensor;
	private ISensorToTentacle tentacle;
	
	private bool jaiInRange; 
	bool IJaiDetected.JaiInRange => jaiInRange;

	private void Awake () {
		tentacle = tentaclesScript;
	}

	private void OnTriggerEnter2D(){
		jaiInRange = true;
		StartCoroutine (tentacle.GoForTheKill());
	}

	private void OnTriggerExit2D(){
		jaiInRange = false;
		StartCoroutine (tentacle.ResetPosition(false));
	}

	void IToggleable.ToggleSensor(bool active){
		sensor.enabled = active;
	}
}
