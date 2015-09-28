using UnityEngine;
using System.Collections;
using GenericFunctions;

public class WaveManager : MonoBehaviour {

	public Transform balloonTransform;
	public int waveNumber;
	public float wavePauseTime;
	public float standardPauseTime;
	public float lowHeight;
	public float medHeight;
	public float highHeight;
	public float standardTimeMin;
	public float standardTimeMax;

	//spawn
	public int[] currentWaveAllSpawnCount;
	public int[] allSpawnCount;
	public int currentWaveSpawnCount;
	public int spawnCount;

	//alive
	public int[] currentWaveAllAliveCount;
	public int[] allAliveCount;
	public int currentWaveAliveCount;
	public int aliveCount;

	//killed
	public int[] currentWaveAllKillCount;
	public int[] allKillCount;
	public int currentWaveKillCount;
	public int killCount;

	//pooints!
	public int[] currentWaveAllPoints;
	public int[] allPoints;
	public int currentWavePoints;
	public int points;

	
	void Awake(){
		allKillCount = new int[Constants.birdNamePrefabs.Length];
		allAliveCount = new int[Constants.birdNamePrefabs.Length];
		allSpawnCount = new int[Constants.birdNamePrefabs.Length];
		allPoints = new int[Constants.birdNamePrefabs.Length];
		currentWaveAllKillCount = new int[Constants.birdNamePrefabs.Length];
		currentWaveAllAliveCount = new int[Constants.birdNamePrefabs.Length];
		currentWaveAllSpawnCount = new int[Constants.birdNamePrefabs.Length];
		currentWaveAllPoints = new int[Constants.birdNamePrefabs.Length];

		balloonTransform = GameObject.Find ("BalloonSpot").transform;
		wavePauseTime = 10f;
		standardPauseTime = 1f;
		lowHeight = -0.6f;
		medHeight = 0f;
		standardTimeMin = 1f;
		standardTimeMax = 2f;
		highHeight = -lowHeight;
		StartCoroutine (Wave1 ());
	}

	public IEnumerator DemoWave(){
		yield return null;
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
		StartCoroutine (Wave2 ());
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
		StartCoroutine (Wave3 ());
	}

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
		//StartCoroutine (Wave4 ());
	}
