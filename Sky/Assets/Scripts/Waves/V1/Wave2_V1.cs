using UnityEngine;
using System.Collections;
using GenericFunctions;
/*
public class Wave2_V1 : WaveManager {

	public Wave3_V1 wave3;
	
	void Awake(){
		wave3 = GetComponent<Wave3_V1> ();
	}

	//DUCKS (+pigeons)
	public IEnumerator Wave2(){
		waveNumber = 2;
		ResetWaveCounters ();
		
		//1 Bottom Duck
		//Right - Left
		yield return new WaitForSeconds(wavePauseTime);
		yield return StartCoroutine (SpawnBirds (Constants.duck, Constants.FixedSpawnHeight(1,-1)));
		
		//3 Bottom Ducks
		//Right - Left
		yield return StartCoroutine (WaitUntilAliveOnScreen (Constants.duck,0));
		yield return StartCoroutine (WaitUntilTimeRange ());
		yield return StartCoroutine (MassProduce_FixedSide_FixedHeight_1Bird (Constants.duck,3,4,Constants.duck,1,-1,1,1));
		
		
		//1 Top Duck
		//Right - Left
		yield return StartCoroutine (WaitUntilAliveOnScreen (Constants.duck,0));
		yield return StartCoroutine (WaitUntilTimeRange ());
		yield return StartCoroutine (SpawnBirds (Constants.duck, Constants.FixedSpawnHeight(1,1)));
		
		//3 Top Ducks
		//Right - Left
		yield return StartCoroutine (WaitUntilAliveOnScreen (Constants.duck,0));
		yield return StartCoroutine (WaitUntilTimeRange ());
		yield return StartCoroutine (MassProduce_FixedSide_FixedHeight_1Bird (Constants.duck,3,4,Constants.duck,1,1,1,1));
		
		//1 each Bot-Top Duck (2x1)
		//Right - Left
		yield return StartCoroutine (WaitUntilAliveOnScreen (Constants.duck,0));
		yield return StartCoroutine (WaitUntilTimeRange ());
		StartCoroutine (SpawnBirds (Constants.duck, Constants.FixedSpawnHeight(1,-1)));
		yield return StartCoroutine (SpawnBirds (Constants.duck, Constants.FixedSpawnHeight(1,1)));
		
		//3 each Bot-Top Duck (2x3)
		//Right - Left
		yield return StartCoroutine (WaitUntilAliveOnScreen (Constants.duck,0));
		yield return StartCoroutine (WaitUntilTimeRange ());
		StartCoroutine (MassProduce_FixedSide_FixedHeight_1Bird (Constants.duck,3,6,Constants.duck,1,-1,1,1));
		yield return StartCoroutine (MassProduce_FixedSide_FixedHeight_1Bird (Constants.duck,3,6,Constants.duck,1,1,1,1));
		
		
		//2 ducks split mid
		yield return StartCoroutine (WaitUntilAliveOnScreen (0));
		yield return StartCoroutine (WaitUntilTimeRange ());
		StartCoroutine (SpawnBirds (Constants.duck, Constants.FixedSpawnHeight(1,0),1));
		yield return StartCoroutine (SpawnBirds (Constants.duck, Constants.FixedSpawnHeight(1,0),3));
		
		//2 ducks split mid X3
		yield return StartCoroutine (WaitUntilAliveOnScreen (0));
		yield return StartCoroutine (WaitUntilTimeRange ());
		for (int i=0; i<3; i++){
			StartCoroutine (SpawnBirds (Constants.duck, Constants.FixedSpawnHeight(1,0),1));
			StartCoroutine (SpawnBirds (Constants.duck, Constants.FixedSpawnHeight(1,0),3));
			yield return new WaitForSeconds(1f);
		}
		yield return StartCoroutine (WaitUntilTimeRange ());
		
		//REWARD
		//PAUSE until life tally total drops to 0
		yield return StartCoroutine (WaitUntilAliveOnScreen (0));
		yield return new WaitForSeconds (2f);
		yield return StartCoroutine (SpawnBirds (Constants.birdOfParadise, Constants.FixedSpawnHeight(1,lowHeight)));
		yield return StartCoroutine (WaitUntilAliveOnScreen (0));
		StartCoroutine (wave3.Wave3 ());
	}
}
*/