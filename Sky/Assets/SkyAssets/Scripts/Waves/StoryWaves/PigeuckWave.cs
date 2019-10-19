using UnityEngine;
using System.Collections;

public class PigeuckWave : Wave {

	//PIGEONS AND DUCKS
	protected override IEnumerator GenerateBirds(){		
		//DUCK LEADER
		SpawnBirds (BirdType.DuckLeader, SpawnPoint(Right,0));
		yield return StartCoroutine (WaitFor (AllDead, true));
		
		//PIGEONS + DUCKS SWEEP TOGETHER 2x1, + 2x3
		float[] pigeonHeights = {LowHeight,HighHeight};
		float[] duckHeights = {-1,1};
		DuckDirection[] duckDirections = {DuckDirection.UpLeft, DuckDirection.DownLeft};
		PigeuckDelegate spawnPigeucks = AtHeight(pigeonHeights,duckHeights, duckDirections);
		for (int i=0; i<pigeonHeights.Length; i++){
			yield return StartCoroutine(Produce1Wait3(()=>spawnPigeucks(i)));
		}

		//PIGEONS MAKING A RUNWAY FOR FLYING DUCKS
		SpawnDelegate spawnPigeons = AtHeights(pigeonHeights);
		for (int i=0; i<4; i++){
            spawnPigeons();
			if (i==2) SpawnBirds (BirdType.DuckLeader, SpawnPoint(Right,0));
			yield return new WaitForSeconds (.5f);
		}
		yield return StartCoroutine (WaitFor (AllDead, true));
		
		//PIGEONS MIMICKING FLYING DUCKS
		yield return StartCoroutine(FlyPigeonsAsDuckLeader());
		yield return StartCoroutine (WaitFor (AllDead, true));
	}

	private delegate void PigeuckDelegate(int i);

	private PigeuckDelegate AtHeight(float[] pigeonHeights, float[] duckHeights, DuckDirection[] directions){
		return i =>{
			SpawnBirds(BirdType.Pigeon,SpawnPoint(Right,pigeonHeights[i]));
			SpawnBirds (BirdType.Duck, SpawnPoint(Right,duckHeights[i]), directions[i]);
		};
	}

	private SpawnDelegate AtHeights(float[] myHeights){
		return ()=>{
			for (int i=0; i<myHeights.Length; i++){
				SpawnBirds (BirdType.Pigeon, SpawnPoint(Right,myHeights[i]));
			}
		};
	}
}