//
//	public IEnumerator Wave4(){
//		waveNumber = 4;
//		ResetWaveCounters ();
//		yield return null;
//		StartCoroutine (Wave5 ());
//	}
//
//	public IEnumerator Wave5(){
//		waveNumber = 5;
//		ResetWaveCounters ();
//		yield return null;
//		StartCoroutine (Wave6 ());
//	}
//
//	public IEnumerator Wave6(){
//		waveNumber = 6;
//		ResetWaveCounters ();
//		yield return null;
//		StartCoroutine (Wave7 ());
//	}
//
//	public IEnumerator Wave7(){
//		waveNumber = 7;
//		ResetWaveCounters ();
//		yield return null;
//		StartCoroutine (Wave8 ());
//	}
//
//	public IEnumerator Wave8(){
//		waveNumber = 8;
//		ResetWaveCounters ();
//		yield return null;
//		StartCoroutine (Wave9 ());
//	}
//
//	public IEnumerator Wave9(){
//		waveNumber = 9;
//		yield return null;
//		StartCoroutine (Wave10 ());
//	}
//
//	public IEnumerator Wave10(){
//		waveNumber = 10;
//		yield return null;
//		//StartCoroutine (SaveLoadData.dataStorage.PromptSave ());
//	}

	/// <summary> Spawn Birds
	/// </summary>
	public IEnumerator SpawnBirds(int birdType, Vector3 spawnPoint,int duckMoveDir =0){ 
		int direction = spawnPoint.x<0 ? 1 : -1;
		GameObject bird = Instantiate (Resources.Load (Constants.birdNamePrefabs [birdType]), spawnPoint, Quaternion.identity) as GameObject;
		bool goLeft = direction == -1 ? true : false;

		if (birdType==Constants.pigeon){
			Pigeon pigeonScript = bird.GetComponent<Pigeon> ();
			pigeonScript.rigidbod.velocity = Vector2.right * direction * pigeonScript.moveSpeed;
			pigeonScript.transform.Face4ward(direction!=1);
		}
		else if (birdType==Constants.duck){
			Duck duckScript = bird.GetComponent<Duck> ();
			duckScript.formationNumber = duckMoveDir;
			StartCoroutine(duckScript.Scatter());
		}
		else if (birdType == Constants.duckLeader){
			DuckLeader duckLeaderScript = bird.GetComponent<DuckLeader> ();
			duckLeaderScript.SetPositions(goLeft);
		}
		else if (birdType==Constants.birdOfParadise){ 	//help stupid pigeons, ducks, and birds of paradise decide which direction to go on start 
			BirdOfParadise birdOfParadiseScript = bird.GetComponent<BirdOfParadise> ();
			birdOfParadiseScript.rigidbod.velocity = Vector2.right * direction * birdOfParadiseScript.moveSpeed;
			birdOfParadiseScript.transform.Face4ward(direction!=1);
		}
		yield return null;
	}

	/// <summary> clamps the input between +/- 80% of the world height
	/// </summary>
	float SecureWorldY(float y){
		return Mathf.Clamp (y, -Constants.worldDimensions.y * .8f, Constants.worldDimensions.y * .8f);
	}

	/// <summary> determines which direction a duck should go 
	/// <para>based on its starting position</para>
	/// </summary>
	int[] RandomDuckSideAndDirection(bool top){
		int xSpot = (int)Mathf.Sign(Random.insideUnitCircle.x);
		int formationNumber;
		if (!top && xSpot == -1){
			formationNumber = 0;
		}
		else if (!top && xSpot == 1){
			formationNumber = 1;
		}
		else if (top && xSpot == -1){
			formationNumber = 2;
		}
		else {
			formationNumber = 3;
		}
		return new int[] {xSpot,formationNumber};
	}

	int RandomDuckDirection(bool right){
		int formationNumber;
		if (right){
			formationNumber = Random.Range(0,2) == 0 ? 1 : 3;
		}
		else{
			formationNumber = Random.Range(0,2) == 0 ? 0 : 2;
		}
		return formationNumber;
	}

	int RandomSide(){
		return (int)Mathf.Sign(Random.insideUnitCircle.x);
	}

	void ResetWaveCounters(){
		currentWaveAllSpawnCount = new int[Constants.birdNamePrefabs.Length];
		currentWaveAllAliveCount = new int[Constants.birdNamePrefabs.Length];
		currentWaveAllKillCount = new int[Constants.birdNamePrefabs.Length];

		currentWaveSpawnCount = 0;
		currentWaveAliveCount = 0;
		currentWaveKillCount = 0;
	}

	//**//**//**//**//**//**//**//**//
	  //WAIT UNTIL FUNCTION FAMILY//
	//**//**//**//**//**//**//**//**//

	//WAIT UNTIL ALIVE

	/// <summary> Wait until at most "maxAlive" "birdTypes" remain alive on screen
	/// </summary>
	public IEnumerator WaitUntilAliveOnScreen(int birdType, int maxAlive){
		while (currentWaveAllAliveCount[birdType] > maxAlive){
			yield return null;
		}
		yield return null;
	}

	/// <summary> Wait until at most "maxAlive" birds remain alive on screen
	/// </summary>
	public IEnumerator WaitUntilAliveOnScreen(int maxAlive){
		while (currentWaveAliveCount > maxAlive){
			yield return null;
		}
		yield return null;
	}

	/// <summary> Wait until at least "deathToll" birds have died in this wave since calling this.
	/// <para>Typically used internal to the "Mass Produce" Function Family to prevent resetting </para>
	/// <para>the starting quantity </para>
	/// </summary>
	public IEnumerator WaitUntilDead(int birdType, int deathToll, int startingQuantity){
		while ((currentWaveAllKillCount[birdType] - startingQuantity) < deathToll){
			yield return null;
		}
		yield return null;
	}

	/// <summary> Wait until at least "deathToll" birds have died
	/// <para> since calling this function</para>
	/// </summary>
	public IEnumerator WaitUntilDead(int birdType, int deathToll){
		int startingQuantity = currentWaveAllKillCount [birdType];
		while ((currentWaveAllKillCount[birdType] - startingQuantity) < deathToll){
			yield return null;
		}
		yield return null;
	}

	/// <summary> Wait until at least "deathToll" birds remain
	/// </summary>
	public IEnumerator WaitUntilDead(int deathToll){
		while (currentWaveKillCount < deathToll){
			yield return null;
		}
		yield return null;
	}

	/// <summary> Wait until at least "minSpawned" "birdTypes" are born
	/// <para>Typically used in the "Mass Produce" Function Family to prevent resetting </para>
	/// <para>the starting quantity </para>
	/// </summary>
	public IEnumerator WaitUntilSpawn(int birdType, int minSpawned, int startingQuantity){
		while ((currentWaveAllSpawnCount[birdType]-startingQuantity)<minSpawned){
			yield return null;
		}
		yield return null;
	}

	/// <summary> Wait until at least "minSpawned" "birdTypes" are born
	/// </summary>
	public IEnumerator WaitUntilSpawn(int birdType, int minSpawned){
		int startingQuantity = currentWaveAllSpawnCount [birdType];
		while ((currentWaveAllSpawnCount[birdType]-startingQuantity)<minSpawned){
			yield return null;
		}
		yield return null;
	}

	/// <summary> Wait until at least "minSpawned" birds are born
	/// </summary>
	public IEnumerator WaitUntilSpawn(int minSpawned){
		int startingQuantity = currentWaveSpawnCount;
		while ((currentWaveSpawnCount-startingQuantity)<minSpawned){
			yield return null;
		}
		yield return null;
	}

	/// <summary> Wait between 1.5-3 seconds
	/// </summary>
	public IEnumerator WaitUntilTimeRange(){
		yield return new WaitForSeconds(Random.Range (1f, 2f));
	}

	/// <summary> Wait between minTime and maxTime seconds
	/// </summary>
	public IEnumerator WaitUntilTimeRange(float minTime, float maxTime){
		yield return new WaitForSeconds(Random.Range(minTime,maxTime));
	}

	/// <summary> Produce some "quantity" of "birdType" birds on the left (-1) or right(+1) side at height "yPosition" (-1 <-> +1)
	/// <para>Waits for its older brother to be born, checking the startingQuantity of older brethren</para>
	/// <para>Waits until at most "maxAlive" "birdToWatch" birds remain alive on screen, typically same as birdType</para>
	/// </summary>
	public IEnumerator MassProduce_FixedSide_FixedHeight_1Bird(int birdType, int quantity, int maxAlive, int birdToWatch, int side, float yPosition, float tMin = 1f, float tMax = 2f){
		int startingQuantity = currentWaveAllSpawnCount [birdType];
		for (int birdCount =0; birdCount<quantity; birdCount++){
			yield return StartCoroutine (WaitUntilSpawn (birdType, birdCount, startingQuantity));
			yield return StartCoroutine (WaitUntilAliveOnScreen (birdToWatch, maxAlive));
			StartCoroutine (SpawnBirds (birdType, Constants.FixedSpawnHeight(side,yPosition),side));
			yield return StartCoroutine (WaitUntilTimeRange(tMin, tMax));
		}
		yield return null;
	}

	/// <summary> Produce some "quantity" of "birdType" birds on the left (-1) or right(+1) side at height "yPosition" (-1 <-> +1)
	/// <para>Waits for its older brother to be born, checking the startingQuantity of older brethren</para>
	/// <para>Waits until at most "maxAlive" total birds remain alive on screen</para>
	/// </summary>
	public IEnumerator MassProduce_FixedSide_FixedHeight_AllBirds(int birdType, int quantity, int maxAlive, int side, float yPosition, float tMin =1f, float tMax =2f){
		int startingQuantity = currentWaveSpawnCount;
		for (int birdCount =0; birdCount<quantity; birdCount++){
			yield return StartCoroutine (WaitUntilSpawn (birdType, birdCount, startingQuantity));
			yield return StartCoroutine (WaitUntilAliveOnScreen (maxAlive));
			StartCoroutine (SpawnBirds (birdType, Constants.FixedSpawnHeight(side,yPosition),side));
			yield return StartCoroutine (WaitUntilTimeRange(tMin, tMax));
		}
		yield return null;
	}

	/// <summary> Produce some "quantity" of "birdType" birds on the left (-1) or right(+1) side at random height between yMin - yMax (+/-1)
	/// <para>Waits for its older brother to be born, checking the startingQuantity of older brethren</para>
	/// <para>Waits until at most "maxAlive" "birdToWatch" birds remain alive on screen, typically same as birdType</para>
	/// </summary>
	public IEnumerator MassProduce_FixedSide_RandomHeight_1Bird(int birdType, int quantity, int maxAlive, int side, int birdToWatch, float yMin, float yMax, float tMin =1f, float tMax =2f){
		int startingQuantity = currentWaveSpawnCount;
		for (int birdCount =0; birdCount<quantity; birdCount++){
			yield return StartCoroutine (WaitUntilSpawn (birdType, birdCount, startingQuantity));
			yield return StartCoroutine (WaitUntilAliveOnScreen (birdToWatch, maxAlive));
			StartCoroutine (SpawnBirds (birdType, Constants.RandomSpawnHeight(side, yMin, yMax),side));
			yield return StartCoroutine (WaitUntilTimeRange(tMin, tMax));
		}
		yield return null;
	}

	/// <summary> Produce some "quantity" of "birdType" birds on the left (-1) or right(+1) side at random height between yMin - yMax (+/-1)
	/// <para>Waits for its older brother to be born, checking the startingQuantity of older brethren</para>
	/// <para>Waits until at most "maxAlive" total birds remain alive on screen</para>
	/// </summary>
	public IEnumerator MassProduce_FixedSide_RandomHeight_AllBird(int birdType, int quantity, int maxAlive, int side, float yMin, float yMax, float tMin =1f, float tMax =2f){
		int startingQuantity = currentWaveSpawnCount;
		for (int birdCount =0; birdCount<quantity; birdCount++){
			yield return StartCoroutine (WaitUntilSpawn (birdType, birdCount, startingQuantity));
			yield return StartCoroutine (WaitUntilAliveOnScreen (maxAlive));
			StartCoroutine (SpawnBirds (birdType, Constants.RandomSpawnHeight(side, yMin, yMax),side));
			yield return StartCoroutine (WaitUntilTimeRange(tMin, tMax));
		}
		yield return null;
	}

	/// <summary> Produce some "quantity" of "birdType" birds on a random side at height "yPosition" (-1 <-> +1)
	/// <para>Waits for its older brother to be born, checking the startingQuantity of older brethren</para>
	/// <para>Waits until at most "maxAlive" "birdToWatch" birds remain alive on screen</para>
	/// </summary>
	public IEnumerator MassProduce_RandomSide_FixedHeight_1Bird(int birdType, int quantity, int maxAlive, int birdToWatch, float yPosition, float tMin =1f, float tMax =2f){
		int[] sideAndDirection;
		int startingQuantity = currentWaveSpawnCount;
		for (int birdCount =0; birdCount<quantity; birdCount++){
			sideAndDirection = RandomDuckSideAndDirection (RandomSide()==1);
			yield return StartCoroutine (WaitUntilSpawn (birdType, birdCount, startingQuantity));
			yield return StartCoroutine (WaitUntilAliveOnScreen (birdToWatch, maxAlive));
			StartCoroutine (SpawnBirds (birdType, Constants.FixedSpawnHeight(sideAndDirection[0], yPosition),sideAndDirection[1]));
			yield return StartCoroutine (WaitUntilTimeRange(tMin, tMax));
		}
		yield return null;
	}

	/// <summary> Produce some "quantity" of "birdType" birds on a random side at height "yPosition" (-1 <-> +1)
	/// <para>Waits for its older brother to be born, checking the startingQuantity of older brethren</para>
	/// <para>Waits until at most "maxAlive" total birds remain alive on screen</para>
	/// </summary>
	public IEnumerator MassProduce_RandomSide_FixedHeight_AllBird(int birdType, int quantity, int maxAlive, float yPosition, float tMin = 1f, float tMax = 2f){
		int[] sideAndDirection;
		int startingQuantity = currentWaveSpawnCount;
		for (int birdCount =0; birdCount<quantity; birdCount++){
			sideAndDirection = RandomDuckSideAndDirection (RandomSide()==1);
			yield return StartCoroutine (WaitUntilSpawn (birdType, birdCount, startingQuantity));
			yield return StartCoroutine (WaitUntilAliveOnScreen (maxAlive));
			StartCoroutine (SpawnBirds (birdType, Constants.FixedSpawnHeight(sideAndDirection[0], yPosition),sideAndDirection[1]));
			yield return StartCoroutine (WaitUntilTimeRange(tMin, tMax));
		}
		yield return null;
	}

	/// <summary> Produce some "quantity" of "birdType" birds on a random side at random height between yMin - yMax (+/-1)
	/// <para>Waits for its older brother to be born, checking the startingQuantity of older brethren</para>
	/// <para>Waits until at most "maxAlive" total birds remain alive on screen</para>
	/// </summary>
	public IEnumerator MassProduce_RandomSide_RandomHeight_1Bird(int birdType, int quantity, int maxAlive, int birdToWatch, float yMin, float yMax, float tMin = 1f, float tMax = 2f){
		int[] sideAndDirection;
		int startingQuantity = currentWaveSpawnCount;
		for (int birdCount =0; birdCount<quantity; birdCount++){
			sideAndDirection = RandomDuckSideAndDirection (RandomSide()==1);
			yield return StartCoroutine (WaitUntilSpawn (birdType, birdCount, startingQuantity));
			yield return StartCoroutine (WaitUntilAliveOnScreen (birdToWatch, maxAlive));
			StartCoroutine (SpawnBirds (birdType, Constants.RandomSpawnHeight(sideAndDirection[0], yMin, yMax),sideAndDirection[1]));
			yield return StartCoroutine (WaitUntilTimeRange(tMin, tMax));
		}
		yield return null;
	}

	/// <summary> Produce some "quantity" of "birdType" birds on a random side at random height between yMin - yMax (+/-1)
	/// <para>Waits for its older brother to be born, checking the startingQuantity of older brethren</para>
	/// <para>Waits until at most "maxAlive" total birds remain alive on screen</para>
	/// </summary>
	public IEnumerator MassProduce_RandomSide_RandomHeight_AllBirds(int birdType, int quantity, int maxAlive, float yMin, float yMax, float tMin =1f, float tMax =2f){
		int[] sideAndDirection;
		int startingQuantity = currentWaveSpawnCount;
		for (int birdCount =0; birdCount<quantity; birdCount++){
			sideAndDirection = RandomDuckSideAndDirection (RandomSide()==1);
			yield return StartCoroutine (WaitUntilSpawn (birdType, birdCount, startingQuantity));
			yield return StartCoroutine (WaitUntilAliveOnScreen (maxAlive));
			StartCoroutine (SpawnBirds (birdType, Constants.RandomSpawnHeight(sideAndDirection[0], yMin, yMax),sideAndDirection[1]));
			yield return StartCoroutine (WaitUntilTimeRange(tMin, tMax));
		}
		yield return null;
	}







