using UnityEngine;
using System.Collections;
using GenericFunctions;

public class WaveManager : MonoBehaviour {

	public Transform balloonTransform;
	public int waveNumber;
	public float wavePauseTime;

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
	public int killCount;
	public int currentWaveKillCount;

	
	void Awake(){
		allKillCount = new int[Constants.birdNamePrefabs.Length];
		allAliveCount = new int[Constants.birdNamePrefabs.Length];
		allSpawnCount = new int[Constants.birdNamePrefabs.Length];
		currentWaveAllKillCount = new int[Constants.birdNamePrefabs.Length];
		currentWaveAllAliveCount = new int[Constants.birdNamePrefabs.Length];
		currentWaveAllSpawnCount = new int[Constants.birdNamePrefabs.Length];

		balloonTransform = GameObject.Find ("BalloonSpot").transform;
		wavePauseTime = 7f;
		StartCoroutine (Wave1 ());
	}

	//PIGEONS
	public IEnumerator Wave1(){
		waveNumber = 1;
		ResetWaveCounters ();

		//LOW left-right
		yield return new WaitForSeconds(wavePauseTime);
		yield return StartCoroutine (SpawnBirds (Constants.pigeon, Constants.RandomSpawnPoint(-1,-1,-0.75f,0f)));
		yield return StartCoroutine (WaitUntilTimeRange ());
		yield return StartCoroutine (SpawnBirds (Constants.pigeon, Constants.RandomSpawnPoint(-1,-1,-0.75f,0f)));

		//LOW right-left
		yield return StartCoroutine (WaitUntilTimeRange ());
		yield return StartCoroutine (SpawnBirds (Constants.pigeon, Constants.RandomSpawnPoint(1,1,-0.75f,0f)));
		yield return StartCoroutine (WaitUntilTimeRange ());
		yield return StartCoroutine (SpawnBirds (Constants.pigeon, Constants.RandomSpawnPoint(1,1,-0.75f,0f)));

		//PAUSE until life tally for pigeons drops to 1
		//HIGH left-right
		yield return StartCoroutine (WaitUntilAliveOnScreen (Constants.pigeon,1));
		yield return StartCoroutine (WaitUntilTimeRange ());
		yield return StartCoroutine (SpawnBirds (Constants.pigeon, Constants.RandomSpawnPoint(-1,-1,0.2f,0.7f)));
		yield return StartCoroutine (WaitUntilTimeRange ());
		yield return StartCoroutine (SpawnBirds (Constants.pigeon, Constants.RandomSpawnPoint(-1,-1,0.2f,0.7f)));

		//HIGH right-left
		yield return StartCoroutine (WaitUntilTimeRange ());
		yield return StartCoroutine (SpawnBirds (Constants.pigeon, Constants.RandomSpawnPoint(1,1,0.2f,0.7f)));
		yield return StartCoroutine (WaitUntilTimeRange ());
		yield return StartCoroutine (SpawnBirds (Constants.pigeon, Constants.RandomSpawnPoint(1,1,0.2f,0.7f)));

		//FLOOD from right-left
		yield return StartCoroutine (WaitUntilAliveOnScreen (Constants.pigeon,1));
		yield return new WaitForSeconds(2f);
		yield return StartCoroutine (SpawnBirds (Constants.pigeon, Constants.RandomSpawnPoint(1,1,SecureWorldY(balloonTransform.position.y),SecureWorldY(balloonTransform.position.y))));
		yield return StartCoroutine (MassProduceOneSide_WatchTheWorld (Constants.pigeon,10,11,-.7f,0.7f,1,0.4f,0.7f));

		//REWARD
		//PAUSE until life tally total drops to 0
		yield return StartCoroutine (WaitUntilAliveOnScreen (0));
		yield return new WaitForSeconds(2f);
		yield return StartCoroutine (SpawnBirds (Constants.birdOfParadise, Constants.RandomSpawnPoint(1,1,-.75f,-.25f)));
		yield return StartCoroutine (WaitUntilAliveOnScreen (0));
		StartCoroutine (Wave2 ());
	}

