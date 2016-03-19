using UnityEngine;
using System.Collections;
using GenericFunctions;

public class DuckWave : Wave {

	//DUCKS (+pigeons)
	protected override IEnumerator RunWave(){
		yield return new WaitForSeconds(wavePauseTime);
		//1 Bottom Duck
		//Right - Left
		float[] heights = new float[]{-1,1};
		DuckDirection[] directions = new DuckDirection[]{DuckDirection.UpLeft, DuckDirection.DownLeft};
		for (int i=0; i<heights.Length; i++){
			yield return StartCoroutine (WaitUntilRemaining (0, true));
			SpawnBirds (BirdType.Duck, SpawnPoint(right,heights[i]),directions[i]);
			yield return StartCoroutine (WaitUntilRemaining (0, true));
			yield return StartCoroutine (MassProduce (BirdType.Duck,3,right,heights[i],directions[i]));
		}

		//1 each Bot-Top Duck (2x1)
		//Right - Left
		yield return StartCoroutine (WaitUntilRemaining (0, true));
		SpawnBirds (BirdType.Duck, SpawnPoint(right,-1), DuckDirection.UpLeft);
		SpawnBirds (BirdType.Duck, SpawnPoint(right,1), DuckDirection.DownLeft);
		
		//3 each Bot-Top Duck (2x3)
		//Right - Left
		yield return StartCoroutine (WaitUntilRemaining (0, true));
		StartCoroutine (MassProduce(BirdType.Duck,3,right,-1,DuckDirection.UpLeft));
		yield return StartCoroutine (MassProduce(BirdType.Duck,3,right,1,DuckDirection.DownLeft));

		//2 ducks split mid X3
		yield return StartCoroutine (WaitUntilRemaining (0, true));
		for (int i=0; i<4; i++){
			SpawnBirds (BirdType.Duck, SpawnPoint(right,0),DuckDirection.UpLeft);
			SpawnBirds (BirdType.Duck, SpawnPoint(right,0),DuckDirection.DownLeft);
			if (i==0) yield return StartCoroutine(WaitUntilRemaining(0,true));
			else yield return new WaitForSeconds(1f);
		}
		yield return StartCoroutine (base.RunWave());
	}
}