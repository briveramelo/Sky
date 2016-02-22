using UnityEngine;
using System.Collections;
using GenericFunctions;

public class Wave1_V1 : WaveEngine {

	[SerializeField] private Wave2_V1 wave2; private IWave1to2 Wave2;

	void Awake(){
		Wave2 = (IWave1to2)wave2;
		StartCoroutine (RunWave1 ());
	}

	//PIGEONS
	IEnumerator RunWave1(){
		waveNumber = 1;
		ResetWaveCounters ();
		
		//RIGHT TO LEFT
		
		// 1 LOW
		yield return new WaitForSeconds(wavePauseTime);
		SpawnBirds (BirdType.Pigeon, Constants.FixedSpawnHeight(1,lowHeight));

		// 3 LOW
		yield return StartCoroutine (WaitUntilAliveOnScreen (0));
		yield return StartCoroutine (WaitUntilTimeRange ());
		yield return StartCoroutine (MassProduce_FixedSide_FixedHeight_1Bird (BirdType.Pigeon, 3, 3, BirdType.Pigeon, 1, lowHeight, 1f, 1f));

		// 1 MID
		yield return StartCoroutine (WaitUntilAliveOnScreen (0));
		yield return StartCoroutine (WaitUntilTimeRange ());
		SpawnBirds (BirdType.Pigeon, Constants.FixedSpawnHeight(1,medHeight));
		
		// 3 MID
		yield return StartCoroutine (WaitUntilAliveOnScreen (0));
		yield return StartCoroutine (WaitUntilTimeRange ());
		yield return StartCoroutine (MassProduce_FixedSide_FixedHeight_1Bird (BirdType.Pigeon, 3, 3, BirdType.Pigeon, 1, medHeight, 1f, 1f));
		
		// 1 HIGH
		yield return StartCoroutine (WaitUntilAliveOnScreen (0));
		yield return StartCoroutine (WaitUntilTimeRange ());
		SpawnBirds (BirdType.Pigeon, Constants.FixedSpawnHeight(1,highHeight));
		
		// 3 HIGH
		yield return StartCoroutine (WaitUntilAliveOnScreen (0));
		yield return StartCoroutine (WaitUntilTimeRange ());
		yield return StartCoroutine (MassProduce_FixedSide_FixedHeight_1Bird (BirdType.Pigeon, 3, 3, BirdType.Pigeon, 1, highHeight, 1f, 1f));
		
		//1 each LOW-MED (2x1)
		yield return StartCoroutine (WaitUntilAliveOnScreen (0));
		yield return StartCoroutine (WaitUntilTimeRange ());
		SpawnBirds (BirdType.Pigeon, Constants.FixedSpawnHeight(1,lowHeight));
		SpawnBirds (BirdType.Pigeon, Constants.FixedSpawnHeight(1,medHeight));
		
		//3 each LOW-MED (2x3)
		yield return StartCoroutine (WaitUntilAliveOnScreen (0));
		yield return StartCoroutine (WaitUntilTimeRange ());
		StartCoroutine (MassProduce_FixedSide_FixedHeight_1Bird (BirdType.Pigeon, 3, 3, BirdType.Pigeon, 1, lowHeight, 1f, 1f));
		yield return StartCoroutine (MassProduce_FixedSide_FixedHeight_1Bird (BirdType.Pigeon, 3, 6, BirdType.Pigeon, 1, medHeight, 1f, 1f));
		
		//1 each LOW-HIGH (2x1)
		yield return StartCoroutine (WaitUntilAliveOnScreen (0));
		yield return StartCoroutine (WaitUntilTimeRange ());
		SpawnBirds (BirdType.Pigeon, Constants.FixedSpawnHeight(1,lowHeight));
		SpawnBirds (BirdType.Pigeon, Constants.FixedSpawnHeight(1,highHeight));
		
		//3 each LOW-HIGH (2x)
		yield return StartCoroutine (WaitUntilAliveOnScreen (0));
		yield return StartCoroutine (WaitUntilTimeRange ());
		StartCoroutine (MassProduce_FixedSide_FixedHeight_1Bird (BirdType.Pigeon, 3, 3, BirdType.Pigeon, 1, lowHeight, 1f, 1f));
		yield return StartCoroutine (MassProduce_FixedSide_FixedHeight_1Bird (BirdType.Pigeon, 3, 6, BirdType.Pigeon, 1, highHeight, 1f, 1f));
		
		
		//REWARD
		//PAUSE until life tally total drops to 0
		yield return StartCoroutine (WaitUntilAliveOnScreen (0));
		yield return new WaitForSeconds(2f);
		SpawnBirds (BirdType.BirdOfParadise, Constants.FixedSpawnHeight(1,lowHeight));
		yield return StartCoroutine (WaitUntilAliveOnScreen (0));
		StartCoroutine (Wave2.RunWave2 ());
	}
}
