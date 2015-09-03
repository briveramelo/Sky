using UnityEngine;
using System.Collections;

public class SelfDestruct : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		Destroy (gameObject, 2f);
	}
}
