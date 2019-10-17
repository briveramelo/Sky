using UnityEngine;

public class TentaclesTip : MonoBehaviour, IToggleable {

	[SerializeField] private Tentacles t; 
	[SerializeField] private Collider2D myTipCol;
	private ITipToTentacle tentacles;

	void IToggleable.ToggleSensor(bool active){
		myTipCol.enabled = active;
	}

	private void Awake(){
		tentacles = t;
	}

	private void OnTriggerEnter2D(){
		StartCoroutine (tentacles.PullDownTheKill());
	}
}
