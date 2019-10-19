using UnityEngine;

public class TentaclesTip : MonoBehaviour, IToggleable {

	[SerializeField] private Tentacles _t; 
	[SerializeField] private Collider2D _myTipCol;
	private ITipToTentacle _tentacles;

	void IToggleable.ToggleSensor(bool active){
		_myTipCol.enabled = active;
	}

	private void Awake(){
		_tentacles = _t;
	}

	private void OnTriggerEnter2D(){
		StartCoroutine (_tentacles.PullDownTheKill());
	}
}