	//DUCKS (+pigeons)
	public IEnumerator Wave2(){
		waveNumber = 2;
		ResetWaveCounters ();
		//Right - Left
		yield return new WaitForSeconds(wavePauseTime);
		int height = Random.Range (0, 2) == 0 ? -1 : 1;
		yield return StartCoroutine (SpawnBirds (Constants.duck, Constants.RandomSpawnPoint(1,1,height,height),RandomDuckDirection(true)));
		yield return StartCoroutine (WaitUntilTimeRange ());
		yield return StartCoroutine (SpawnBirds (Constants.duck, Constants.RandomSpawnPoint(1,1,-height,-height),RandomDuckDirection(true)));

		//PAUSE until there are 1 more ducks alive
		//Produce 3 ducks
		//Left - Right
		yield return StartCoroutine (WaitUntilAliveOnScreen (Constants.duck,1));
		yield return StartCoroutine (MassProduceOneSide_WatchTheWorld (Constants.duck,3,4,-1,-1,-1));

		//PAUSE until life tally for ducks drops to 1
		yield return StartCoroutine (WaitUntilAliveOnScreen (Constants.duck,1));

		//Only spawn the next pigeon after the previous has been spawned for (?)time
		yield return StartCoroutine (WaitUntilTimeRange ());
		StartCoroutine (MassProduceRandomSide_WatchTheWorld (Constants.pigeon,6,5,-.7f,.7f));
		yield return StartCoroutine (MassProduceRandomSide_WatchTheWorld (Constants.duck,6,5,-1,1));

		//DUCK LEADER
		yield return StartCoroutine (WaitUntilAliveOnScreen (0));
		yield return new WaitForSeconds (2f);
		yield return StartCoroutine (SpawnBirds (Constants.duckLeader, Constants.RandomSpawnPoint(-1,-1,-.2f,.2f)));

		//REWARD
		//PAUSE until life tally total drops to 0
		yield return StartCoroutine (WaitUntilAliveOnScreen (0));
		yield return new WaitForSeconds (2f);
		yield return StartCoroutine (SpawnBirds (Constants.birdOfParadise, Constants.RandomSpawnPoint(1,1,-.75f,-.25f)));
		yield return StartCoroutine (WaitUntilAliveOnScreen (0));
		StartCoroutine (Wave3 ());
	}

	//BATS
	public IEnumerator Wave3(){
		waveNumber = 3;
		ResetWaveCounters ();

		yield return new WaitForSeconds(wavePauseTime);
		int side = RandomSide ();
		yield return StartCoroutine (SpawnBirds (Constants.bat, Constants.RandomSpawnPoint(side,side,-1,1)));
		yield return StartCoroutine (WaitUntilTimeRange ());
		yield return StartCoroutine (SpawnBirds (Constants.bat, Constants.RandomSpawnPoint(side,side,-1,1)));

		yield return StartCoroutine (WaitUntilAliveOnScreen (0));
		StartCoroutine (MassProduceRandomSide_WatchYourself (Constants.bat,20,10,-1,1,0.5f,1f));
		StartCoroutine (MassProduceRandomSide_WatchYourself (Constants.pigeon,5,1,-1,1,2f,4f));
		StartCoroutine (MassProduceRandomSide_WatchYourself (Constants.duck,5,1,-1,1,4f,6f));

		yield return StartCoroutine (WaitUntilDead(currentWaveSpawnCount-5));
		StartCoroutine (SpawnBirds (Constants.duckLeader,Constants.RandomSpawnPoint(-1,-1,-0.5f,0.5f)));

		yield return StartCoroutine (WaitUntilAliveOnScreen (Constants.duck,1));
		StartCoroutine (SpawnBirds (Constants.duckLeader,Constants.RandomSpawnPoint(-1,-1,-0.5f,0.5f)));

		yield return StartCoroutine (WaitUntilAliveOnScreen (Constants.duck,2));
		StartCoroutine (SpawnBirds (Constants.duckLeader,Constants.RandomSpawnPoint(-1,-1,-0.5f,0.5f)));

		//REWARD
		//PAUSE until life tally total drops to 0
		yield return StartCoroutine (WaitUntilAliveOnScreen (0));
		yield return new WaitForSeconds (2f);
		yield return StartCoroutine (SpawnBirds (Constants.birdOfParadise, Constants.RandomSpawnPoint(1,1,-.75f,-.25f)));
		yield return StartCoroutine (WaitUntilAliveOnScreen (0));
		//StartCoroutine (Wave4 ());
	}

