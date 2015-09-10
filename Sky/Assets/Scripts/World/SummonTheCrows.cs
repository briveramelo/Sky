using UnityEngine;
using System.Collections;

public class SummonTheCrows : MonoBehaviour {

	public string crowString;

	// Use this for initialization
	void Awake () {
		crowString = "Prefabs/Birds/Murder";
	}
	
	public IEnumerator Murder(){
		GameObject crows = Instantiate (Resources.Load (crowString), Vector3.zero, Quaternion.identity) as GameObject;
		yield return null;
	}
}
