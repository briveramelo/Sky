using UnityEngine;
using System.Collections;

public class TimeEffects : MonoBehaviour {

	public IEnumerator SlowTime(float slowDuration, float timeScale){
		StartCoroutine (Wait4RealSeconds (slowDuration));
		Time.timeScale = timeScale;
		yield return null;
	}

	public IEnumerator Wait4RealSeconds(float slowDuration){
		float startTime = Time.realtimeSinceStartup;
		while (Time.realtimeSinceStartup - startTime < slowDuration){
			yield return null;
		}
		Time.timeScale = 1f;
	}
}


