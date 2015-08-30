using UnityEngine;
using System.Collections;

public class Jai : MonoBehaviour {

	public bool throwing;
	public bool stabbing;

	public int throws;
	public int stabs;

	public float throwForce;

	public string ballString;

	public GameObject ball;

	void Start(){
		throwing = false;
		throwForce = 1000f;
		ballString = "Prefabs/Equipment/Ball";
	}

	public IEnumerator ThrowSpear(Vector2 throwDir){
		ball = Instantiate (Resources.Load (ballString), transform.position, Quaternion.identity) as GameObject;
		ball.rigidbody2D.AddForce (throwDir * throwForce);
		throws++;
		yield return null;
	}

	public IEnumerator StabSpear(Vector2 throwDir){
		stabs++;
		yield return null;
	}

}
