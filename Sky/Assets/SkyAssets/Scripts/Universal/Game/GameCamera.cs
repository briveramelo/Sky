using UnityEngine;
using System.Collections;

public class GameCamera : MonoBehaviour {

	public static GameCamera Instance;
	private bool shaking;
	private Vector3 startSpot;

	private void Awake(){
		if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }
        startSpot = transform.position;
	}

	public void ShakeTheCamera(){
		StartCoroutine (TriggerShake());
	}

	private IEnumerator TriggerShake(){
		StartCoroutine (ShakeIt ());
		yield return new WaitForSeconds (.1f);
		shaking = false;
	}

	private IEnumerator ShakeIt(){
		shaking = true;
		while (shaking) {
			Vector3 shift = new Vector3( Random.insideUnitCircle.x * .2f,Random.insideUnitCircle.y * .2f,0f);
			transform.position = startSpot + shift;
			yield return null;
		}
		transform.position = startSpot;
	}
}
