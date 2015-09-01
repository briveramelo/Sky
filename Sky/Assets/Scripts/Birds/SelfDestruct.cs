using UnityEngine;
using System.Collections;

public class SelfDestruct : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		StartCoroutine (DestroySelf ());
	}
	
	public IEnumerator DestroySelf(){
		yield return new WaitForSeconds (2f);
		Destroy (gameObject);
	}
}
