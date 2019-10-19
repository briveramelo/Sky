using UnityEngine;
using System.Collections;

public class GameClock : Singleton<GameClock> {

	public void SlowTime(float slowDuration, float timeScale){
		StopAllCoroutines();
		StartCoroutine (Wait4RealSeconds (slowDuration, timeScale));
	}

	private IEnumerator Wait4RealSeconds(float slowDuration, float timeScale){
		Time.timeScale = timeScale;
		yield return new WaitForSeconds(slowDuration);
		Time.timeScale = 1f;
	}
}
