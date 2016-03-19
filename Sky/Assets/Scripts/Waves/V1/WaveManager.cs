using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour {

	[SerializeField] private Wave[] allWaves;
	private IWaveRunnable[] waves;

	void Awake(){
		waves = (IWaveRunnable[])allWaves;
		StartCoroutine (RunWaves());
	}

	IEnumerator RunWaves(){
		foreach (IWaveRunnable wave in waves){
			yield return StartCoroutine (wave.RunWave());
		}
	}
}
