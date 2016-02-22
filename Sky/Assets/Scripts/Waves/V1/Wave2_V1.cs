using UnityEngine;
using System.Collections;
using GenericFunctions;

public class Wave2_V1 : WaveEngine, IWave1to2 {

	[SerializeField] private Wave3_V1 wave3; private IWave2to3 Wave3;

	void Awake(){
		Wave3 = (IWave2to3)wave3;
	}

	//DUCKS (+pigeons)
	IEnumerator IWave1to2.RunWave2(){
		waveNumber = 2;
		ResetWaveCounters ();
		
		//1 Bottom Duck
		//Right - Left
		yield return new WaitForSeconds(wavePauseTime);
		SpawnBirds (BirdType.Duck, Constants.FixedSpawnHeight(1,-1));
		
		//3 Bottom Ducks
		//Right - Left
		yield return StartCoroutine (WaitUntilAliveOnScreen (BirdType.Duck,0));
		yield return StartCoroutine (WaitUntilTimeRange ());
		yield return StartCoroutine (MassProduce_FixedSide_FixedHeight_1Bird (BirdType.Duck,3,4,BirdType.Duck,1,-1,1,1));
		
		
		//1 Top Duck
		//Right - Left
		yield return StartCoroutine (WaitUntilAliveOnScreen (BirdType.Duck,0));
		yield return StartCoroutine (WaitUntilTimeRange ());
		SpawnBirds (BirdType.Duck, Constants.FixedSpawnHeight(1,1));
		
		//3 Top Ducks
		//Right - Left
		yield return StartCoroutine (WaitUntilAliveOnScreen (BirdType.Duck,0));
		yield return StartCoroutine (WaitUntilTimeRange ());
		yield return StartCoroutine (MassProduce_FixedSide_FixedHeight_1Bird (BirdType.Duck,3,4,BirdType.Duck,1,1,1,1));
		
		//1 each Bot-Top Duck (2x1)
		//Right - Left
		yield return StartCoroutine (WaitUntilAliveOnScreen (BirdType.Duck,0));
		yield return StartCoroutine (WaitUntilTimeRange ());
		SpawnBirds (BirdType.Duck, Constants.FixedSpawnHeight(1,-1));
		SpawnBirds (BirdType.Duck, Constants.FixedSpawnHeight(1,1));
		
		//3 each Bot-Top Duck (2x3)
		//Right - Left
		yield return StartCoroutine (WaitUntilAliveOnScreen (BirdType.Duck,0));
		yield return StartCoroutine (WaitUntilTimeRange ());
		StartCoroutine (MassProduce_FixedSide_FixedHeight_1Bird (BirdType.Duck,3,6,BirdType.Duck,1,-1,1,1));
		yield return StartCoroutine (MassProduce_FixedSide_FixedHeight_1Bird (BirdType.Duck,3,6,BirdType.Duck,1,1,1,1));
		
		
		//2 ducks split mid
		yield return StartCoroutine (WaitUntilAliveOnScreen (0));
		yield return StartCoroutine (WaitUntilTimeRange ());
		SpawnBirds (BirdType.Duck, Constants.FixedSpawnHeight(1,0),1);
		SpawnBirds (BirdType.Duck, Constants.FixedSpawnHeight(1,0),3);
		
		//2 ducks split mid X3
		yield return StartCoroutine (WaitUntilAliveOnScreen (0));
		yield return StartCoroutine (WaitUntilTimeRange ());
		for (int i=0; i<3; i++){
			SpawnBirds (BirdType.Duck, Constants.FixedSpawnHeight(1,0),1);
			SpawnBirds (BirdType.Duck, Constants.FixedSpawnHeight(1,0),3);
			yield return new WaitForSeconds(1f);
		}
		yield return StartCoroutine (WaitUntilTimeRange ());
		
		//REWARD
		//PAUSE until life tally total drops to 0
		yield return StartCoroutine (WaitUntilAliveOnScreen (0));
		yield return new WaitForSeconds (2f);
		SpawnBirds (BirdType.BirdOfParadise, Constants.FixedSpawnHeight(1,lowHeight));
		yield return StartCoroutine (WaitUntilAliveOnScreen (0));
		StartCoroutine (Wave3.RunWave3 ());
	}
}
