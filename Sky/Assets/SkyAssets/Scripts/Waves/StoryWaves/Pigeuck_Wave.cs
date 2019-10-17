using UnityEngine;
using System.Collections;

public class Pigeuck_Wave : Wave {

	//PIGEONS AND DUCKS
	protected override IEnumerator GenerateBirds(){		
		//DUCK LEADER
		SpawnBirds (BirdType.DuckLeader, SpawnPoint(right,0));
		yield return StartCoroutine (WaitFor (allDead, true));
		
		//PIGEONS + DUCKS SWEEP TOGETHER 2x1, + 2x3
		float[] pigeonHeights = {lowHeight,highHeight};
		float[] duckHeights = {-1,1};
		DuckDirection[] duckDirections = {DuckDirection.UpLeft, DuckDirection.DownLeft};
		PigeuckDelegate SpawnPigeucks = AtHeight(pigeonHeights,duckHeights, duckDirections);
		for (int i=0; i<pigeonHeights.Length; i++){
			yield return StartCoroutine(Produce1Wait3(()=>SpawnPigeucks(i)));
		}

		//PIGEONS MAKING A RUNWAY FOR FLYING DUCKS
		SpawnDelegate SpawnPigeons = AtHeights(pigeonHeights);
		for (int i=0; i<4; i++){
            SpawnPigeons();
			if (i==2) SpawnBirds (BirdType.DuckLeader, SpawnPoint(right,0));
			yield return new WaitForSeconds (.5f);
		}
		yield return StartCoroutine (WaitFor (allDead, true));
		
		//PIGEONS MIMICKING FLYING DUCKS
		yield return StartCoroutine(FlyPigeonsAsDuckLeader());
		yield return StartCoroutine (WaitFor (allDead, true));
	}

	private delegate void PigeuckDelegate(int i);

	private PigeuckDelegate AtHeight(float[] pigeonHeights, float[] duckHeights, DuckDirection[] directions){
		return i =>{
			SpawnBirds(BirdType.Pigeon,SpawnPoint(right,pigeonHeights[i]));
			SpawnBirds (BirdType.Duck, SpawnPoint(right,duckHeights[i]), directions[i]);
		};
	}

	private SpawnDelegate AtHeights(float[] myHeights){
		return ()=>{
			for (int i=0; i<myHeights.Length; i++){
				SpawnBirds (BirdType.Pigeon, SpawnPoint(right,myHeights[i]));
			}
		};
	}
}