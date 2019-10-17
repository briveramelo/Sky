using UnityEngine;

public class TentaclesTip : MonoBehaviour, IToggleable {

	[SerializeField] Tentacles t; 
	[SerializeField] Collider2D myTipCol;
	ITipToTentacle tentacles;

	void IToggleable.ToggleSensor(bool active){
		myTipCol.enabled = active;
	}

	void Awake(){
		tentacles = t;
	}

	void OnTriggerEnter2D(){
		StartCoroutine (tentacles.PullDownTheKill());
	}
}
