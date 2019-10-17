using UnityEngine;
using System.Collections;

public class GameClock : MonoBehaviour {

	private static GameClock instance;
	public static GameClock Instance => instance;

	void Awake(){
        if (instance == null) {
            instance = this;
        }
        else {
            Destroy(gameObject);
        }
	}

	public void SlowTime(float slowDuration, float timeScale){
		StopAllCoroutines();
		StartCoroutine (Wait4RealSeconds (slowDuration, timeScale));
	}

	IEnumerator Wait4RealSeconds(float slowDuration, float timeScale){
		Time.timeScale = timeScale;
		yield return new WaitForSeconds(slowDuration);
		Time.timeScale = 1f;
	}
}
