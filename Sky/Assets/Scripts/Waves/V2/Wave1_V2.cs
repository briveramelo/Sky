using UnityEngine;
using System.Collections;
using GenericFunctions;

public class Wave1_V2 : WaveManager {

	public Wave2_V2 wave2;
	
	void Awake(){
		wave2 = GetComponent<Wave2_V2> ();
		StartCoroutine (Wave1 ());
	}

	//PIGEONS
	public IEnumerator Wave1(){
		waveNumber = 1;
		ResetWaveCounters ();
		
		//RIGHT TO LEFT
		
		// 1 LOW
		yield return new WaitForSeconds(wavePauseTime);
		yield return StartCoroutine (SpawnBirds (Constants.pigeon, Constants.FixedSpawnHeight(1,lowHeight)));
		
		// 3 LOW
		yield return StartCoroutine (WaitUntilAliveOnScreen (0));
		yield return StartCoroutine (WaitUntilTimeRange ());
		yield return StartCoroutine (MassProduce_FixedSide_FixedHeight_1Bird (Constants.pigeon, 3, 3, Constants.pigeon, 1, lowHeight, 1f, 1f));
		
		// 1 MID
		yield return StartCoroutine (WaitUntilAliveOnScreen (0));
		yield return StartCoroutine (WaitUntilTimeRange ());
		yield return StartCoroutine (SpawnBirds (Constants.pigeon, Constants.FixedSpawnHeight(1,medHeight)));
		
		// 3 MID
		yield return StartCoroutine (WaitUntilAliveOnScreen (0));
		yield return StartCoroutine (WaitUntilTimeRange ());
		yield return StartCoroutine (MassProduce_FixedSide_FixedHeight_1Bird (Constants.pigeon, 3, 3, Constants.pigeon, 1, medHeight, 1f, 1f));
		
		// 1 HIGH
		yield return StartCoroutine (WaitUntilAliveOnScreen (0));
		yield return StartCoroutine (WaitUntilTimeRange ());
		yield return StartCoroutine (SpawnBirds (Constants.pigeon, Constants.FixedSpawnHeight(1,highHeight)));
		
		// 3 HIGH
		yield return StartCoroutine (WaitUntilAliveOnScreen (0));
		yield return StartCoroutine (WaitUntilTimeRange ());
		yield return StartCoroutine (MassProduce_FixedSide_FixedHeight_1Bird (Constants.pigeon, 3, 3, Constants.pigeon, 1, highHeight, 1f, 1f));
		
		//1 each LOW-MED (2x1)
		yield return StartCoroutine (WaitUntilAliveOnScreen (0));
		yield return StartCoroutine (WaitUntilTimeRange ());
		StartCoroutine (SpawnBirds (Constants.pigeon, Constants.FixedSpawnHeight(1,lowHeight)));
		yield return StartCoroutine (SpawnBirds (Constants.pigeon, Constants.FixedSpawnHeight(1,medHeight)));
		
		//3 each LOW-MED (2x3)
		yield return StartCoroutine (WaitUntilAliveOnScreen (0));
		yield return StartCoroutine (WaitUntilTimeRange ());
		StartCoroutine (MassProduce_FixedSide_FixedHeight_1Bird (Constants.pigeon, 3, 3, Constants.pigeon, 1, lowHeight, 1f, 1f));
		yield return StartCoroutine (MassProduce_FixedSide_FixedHeight_1Bird (Constants.pigeon, 3, 6, Constants.pigeon, 1, medHeight, 1f, 1f));
		
		//1 each LOW-HIGH (2x1)
		yield return StartCoroutine (WaitUntilAliveOnScreen (0));
		yield return StartCoroutine (WaitUntilTimeRange ());
		StartCoroutine (SpawnBirds (Constants.pigeon, Constants.FixedSpawnHeight(1,lowHeight)));
		yield return StartCoroutine (SpawnBirds (Constants.pigeon, Constants.FixedSpawnHeight(1,highHeight)));
		
		//3 each LOW-HIGH (2x)
		yield return StartCoroutine (WaitUntilAliveOnScreen (0));
		yield return StartCoroutine (WaitUntilTimeRange ());
		StartCoroutine (MassProduce_FixedSide_FixedHeight_1Bird (Constants.pigeon, 3, 3, Constants.pigeon, 1, lowHeight, 1f, 1f));
		yield return StartCoroutine (MassProduce_FixedSide_FixedHeight_1Bird (Constants.pigeon, 3, 6, Constants.pigeon, 1, highHeight, 1f, 1f));
		
		
		//REWARD
		//PAUSE until life tally total drops to 0
		yield return StartCoroutine (WaitUntilAliveOnScreen (0));
		yield return new WaitForSeconds(2f);
		yield return StartCoroutine (SpawnBirds (Constants.birdOfParadise, Constants.FixedSpawnHeight(1,lowHeight)));
		yield return StartCoroutine (WaitUntilAliveOnScreen (0));
		StartCoroutine (wave2.Wave2 ());
	}

}
