using UnityEngine;
using System.Collections;
using GenericFunctions;

public class PidgeonWave : Wave {

	//PIGEONS
	protected override IEnumerator RunWave(){
		//RIGHT TO LEFT
		yield return new WaitForSeconds(wavePauseTime);
	
		float[] heights= new float[]{lowHeight,medHeight,highHeight}; 
		for (int i=0; i<heights.Length; i++){
			yield return StartCoroutine (WaitUntilRemaining (0, true));
			SpawnBirds (BirdType.Pigeon, SpawnPoint(right,heights[i]));
			yield return StartCoroutine (WaitUntilRemaining (0, true));
			yield return StartCoroutine (MassProduce (BirdType.Pigeon, 3, right, heights[i]));
		}

		//Bot
		float[] bottomHeights= new float[]{lowHeight,lowHeight,medHeight};
		float[] topHeights= new float[]{medHeight,highHeight,highHeight};
		for (int i=0; i<bottomHeights.Length; i++){
			yield return StartCoroutine (WaitUntilRemaining (0, true));
			SpawnBirds (BirdType.Pigeon, SpawnPoint(right,bottomHeights[i]));
			SpawnBirds (BirdType.Pigeon, SpawnPoint(right,topHeights[i]));
			yield return StartCoroutine (WaitUntilRemaining (0, true));
			StartCoroutine (MassProduce (BirdType.Pigeon, 3, right, bottomHeights[i]));
			yield return StartCoroutine (MassProduce (BirdType.Pigeon, 3, right, topHeights[i]));
		}

		yield return StartCoroutine (base.RunWave());
	}
}