//	/// <summary> Produce some "quantity" of "birdType" birds
//	/// <para>Waits until at most "maxAlive" total birds remain alive on screen</para>
//	/// <para>And waits for its older brother to be born, checking the startingQuantity of older brethren</para>
//	/// </summary>
//	public IEnumerator MassProduceRandomSide_WatchTheWorld(int birdType, int quantity, int maxAlive, float yMin, float yMax, float tMin =1f, float tMax =2f){
//		int[] sideAndDirection;
//		int startingQuantity = currentWaveAllSpawnCount [birdType];
//		for (int birdCount =0; birdCount<quantity; birdCount++){
//			sideAndDirection = RandomDuckSideAndDirection (RandomSide()==1);
//			yield return StartCoroutine (WaitUntilSpawn (birdType, birdCount, startingQuantity));
//			yield return StartCoroutine (WaitUntilAliveOnScreen (maxAlive));
//			StartCoroutine (SpawnBirds (birdType, Constants.FixedSpawnHeight(sideAndDirection[0],yMin,yMax),sideAndDirection[1]));
//			yield return StartCoroutine (WaitUntilTimeRange(tMin, tMax));
//		}
//		yield return null;
//	}
//
//	/// <summary> Produce some "quantity" of "birdType" birds
//	/// <para>Waits until at most "maxAlive" total birds remain alive on screen</para>
//	/// <para>And waits for its older brother to be born, checking the startingQuantity of older brethren</para>
//	/// </summary>
//	public IEnumerator MassProduceOneSide_WatchTheWorld(int birdType, int quantity, int maxAlive, int startSide, float yMin, float yMax, float tMin =1f, float tMax =2f){
//		int startingQuantity = currentWaveAllSpawnCount [birdType];
//		for (int birdCount =0; birdCount<quantity; birdCount++){
//			yield return StartCoroutine (WaitUntilSpawn (birdType, birdCount, startingQuantity));
//			yield return StartCoroutine (WaitUntilAliveOnScreen (maxAlive));
//			StartCoroutine (SpawnBirds (birdType, Constants.FixedSpawnHeight(startSide,yMin,yMax)));
//			yield return StartCoroutine (WaitUntilTimeRange(tMin, tMax));
//		}
//		yield return null;
//	}
//
//	/// <summary> Produce some "quantity" of "birdType" birds
//	/// <para>Waits until at most "maxAlive" total birds remain alive on screen</para>
//	/// <para>And waits for its older brother to be born, checking the startingQuantity of older brethren</para>
//	/// </summary>
//	public IEnumerator MassProduceRandomSide_WatchYourself(int birdType, int quantity, int maxAlive, float yMin, float yMax, float tMin =1f, float tMax =2f){
//		int[] sideAndDirection;
//		int startingQuantity = currentWaveAllSpawnCount [birdType];
//		for (int birdCount =0; birdCount<quantity; birdCount++){
//			sideAndDirection = RandomDuckSideAndDirection (RandomSide()==1);
//			yield return StartCoroutine (WaitUntilSpawn (birdType, birdCount, startingQuantity));
//			yield return StartCoroutine (WaitUntilAliveOnScreen (birdType,maxAlive));
//			StartCoroutine (SpawnBirds (birdType, Constants.FixedSpawnHeight(sideAndDirection[0],yMin,yMax),sideAndDirection[1]));
//			yield return StartCoroutine (WaitUntilTimeRange(tMin, tMax));
//		}
//		yield return null;
//	}
//
//	/// <summary> Produce some "quantity" of "birdType" birds
//	/// <para>Waits until at most "maxAlive" total birds remain alive on screen</para>
//	/// <para>And waits for its older brother to be born, checking the startingQuantity of older brethren</para>
//	/// </summary>
//	public IEnumerator MassProduceRandomSide_WatchPeers(int birdType, int quantity, int peerType, int maxAlive, float yMin, float yMax, float tMin =1f, float tMax =2f){
//		int[] sideAndDirection;
//		int startingQuantity = currentWaveAllSpawnCount [birdType];
//		for (int birdCount =0; birdCount<quantity; birdCount++){
//			sideAndDirection = RandomDuckSideAndDirection (RandomSide()==1);
//			yield return StartCoroutine (WaitUntilSpawn (birdType, birdCount, startingQuantity));
//			yield return StartCoroutine (WaitUntilAliveOnScreen (peerType,maxAlive));
//			StartCoroutine (SpawnBirds (birdType, Constants.FixedSpawnHeight(sideAndDirection[0],yMin,yMax),sideAndDirection[1]));
//			yield return StartCoroutine (WaitUntilTimeRange(tMin, tMax));
//		}
//		yield return null;
//	}


	public IEnumerator Birth(int birdType){
		currentWaveAllSpawnCount[birdType]++;
		allSpawnCount[birdType]++;
		currentWaveSpawnCount++;
		spawnCount++;
		
		currentWaveAllAliveCount [birdType]++;
		allAliveCount [birdType]++;
		currentWaveAliveCount++;
		aliveCount++;
		yield return null;
	}
	
	public IEnumerator Death(int birdType){
		currentWaveAllAliveCount [birdType]--;
		allAliveCount[birdType]--;
		currentWaveAliveCount--;
		aliveCount--;

		currentWaveAllKillCount [birdType]++;
		allKillCount[birdType]++;
		currentWaveKillCount++;
		killCount++;
	
		yield return null;
	}

	public IEnumerator AddPoints(int birdType, int thesePoints, float pointMultiplier){
		//special case for killing birds of point multipliers
		int totalFromMultiplier = 0;
		if (birdType == Constants.seagull || birdType == Constants.tentacles){ 
			foreach (GetHurt getHurtScript in FindObjectsOfType<GetHurt>()){
				totalFromMultiplier += Mathf.CeilToInt((getHurtScript.damagePointValue * getHurtScript.health + getHurtScript.killPointValue) * pointMultiplier);
			}
		}

		currentWaveAllPoints [birdType] += thesePoints + totalFromMultiplier;
		allPoints[birdType] += thesePoints + totalFromMultiplier;
		currentWavePoints += thesePoints + totalFromMultiplier;
		points += thesePoints + totalFromMultiplier;
		yield return null;
	}
}

