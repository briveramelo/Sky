using UnityEngine;
using System.Collections;

namespace GenericFunctions{
	public static class TimeEffects {

		public static IEnumerator SlowTime(float slowDuration, float timeScale){
			Time.timeScale = timeScale;
			float startTime = Time.realtimeSinceStartup;
			while (Time.realtimeSinceStartup - startTime < slowDuration){
				yield return null;
			}
			Time.timeScale = 1f;
			yield return null;
		}
	}
}

