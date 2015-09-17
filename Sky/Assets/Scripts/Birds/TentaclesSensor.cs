using UnityEngine;
using System.Collections;

public class TentaclesSensor : MonoBehaviour {

	public Tentacles tentaclesScript;

	// Use this for initialization
	void Awake () {
		tentaclesScript = transform.parent.GetComponent<Tentacles> ();
	}
	
	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.layer==13){ //rise against the basket
			StartCoroutine (tentaclesScript.Surface(col.gameObject.layer,-1));
		}
		else if (col.gameObject.layer==16){
			StartCoroutine (tentaclesScript.Surface(col.gameObject.layer,col.gameObject.GetComponent<GetHurt>().birdType));
		}
	}
}
