using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour {

	[SerializeField] private Wave[] allWaves;
	private IWaveRunnable[] waves;
	const float wavePauseTime = 10f;

	void Awake(){
		waves = (IWaveRunnable[])allWaves;
		StartCoroutine (RunWaves());
	}

	IEnumerator RunWaves(){
		yield return null;
		foreach (IWaveRunnable wave in waves){
			yield return StartCoroutine (wave.RunWave());
			yield return new WaitForSeconds(wavePauseTime);
		}
	}
}
