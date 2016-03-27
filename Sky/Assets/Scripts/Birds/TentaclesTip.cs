using UnityEngine;
using System.Collections;

public class TentaclesTip : MonoBehaviour, IToggleable {

	[SerializeField] private Tentacles t; private ITipToTentacle tentacles;
	[SerializeField] private Collider2D myCol;

	void IToggleable.ToggleSensor(bool active){
		myCol.enabled = active;
	}

	void Awake(){
		tentacles = (ITipToTentacle)t;
	}

	void OnTriggerEnter2D(){
		StartCoroutine (tentacles.PullDownTheKill());
	}
}
