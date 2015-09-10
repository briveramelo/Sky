using UnityEngine;
using System.Collections;

public class ScreenShake : MonoBehaviour {

	public Vector3 startSpot;
	public Vector3 shift;

	public bool shaking;

	public IEnumerator CameraShake(){
		StartCoroutine (ShakeIt ());
		yield return new WaitForSeconds (.1f);
		shaking = false;
	}
	
	public IEnumerator ShakeIt(){
		shaking = true;
		startSpot = transform.position;
		while (shaking) {
			shift = new Vector3( Random.insideUnitCircle.x,Random.insideUnitCircle.y,-10) * .2f;
			transform.position = startSpot + shift;
			yield return null;
		}
		transform.position = new Vector3 (0f, 0f, -10f);
		yield return null;
	}
}
