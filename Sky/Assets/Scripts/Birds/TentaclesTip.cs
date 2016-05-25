using UnityEngine;

public class TentaclesTip : MonoBehaviour, IToggleable {

	[SerializeField] Tentacles t; ITipToTentacle tentacles;
	[SerializeField] Collider2D myTipCol;

	void IToggleable.ToggleSensor(bool active){
		myTipCol.enabled = active;
	}

	void Awake(){
		tentacles = (ITipToTentacle)t;
	}

	void OnTriggerEnter2D(){
		StartCoroutine (tentacles.PullDownTheKill());
	}
}
