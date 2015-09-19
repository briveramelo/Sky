using UnityEngine;
using System.Collections;
using GenericFunctions;

public class SummonTheCrows : MonoBehaviour {

	public IEnumerator Murder(){
		GameObject crows = Instantiate (Resources.Load (Constants.murderPrefab), Vector3.zero, Quaternion.identity) as GameObject;
		yield return null;
	}
}
