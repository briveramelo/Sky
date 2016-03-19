using UnityEngine;
using System.Collections;
using GenericFunctions;

public class PidguckWave : Wave {

	//PIGEONS AND DUCKS
	protected override IEnumerator RunWave(){
		yield return new WaitForSeconds(wavePauseTime);
		
		//DUCK LEADER
		//Right - Left
		yield return StartCoroutine (WaitUntilTimeRange ());
		SpawnBirds (BirdType.DuckLeader, SpawnPoint(right,0));
		
		//PIGEONS MAKING A RUNWAY FOR FLYING DUCKS
		yield return StartCoroutine (WaitUntilRemaining (0, true));
		for (int i=0; i<4; i++){
			SpawnBirds (BirdType.Pigeon, SpawnPoint(right,highHeight));
			SpawnBirds (BirdType.Pigeon, SpawnPoint(right,lowHeight));
			if (i==2) SpawnBirds (BirdType.DuckLeader, SpawnPoint(right,0));
			yield return new WaitForSeconds (.5f);
		}
		
		//PIGEONS MIMICKING FLYING DUCKS
		yield return StartCoroutine (WaitUntilRemaining (0, true));
		SpawnBirds (BirdType.Pigeon, SpawnPoint(right,0));
		for (int i=0; i<3; i++){
			yield return new WaitForSeconds (.5f);
			SpawnBirds (BirdType.Pigeon, SpawnPoint(right,.1f * (i+1)));
			SpawnBirds (BirdType.Pigeon, SpawnPoint(right,-.1f * (i+1)));
		}
		SpawnBirds (BirdType.DuckLeader, SpawnPoint(right,0));

		//PIGEONS + DUCKS SWEEP TOGETHER 2x1, + 2x3
		float[] pigeonHeights = new float[]{lowHeight,highHeight};
		float[] duckHeights = new float[]{-1,1};
		DuckDirection[] duckDirections = new DuckDirection[]{DuckDirection.UpLeft, DuckDirection.DownLeft};
		for (int i=0; i<pigeonHeights.Length; i++){
			yield return StartCoroutine (WaitUntilRemaining (0, true));
			for (int j=0; j<4; j++){
				SpawnBirds (BirdType.Pigeon, SpawnPoint(right,pigeonHeights[i]));
				SpawnBirds (BirdType.Duck, SpawnPoint(right,duckHeights[i]), duckDirections[i]);
				if (j==0) yield return StartCoroutine (WaitUntilRemaining (0, true));
				else yield return new WaitForSeconds(1f);
			}
		}

		yield return StartCoroutine (base.RunWave());
	}
}
