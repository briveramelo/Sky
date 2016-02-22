using UnityEngine;
using System.Collections;

public class GameClock : MonoBehaviour {

	private static GameClock instance;
	public static GameClock Instance{get{return instance;}}

	void Awake(){
		instance = this;
	}

	public void SlowTime(float slowDuration, float timeScale){
		Time.timeScale = timeScale;
		StartCoroutine (Wait4RealSeconds (slowDuration));
	}

	IEnumerator Wait4RealSeconds(float slowDuration){
		float startTime = Time.realtimeSinceStartup;
		while (Time.realtimeSinceStartup - startTime < slowDuration){
			yield return null;
		}
		Time.timeScale = 1f;
	}
}
