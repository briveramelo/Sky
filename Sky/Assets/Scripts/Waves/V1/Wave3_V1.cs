using UnityEngine;
using System.Collections;
using GenericFunctions;

public class Wave3_V1 : WaveEngine, IWave2to3 {

	//PIGEONS AND DUCKS
	IEnumerator IWave2to3.RunWave3(){
		waveNumber = 3;
		ResetWaveCounters ();
		yield return new WaitForSeconds(wavePauseTime);
		
		//DUCK LEADER
		//Right - Left
		yield return StartCoroutine (WaitUntilTimeRange ());
		SpawnBirds (BirdType.DuckLeader, Constants.FixedSpawnHeight(1,0));
		
		
		//PIGEONS MAKING A RUNWAY FOR FLYING DUCKS
		yield return StartCoroutine (WaitUntilAliveOnScreen (0));
		yield return StartCoroutine (WaitUntilTimeRange ());
		SpawnBirds (BirdType.Pigeon, Constants.FixedSpawnHeight(1,highHeight));
		SpawnBirds (BirdType.Pigeon, Constants.FixedSpawnHeight(1,lowHeight));
		yield return new WaitForSeconds (.5f);
		SpawnBirds (BirdType.Pigeon, Constants.FixedSpawnHeight(1,highHeight));
		SpawnBirds (BirdType.Pigeon, Constants.FixedSpawnHeight(1,lowHeight));
		yield return new WaitForSeconds (.5f);
		SpawnBirds (BirdType.Pigeon, Constants.FixedSpawnHeight(1,highHeight));
		SpawnBirds (BirdType.Pigeon, Constants.FixedSpawnHeight(1,lowHeight));
		SpawnBirds (BirdType.DuckLeader, Constants.FixedSpawnHeight(1,0));
		yield return new WaitForSeconds (.5f);
		SpawnBirds (BirdType.Pigeon, Constants.FixedSpawnHeight(1,highHeight));
		SpawnBirds (BirdType.Pigeon, Constants.FixedSpawnHeight(1,lowHeight));
		yield return new WaitForSeconds (.5f);
		SpawnBirds (BirdType.Pigeon, Constants.FixedSpawnHeight(1,highHeight));
		SpawnBirds (BirdType.Pigeon, Constants.FixedSpawnHeight(1,lowHeight));
		
		
		//PIGEONS MIMICKING FLYING DUCKS
		yield return StartCoroutine (WaitUntilAliveOnScreen (0));
		yield return StartCoroutine (WaitUntilTimeRange ());
		SpawnBirds (BirdType.Pigeon, Constants.FixedSpawnHeight(1,0));
		yield return new WaitForSeconds (.5f);
		SpawnBirds (BirdType.Pigeon, Constants.FixedSpawnHeight(1,.1f));
		SpawnBirds (BirdType.Pigeon, Constants.FixedSpawnHeight(1,-.1f));
		yield return new WaitForSeconds (.5f);
		SpawnBirds (BirdType.Pigeon, Constants.FixedSpawnHeight(1,.2f));
		SpawnBirds (BirdType.Pigeon, Constants.FixedSpawnHeight(1,-.2f));
		yield return new WaitForSeconds (.5f);
		SpawnBirds (BirdType.Pigeon, Constants.FixedSpawnHeight(1,.3f));
		SpawnBirds (BirdType.Pigeon, Constants.FixedSpawnHeight(1,-.3f));
		SpawnBirds (BirdType.DuckLeader, Constants.FixedSpawnHeight(1,0));
		
		
		//1 each LOW (2x1)
		yield return StartCoroutine (WaitUntilAliveOnScreen (0));
		yield return StartCoroutine (WaitUntilTimeRange ());
		SpawnBirds (BirdType.Pigeon, Constants.FixedSpawnHeight(1,lowHeight));
		SpawnBirds (BirdType.Duck, Constants.FixedSpawnHeight(1,-1));

		//3 each LOW (2x3)
		yield return StartCoroutine (WaitUntilAliveOnScreen (0));
		yield return StartCoroutine (WaitUntilTimeRange ());
		StartCoroutine (MassProduce_FixedSide_FixedHeight_1Bird (BirdType.Pigeon,3,3,BirdType.Pigeon,1,lowHeight,1,1));
		yield return StartCoroutine (MassProduce_FixedSide_FixedHeight_1Bird (BirdType.Duck,3,3,BirdType.Duck,1,-1,1,1));

		//1 each HIGH (2x1)
		yield return StartCoroutine (WaitUntilAliveOnScreen (0));
		yield return StartCoroutine (WaitUntilTimeRange ());
		SpawnBirds (BirdType.Pigeon, Constants.FixedSpawnHeight(1,highHeight));
		SpawnBirds (BirdType.Duck, Constants.FixedSpawnHeight(1,1),1);

		//3 each HIGH (2x3)
		yield return StartCoroutine (WaitUntilAliveOnScreen (0));
		yield return StartCoroutine (WaitUntilTimeRange ());
		StartCoroutine (MassProduce_FixedSide_FixedHeight_1Bird (BirdType.Pigeon,3,3,BirdType.Pigeon,1,highHeight,1,1));
		yield return StartCoroutine (MassProduce_FixedSide_FixedHeight_1Bird (BirdType.Duck,3,3,BirdType.Duck,1,1,1,1));
		
		
		
		//REWARD
		//PAUSE until life tally total drops to 0
		yield return StartCoroutine (WaitUntilAliveOnScreen (0));
		yield return new WaitForSeconds (2f);
		SpawnBirds (BirdType.BirdOfParadise, Constants.FixedSpawnHeight(1,lowHeight));
		yield return StartCoroutine (WaitUntilAliveOnScreen (0));
	}
}
