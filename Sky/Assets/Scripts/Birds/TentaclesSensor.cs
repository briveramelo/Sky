using UnityEngine;

public interface IToggleable {
	void ToggleSensor(bool active);
}
public interface IJaiDetected{
	bool JaiInRange{get;}
}
public class TentaclesSensor : MonoBehaviour, IToggleable, IJaiDetected {

	[SerializeField] Tentacles tentaclesScript; ISensorToTentacle tentacle;
	[SerializeField] Collider2D sensor;
	private bool jaiInRange; bool IJaiDetected.JaiInRange{get{return jaiInRange;}}

	void Awake () {
		tentacle = (ISensorToTentacle)tentaclesScript;
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
