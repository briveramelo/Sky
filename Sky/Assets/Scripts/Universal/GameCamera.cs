using UnityEngine;
using System.Collections;

public class GameCamera : MonoBehaviour {

	public static GameCamera Instance;
	private bool shaking;

	void Awake(){
		Instance = this;
	}

	public void ShakeTheCamera(){
		StartCoroutine (TriggerShake());
	}

	IEnumerator TriggerShake(){
		StartCoroutine (ShakeIt ());
		yield return new WaitForSeconds (.1f);
		shaking = false;
	}
	
	IEnumerator ShakeIt(){
		shaking = true;
		Vector3 startSpot = transform.position;
		while (shaking) {
			Vector3 shift = new Vector3( Random.insideUnitCircle.x,Random.insideUnitCircle.y,-10) * .2f;
			transform.position = startSpot + shift;
			yield return null;
		}
		transform.position = new Vector3 (0f, 0f, -10f);
		yield return null;
	}
}
