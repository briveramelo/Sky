using UnityEngine;
using System.Collections;

public class Jai : MonoBehaviour {

	public bool throwing;
	public bool stabbing;

	public int throws;
	public int stabs;

	void Start(){
		throwing = false;
	}

	public IEnumerator ThrowSpear(Vector2 throwDir){
		throws++;
		yield return null;
	}

	public IEnumerator StabSpear(Vector2 throwDir){
		stabs++;
		yield return null;
	}

}
