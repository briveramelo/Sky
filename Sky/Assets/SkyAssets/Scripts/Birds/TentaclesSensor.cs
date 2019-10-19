using UnityEngine;

public interface IToggleable {
	void ToggleSensor(bool active);
}
public interface IJaiDetected{
	bool JaiInRange{get;}
}
public class TentaclesSensor : MonoBehaviour, IToggleable, IJaiDetected {

	[SerializeField] private Tentacles _tentaclesScript; 
	[SerializeField] private Collider2D _sensor;
	
	private ISensorToTentacle _tentacle;
	private bool _jaiInRange; 
	
	bool IJaiDetected.JaiInRange => _jaiInRange;

	private void Awake () {
		_tentacle = _tentaclesScript;
	}

	private void OnTriggerEnter2D(){
		_jaiInRange = true;
		StartCoroutine (_tentacle.GoForTheKill());
	}

	private void OnTriggerExit2D(){
		_jaiInRange = false;
		StartCoroutine (_tentacle.ResetPosition(false));
	}

	void IToggleable.ToggleSensor(bool active){
		_sensor.enabled = active;
	}
}
