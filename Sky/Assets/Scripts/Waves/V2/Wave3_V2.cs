using UnityEngine;
using System.Collections;
using GenericFunctions;

/*
public class Wave3_V2 : WaveManager {

	//PIGEONS AND DUCKS
	public IEnumerator Wave3(){
		waveNumber = 3;
		ResetWaveCounters ();
		yield return new WaitForSeconds(wavePauseTime);
		
		//DUCK LEADER
		//Right - Left
		yield return StartCoroutine (WaitUntilTimeRange ());
		yield return StartCoroutine (SpawnBirds (Constants.duckLeader, Constants.FixedSpawnHeight(1,0)));
		
		
		//PIGEONS MAKING A RUNWAY FOR FLYING DUCKS
		yield return StartCoroutine (WaitUntilAliveOnScreen (0));
		yield return StartCoroutine (WaitUntilTimeRange ());
		StartCoroutine (SpawnBirds (Constants.pigeon, Constants.FixedSpawnHeight(1,highHeight)));
		StartCoroutine (SpawnBirds (Constants.pigeon, Constants.FixedSpawnHeight(1,lowHeight)));
		yield return new WaitForSeconds (.5f);
		StartCoroutine (SpawnBirds (Constants.pigeon, Constants.FixedSpawnHeight(1,highHeight)));
		StartCoroutine (SpawnBirds (Constants.pigeon, Constants.FixedSpawnHeight(1,lowHeight)));
		yield return new WaitForSeconds (.5f);
		StartCoroutine (SpawnBirds (Constants.pigeon, Constants.FixedSpawnHeight(1,highHeight)));
		StartCoroutine (SpawnBirds (Constants.pigeon, Constants.FixedSpawnHeight(1,lowHeight)));
		StartCoroutine (SpawnBirds (Constants.duckLeader, Constants.FixedSpawnHeight(1,0)));
		yield return new WaitForSeconds (.5f);
		StartCoroutine (SpawnBirds (Constants.pigeon, Constants.FixedSpawnHeight(1,highHeight)));
		StartCoroutine (SpawnBirds (Constants.pigeon, Constants.FixedSpawnHeight(1,lowHeight)));
		yield return new WaitForSeconds (.5f);
		StartCoroutine (SpawnBirds (Constants.pigeon, Constants.FixedSpawnHeight(1,highHeight)));
		yield return StartCoroutine (SpawnBirds (Constants.pigeon, Constants.FixedSpawnHeight(1,lowHeight)));
		
		
		//PIGEONS MIMICKING FLYING DUCKS
		yield return StartCoroutine (WaitUntilAliveOnScreen (0));
		yield return StartCoroutine (WaitUntilTimeRange ());
		StartCoroutine (SpawnBirds (Constants.pigeon, Constants.FixedSpawnHeight(1,0)));
		yield return new WaitForSeconds (.5f);
		StartCoroutine (SpawnBirds (Constants.pigeon, Constants.FixedSpawnHeight(1,.1f)));
		StartCoroutine (SpawnBirds (Constants.pigeon, Constants.FixedSpawnHeight(1,-.1f)));
		yield return new WaitForSeconds (.5f);
		StartCoroutine (SpawnBirds (Constants.pigeon, Constants.FixedSpawnHeight(1,.2f)));
		StartCoroutine (SpawnBirds (Constants.pigeon, Constants.FixedSpawnHeight(1,-.2f)));
		yield return new WaitForSeconds (.5f);
		StartCoroutine (SpawnBirds (Constants.pigeon, Constants.FixedSpawnHeight(1,.3f)));
		StartCoroutine (SpawnBirds (Constants.pigeon, Constants.FixedSpawnHeight(1,-.3f)));
		yield return StartCoroutine (SpawnBirds (Constants.duckLeader, Constants.FixedSpawnHeight(1,0)));
		
		
		//1 each LOW (2x1)
		//		yield return StartCoroutine (WaitUntilAliveOnScreen (0));
		//		yield return StartCoroutine (WaitUntilTimeRange ());
		//		StartCoroutine (SpawnBirds (Constants.pigeon, Constants.FixedSpawnHeight(1,lowHeight)));
		//		yield return StartCoroutine (SpawnBirds (Constants.duck, Constants.FixedSpawnHeight(1,-1)));
		//
		//		//3 each LOW (2x3)
		//		yield return StartCoroutine (WaitUntilAliveOnScreen (0));
		//		yield return StartCoroutine (WaitUntilTimeRange ());
		//		StartCoroutine (MassProduce_FixedSide_FixedHeight_1Bird (Constants.pigeon,3,3,Constants.pigeon,1,lowHeight,1,1));
		//		yield return StartCoroutine (MassProduce_FixedSide_FixedHeight_1Bird (Constants.duck,3,3,Constants.duck,1,-1,1,1));
		//
		//		//1 each HIGH (2x1)
		//		yield return StartCoroutine (WaitUntilAliveOnScreen (0));
		//		yield return StartCoroutine (WaitUntilTimeRange ());
		//		StartCoroutine (SpawnBirds (Constants.pigeon, Constants.FixedSpawnHeight(1,highHeight)));
		//		yield return StartCoroutine (SpawnBirds (Constants.duck, Constants.FixedSpawnHeight(1,1),1));
		//
		//		//3 each HIGH (2x3)
		//		yield return StartCoroutine (WaitUntilAliveOnScreen (0));
		//		yield return StartCoroutine (WaitUntilTimeRange ());
		//		StartCoroutine (MassProduce_FixedSide_FixedHeight_1Bird (Constants.pigeon,3,3,Constants.pigeon,1,highHeight,1,1));
		//		yield return StartCoroutine (MassProduce_FixedSide_FixedHeight_1Bird (Constants.duck,3,3,Constants.duck,1,1,1,1));
		
		
		
		//REWARD
		//PAUSE until life tally total drops to 0
		yield return StartCoroutine (WaitUntilAliveOnScreen (0));
		yield return new WaitForSeconds (2f);
		yield return StartCoroutine (SpawnBirds (Constants.birdOfParadise, Constants.FixedSpawnHeight(1,lowHeight)));
		yield return StartCoroutine (WaitUntilAliveOnScreen (0));
	}
}
*/