	public IEnumerator Wave4(){
		waveNumber = 4;
		ResetWaveCounters ();
		yield return null;
		StartCoroutine (Wave5 ());
	}

	public IEnumerator Wave5(){
		waveNumber = 5;
		ResetWaveCounters ();
		yield return null;
		StartCoroutine (Wave6 ());
	}

	public IEnumerator Wave6(){
		waveNumber = 6;
		ResetWaveCounters ();
		yield return null;
		StartCoroutine (Wave7 ());
	}

	public IEnumerator Wave7(){
		waveNumber = 7;
		ResetWaveCounters ();
		yield return null;
		StartCoroutine (Wave8 ());
	}

	public IEnumerator Wave8(){
		waveNumber = 8;
		ResetWaveCounters ();
		yield return null;
		StartCoroutine (Wave9 ());
	}

	public IEnumerator Wave9(){
		waveNumber = 9;
		yield return null;
		StartCoroutine (Wave10 ());
	}

	public IEnumerator Wave10(){
		waveNumber = 10;
		yield return null;
		//StartCoroutine (SaveLoadData.dataStorage.PromptSave ());
	}

	/// <summary> Spawn Birds
	/// </summary>
	public IEnumerator SpawnBirds(int birdType, Vector3 spawnPoint,int duckMoveDir =0){ 
		int direction = spawnPoint.x<0 ? 1 : -1;
		GameObject bird = Instantiate (Resources.Load (Constants.birdNamePrefabs [birdType]), spawnPoint, Quaternion.identity) as GameObject;

		if (birdType==Constants.pigeon){
			Pigeon pigeonScript = bird.GetComponent<Pigeon> ();
			pigeonScript.rigidbod.velocity = Vector2.right * direction * pigeonScript.moveSpeed;
			pigeonScript.transform.Face4ward(direction!=1);
		}
		else if (birdType==Constants.duck){
			Duck duckScript = bird.GetComponent<Duck> ();
			duckScript.formationNumber = duckMoveDir;
		}
		else if (birdType == Constants.duckLeader){
			DuckLeader duckLeaderScript = bird.GetComponent<DuckLeader> ();
			duckLeaderScript.rigbod.velocity = duckLeaderScript.moveSpeed * direction * Vector2.right;
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
		yield return new WaitForSeconds(Random.Range (1.5f,3f));
	}

	/// <summary> Wait between minTime and maxTime seconds
	/// </summary>
	public IEnumerator WaitUntilTimeRange(float minTime, float maxTime){
		yield return new WaitForSeconds(Random.Range(minTime,maxTime));
	}

	/// <summary> Produce some "quantity" of "birdType" birds
	/// <para>Waits until at most "maxAlive" total birds remain alive on screen</para>
	/// <para>And waits for its older brother to be born, checking the startingQuantity of older brethren</para>
	/// </summary>
	public IEnumerator MassProduceRandomSide_WatchTheWorld(int birdType, int quantity, int maxAlive, float yMin, float yMax, float tMin =1.5f, float tMax =3f){
		int[] sideAndDirection;
		int startingQuantity = currentWaveAllSpawnCount [birdType];
		for (int birdCount =0; birdCount<quantity; birdCount++){
			sideAndDirection = RandomDuckSideAndDirection (RandomSide()==1);
			yield return StartCoroutine (WaitUntilSpawn (birdType, birdCount, startingQuantity));
			yield return StartCoroutine (WaitUntilAliveOnScreen (maxAlive));
			StartCoroutine (SpawnBirds (birdType, Constants.RandomSpawnPoint(sideAndDirection[0],sideAndDirection[0],yMin,yMax),sideAndDirection[1]));
			yield return StartCoroutine (WaitUntilTimeRange(tMin, tMax));
		}
		yield return null;
	}

	/// <summary> Produce some "quantity" of "birdType" birds
	/// <para>Waits until at most "maxAlive" total birds remain alive on screen</para>
	/// <para>And waits for its older brother to be born, checking the startingQuantity of older brethren</para>
	/// </summary>
	public IEnumerator MassProduceOneSide_WatchTheWorld(int birdType, int quantity, int maxAlive, float yMin, float yMax, int startSide, float tMin =1.5f, float tMax =3f){
		int startingQuantity = currentWaveAllSpawnCount [birdType];
		for (int birdCount =0; birdCount<quantity; birdCount++){
			yield return StartCoroutine (WaitUntilSpawn (birdType, birdCount, startingQuantity));
			yield return StartCoroutine (WaitUntilAliveOnScreen (maxAlive));
			StartCoroutine (SpawnBirds (birdType, Constants.RandomSpawnPoint(startSide,startSide,yMin,yMax)));
			yield return StartCoroutine (WaitUntilTimeRange(tMin, tMax));
		}
		yield return null;
	}

	/// <summary> Produce some "quantity" of "birdType" birds
	/// <para>Waits until at most "maxAlive" total birds remain alive on screen</para>
	/// <para>And waits for its older brother to be born, checking the startingQuantity of older brethren</para>
	/// </summary>
	public IEnumerator MassProduceRandomSide_WatchYourself(int birdType, int quantity, int maxAlive, float yMin, float yMax, float tMin =1.5f, float tMax =3f){
		int[] sideAndDirection;
		int startingQuantity = currentWaveAllSpawnCount [birdType];
		for (int birdCount =0; birdCount<quantity; birdCount++){
			sideAndDirection = RandomDuckSideAndDirection (RandomSide()==1);
			yield return StartCoroutine (WaitUntilSpawn (birdType, birdCount, startingQuantity));
			yield return StartCoroutine (WaitUntilAliveOnScreen (birdType,maxAlive));
			StartCoroutine (SpawnBirds (birdType, Constants.RandomSpawnPoint(sideAndDirection[0],sideAndDirection[0],yMin,yMax),sideAndDirection[1]));
			yield return StartCoroutine (WaitUntilTimeRange(tMin, tMax));
		}
		yield return null;
	}

	/// <summary> Produce some "quantity" of "birdType" birds
	/// <para>Waits until at most "maxAlive" total birds remain alive on screen</para>
	/// <para>And waits for its older brother to be born, checking the startingQuantity of older brethren</para>
	/// </summary>
	public IEnumerator MassProduceRandomSide_WatchPeers(int birdType, int quantity, int peerType, int maxAlive, float yMin, float yMax, float tMin =1.5f, float tMax =3f){
		int[] sideAndDirection;
		int startingQuantity = currentWaveAllSpawnCount [birdType];
		for (int birdCount =0; birdCount<quantity; birdCount++){
			sideAndDirection = RandomDuckSideAndDirection (RandomSide()==1);
			yield return StartCoroutine (WaitUntilSpawn (birdType, birdCount, startingQuantity));
			yield return StartCoroutine (WaitUntilAliveOnScreen (peerType,maxAlive));
			StartCoroutine (SpawnBirds (birdType, Constants.RandomSpawnPoint(sideAndDirection[0],sideAndDirection[0],yMin,yMax),sideAndDirection[1]));
			yield return StartCoroutine (WaitUntilTimeRange(tMin, tMax));
		}
		yield return null;
	}

}

