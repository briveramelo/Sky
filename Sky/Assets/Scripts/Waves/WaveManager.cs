using UnityEngine;
using System.Collections;
using GenericFunctions;

public class WaveManager : MonoBehaviour{

	//SINGLETON SCRIPT
	//CONTAINS STATIC VARIABLES AND FUNCTIONS
	//THAT WAVE SCRIPTS + SAVELOAD CAN REFERENCE FOR THEIR BIDDING

	public static WaveManager Instance;

	private int waveNumber;
	private float wavePauseTime;
	private float standardPauseTime;
	private float lowHeight;
	private float medHeight;
	private float highHeight;
	private float standardTimeMin;
	private float standardTimeMax;

	//spawn
	private int[] currentWaveAllSpawnCount;
	private int[] allSpawnCount;
	private int currentWaveSpawnCount;
	private int spawnCount;

	//alive
	private int[] currentWaveAllAliveCount;
	private int[] allAliveCount;
	private int currentWaveAliveCount;
	private int aliveCount;

	//killed
	private int[] currentWaveAllKillCount;
	private int[] allKillCount;
	private int currentWaveKillCount;
	private int killCount;

	//pooints!
	private int[] currentWaveAllPoints;
	private int[] allPoints;
	private int currentWavePoints;
	private int points;

	private int birdTypeCount;
	
	void Start(){
		Instance = this;
		birdTypeCount = Incubator.Instance.Birds.Length;
		allKillCount = new int[birdTypeCount];
		allAliveCount = new int[birdTypeCount];
		allSpawnCount = new int[birdTypeCount];
		allPoints = new int[birdTypeCount];
		currentWaveAllKillCount = new int[birdTypeCount];
		currentWaveAllAliveCount = new int[birdTypeCount];
		currentWaveAllSpawnCount = new int[birdTypeCount];
		currentWaveAllPoints = new int[birdTypeCount];

		wavePauseTime = 10f;
		standardPauseTime = 1f;
		lowHeight = -0.6f;
		medHeight = 0f;
		standardTimeMin = 1f;
		standardTimeMax = 2f;
		highHeight = -lowHeight;
	}

	/// <summary> Spawn Birds
	/// </summary>
	protected void SpawnBirds(int birdType, Vector3 spawnPoint,int duckMoveDir =0){ 
		int direction = spawnPoint.x<0 ? 1 : -1;
		Bird bird = (Instantiate (Incubator.Instance.Birds[birdType], spawnPoint, Quaternion.identity) as GameObject).GetComponent<Bird>();

		switch ((BirdType)birdType){
		case BirdType.Pigeon: 
			Pigeon pigeonScript = (Pigeon)bird;
			pigeonScript.SetVelocity(Vector2.right * direction);
			pigeonScript.transform.Face4ward(direction!=1);
			break;
		case BirdType.Duck: 
			ILeaderToDuck duckScript = (ILeaderToDuck)bird;
			duckScript.FormationNumber = duckMoveDir;
			duckScript.Scatter();
			break;
		case BirdType.DuckLeader: 
			DuckLeader duckLeaderScript = (DuckLeader)bird;
			bool goLeft = direction == -1 ? true : false;
			duckLeaderScript.SetFormation(goLeft);
			break;
		case BirdType.BirdOfParadise: 
			BirdOfParadise birdOfParadiseScript = (BirdOfParadise)bird;
			birdOfParadiseScript.SetVelocity(Vector2.right * direction);
			break;
		}
	}

	/// <summary> clamps the input between +/- 80% of the world height
	/// </summary>
	float SecureWorldY(float y){
		return Mathf.Clamp (y, -Constants.worldDimensions.y * .8f, Constants.worldDimensions.y * .8f);
	}

	/// <summary> determines which direction a duck should go 
	/// <para>based on its starting position</para>
	/// </summary>
	protected int[] RandomDuckSideAndDirection(bool top){
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

	protected int RandomDuckDirection(bool right){
		int formationNumber;
		if (right){
			formationNumber = Random.Range(0,2) == 0 ? 1 : 3;
		}
		else{
			formationNumber = Random.Range(0,2) == 0 ? 0 : 2;
		}
		return formationNumber;
	}

	protected int RandomSide(){
		return (int)Mathf.Sign(Random.insideUnitCircle.x);
	}

	protected void ResetWaveCounters(){
		currentWaveAllSpawnCount = new int[birdTypeCount];
		currentWaveAllAliveCount = new int[birdTypeCount];
		currentWaveAllKillCount = new int[birdTypeCount];

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
	protected IEnumerator WaitUntilAliveOnScreen(int birdType, int maxAlive){
		while (currentWaveAllAliveCount[birdType] > maxAlive){
			yield return null;
		}
		yield return null;
	}

	/// <summary> Wait until at most "maxAlive" birds remain alive on screen
	/// </summary>
	protected IEnumerator WaitUntilAliveOnScreen(int maxAlive){
		while (currentWaveAliveCount > maxAlive){
			yield return null;
		}
		yield return null;
	}

	/// <summary> Wait until at least "deathToll" birds have died in this wave since calling this.
	/// <para>Typically used internal to the "Mass Produce" Function Family to prevent resetting </para>
	/// <para>the starting quantity </para>
	/// </summary>
	protected IEnumerator WaitUntilDead(int birdType, int deathToll, int startingQuantity){
		while ((currentWaveAllKillCount[birdType] - startingQuantity) < deathToll){
			yield return null;
		}
		yield return null;
	}

	/// <summary> Wait until at least "deathToll" birds have died
	/// <para> since calling this function</para>
	/// </summary>
	protected IEnumerator WaitUntilDead(int birdType, int deathToll){
		int startingQuantity = currentWaveAllKillCount [birdType];
		while ((currentWaveAllKillCount[birdType] - startingQuantity) < deathToll){
			yield return null;
		}
		yield return null;
	}

	/// <summary> Wait until at least "deathToll" birds remain
	/// </summary>
	protected IEnumerator WaitUntilDead(int deathToll){
		while (currentWaveKillCount < deathToll){
			yield return null;
		}
		yield return null;
	}

	/// <summary> Wait until at least "minSpawned" "birdTypes" are born
	/// <para>Typically used in the "Mass Produce" Function Family to prevent resetting </para>
	/// <para>the starting quantity </para>
	/// </summary>
	protected IEnumerator WaitUntilSpawn(int birdType, int minSpawned, int startingQuantity){
		while ((currentWaveAllSpawnCount[birdType]-startingQuantity)<minSpawned){
			yield return null;
		}
		yield return null;
	}

	/// <summary> Wait until at least "minSpawned" "birdTypes" are born
	/// </summary>
	protected IEnumerator WaitUntilSpawn(int birdType, int minSpawned){
		int startingQuantity = currentWaveAllSpawnCount [birdType];
		while ((currentWaveAllSpawnCount[birdType]-startingQuantity)<minSpawned){
			yield return null;
		}
		yield return null;
	}

	/// <summary> Wait until at least "minSpawned" birds are born
	/// </summary>
	protected IEnumerator WaitUntilSpawn(int minSpawned){
		int startingQuantity = currentWaveSpawnCount;
		while ((currentWaveSpawnCount-startingQuantity)<minSpawned){
			yield return null;
		}
		yield return null;
	}

	/// <summary> Wait between 1-2 seconds
	/// </summary>
	protected IEnumerator WaitUntilTimeRange(){
		yield return new WaitForSeconds(Random.Range (1f, 2f));
	}

	/// <summary> Wait between minTime and maxTime seconds
	/// </summary>
	protected IEnumerator WaitUntilTimeRange(float minTime, float maxTime){
		yield return new WaitForSeconds(Random.Range(minTime,maxTime));
	}

	/// <summary> Produce some "quantity" of "birdType" birds on the left (-1) or right(+1) side at height "yPosition" (-1 <-> +1)
	/// <para>Waits for its older brother to be born, checking the startingQuantity of older brethren</para>
	/// <para>Waits until at most "maxAlive" "birdToWatch" birds remain alive on screen, typically same as birdType</para>
	/// </summary>
	protected IEnumerator MassProduce_FixedSide_FixedHeight_1Bird(int birdType, int quantity, int maxAlive, int birdToWatch, int side, float yPosition, float tMin = 1f, float tMax = 2f){
		int startingQuantity = currentWaveAllSpawnCount [birdType];
		for (int birdCount =0; birdCount<quantity; birdCount++){
			yield return StartCoroutine (WaitUntilSpawn (birdType, birdCount, startingQuantity));
			yield return StartCoroutine (WaitUntilAliveOnScreen (birdToWatch, maxAlive));
			SpawnBirds (birdType, Constants.FixedSpawnHeight(side,yPosition),side);
			yield return StartCoroutine (WaitUntilTimeRange(tMin, tMax));
		}
		yield return null;
	}

	/// <summary> Produce some "quantity" of "birdType" birds on the left (-1) or right(+1) side at height "yPosition" (-1 <-> +1)
	/// <para>Waits for its older brother to be born, checking the startingQuantity of older brethren</para>
	/// <para>Waits until at most "maxAlive" total birds remain alive on screen</para>
	/// </summary>
	protected IEnumerator MassProduce_FixedSide_FixedHeight_AllBirds(int birdType, int quantity, int maxAlive, int side, float yPosition, float tMin =1f, float tMax =2f){
		int startingQuantity = currentWaveSpawnCount;
		for (int birdCount =0; birdCount<quantity; birdCount++){
			yield return StartCoroutine (WaitUntilSpawn (birdType, birdCount, startingQuantity));
			yield return StartCoroutine (WaitUntilAliveOnScreen (maxAlive));
			SpawnBirds (birdType, Constants.FixedSpawnHeight(side,yPosition),side);
			yield return StartCoroutine (WaitUntilTimeRange(tMin, tMax));
		}
		yield return null;
	}

	/// <summary> Produce some "quantity" of "birdType" birds on the left (-1) or right(+1) side at random height between yMin - yMax (+/-1)
	/// <para>Waits for its older brother to be born, checking the startingQuantity of older brethren</para>
	/// <para>Waits until at most "maxAlive" "birdToWatch" birds remain alive on screen, typically same as birdType</para>
	/// </summary>
	protected IEnumerator MassProduce_FixedSide_RandomHeight_1Bird(int birdType, int quantity, int maxAlive, int side, int birdToWatch, float yMin, float yMax, float tMin =1f, float tMax =2f){
		int startingQuantity = currentWaveSpawnCount;
		for (int birdCount =0; birdCount<quantity; birdCount++){
			yield return StartCoroutine (WaitUntilSpawn (birdType, birdCount, startingQuantity));
			yield return StartCoroutine (WaitUntilAliveOnScreen (birdToWatch, maxAlive));
			SpawnBirds (birdType, Constants.RandomSpawnHeight(side, yMin, yMax),side);
			yield return StartCoroutine (WaitUntilTimeRange(tMin, tMax));
		}
		yield return null;
	}

	/// <summary> Produce some "quantity" of "birdType" birds on the left (-1) or right(+1) side at random height between yMin - yMax (+/-1)
	/// <para>Waits for its older brother to be born, checking the startingQuantity of older brethren</para>
	/// <para>Waits until at most "maxAlive" total birds remain alive on screen</para>
	/// </summary>
	protected IEnumerator MassProduce_FixedSide_RandomHeight_AllBird(int birdType, int quantity, int maxAlive, int side, float yMin, float yMax, float tMin =1f, float tMax =2f){
		int startingQuantity = currentWaveSpawnCount;
		for (int birdCount =0; birdCount<quantity; birdCount++){
			yield return StartCoroutine (WaitUntilSpawn (birdType, birdCount, startingQuantity));
			yield return StartCoroutine (WaitUntilAliveOnScreen (maxAlive));
			SpawnBirds (birdType, Constants.RandomSpawnHeight(side, yMin, yMax),side);
			yield return StartCoroutine (WaitUntilTimeRange(tMin, tMax));
		}
		yield return null;
	}

	/// <summary> Produce some "quantity" of "birdType" birds on a random side at height "yPosition" (-1 <-> +1)
	/// <para>Waits for its older brother to be born, checking the startingQuantity of older brethren</para>
	/// <para>Waits until at most "maxAlive" "birdToWatch" birds remain alive on screen</para>
	/// </summary>
	protected IEnumerator MassProduce_RandomSide_FixedHeight_1Bird(int birdType, int quantity, int maxAlive, int birdToWatch, float yPosition, float tMin =1f, float tMax =2f){
		int[] sideAndDirection;
		int startingQuantity = currentWaveSpawnCount;
		for (int birdCount =0; birdCount<quantity; birdCount++){
			sideAndDirection = RandomDuckSideAndDirection (RandomSide()==1);
			yield return StartCoroutine (WaitUntilSpawn (birdType, birdCount, startingQuantity));
			yield return StartCoroutine (WaitUntilAliveOnScreen (birdToWatch, maxAlive));
			SpawnBirds (birdType, Constants.FixedSpawnHeight(sideAndDirection[0], yPosition),sideAndDirection[1]);
			yield return StartCoroutine (WaitUntilTimeRange(tMin, tMax));
		}
		yield return null;
	}

	/// <summary> Produce some "quantity" of "birdType" birds on a random side at height "yPosition" (-1 <-> +1)
	/// <para>Waits for its older brother to be born, checking the startingQuantity of older brethren</para>
	/// <para>Waits until at most "maxAlive" total birds remain alive on screen</para>
	/// </summary>
	protected IEnumerator MassProduce_RandomSide_FixedHeight_AllBird(int birdType, int quantity, int maxAlive, float yPosition, float tMin = 1f, float tMax = 2f){
		int[] sideAndDirection;
		int startingQuantity = currentWaveSpawnCount;
		for (int birdCount =0; birdCount<quantity; birdCount++){
			sideAndDirection = RandomDuckSideAndDirection (RandomSide()==1);
			yield return StartCoroutine (WaitUntilSpawn (birdType, birdCount, startingQuantity));
			yield return StartCoroutine (WaitUntilAliveOnScreen (maxAlive));
			SpawnBirds (birdType, Constants.FixedSpawnHeight(sideAndDirection[0], yPosition),sideAndDirection[1]);
			yield return StartCoroutine (WaitUntilTimeRange(tMin, tMax));
		}
		yield return null;
	}

	/// <summary> Produce some "quantity" of "birdType" birds on a random side at random height between yMin - yMax (+/-1)
	/// <para>Waits for its older brother to be born, checking the startingQuantity of older brethren</para>
	/// <para>Waits until at most "maxAlive" total birds remain alive on screen</para>
	/// </summary>
	protected IEnumerator MassProduce_RandomSide_RandomHeight_1Bird(int birdType, int quantity, int maxAlive, int birdToWatch, float yMin, float yMax, float tMin = 1f, float tMax = 2f){
		int[] sideAndDirection;
		int startingQuantity = currentWaveSpawnCount;
		for (int birdCount =0; birdCount<quantity; birdCount++){
			sideAndDirection = RandomDuckSideAndDirection (RandomSide()==1);
			yield return StartCoroutine (WaitUntilSpawn (birdType, birdCount, startingQuantity));
			yield return StartCoroutine (WaitUntilAliveOnScreen (birdToWatch, maxAlive));
			SpawnBirds (birdType, Constants.RandomSpawnHeight(sideAndDirection[0], yMin, yMax),sideAndDirection[1]);
			yield return StartCoroutine (WaitUntilTimeRange(tMin, tMax));
		}
		yield return null;
	}

	/// <summary> Produce some "quantity" of "birdType" birds on a random side at random height between yMin - yMax (+/-1)
	/// <para>Waits for its older brother to be born, checking the startingQuantity of older brethren</para>
	/// <para>Waits until at most "maxAlive" total birds remain alive on screen</para>
	/// </summary>
	protected IEnumerator MassProduce_RandomSide_RandomHeight_AllBirds(int birdType, int quantity, int maxAlive, float yMin, float yMax, float tMin =1f, float tMax =2f){
		int[] sideAndDirection;
		int startingQuantity = currentWaveSpawnCount;
		for (int birdCount =0; birdCount<quantity; birdCount++){
			sideAndDirection = RandomDuckSideAndDirection (RandomSide()==1);
			yield return StartCoroutine (WaitUntilSpawn (birdType, birdCount, startingQuantity));
			yield return StartCoroutine (WaitUntilAliveOnScreen (maxAlive));
			SpawnBirds (birdType, Constants.RandomSpawnHeight(sideAndDirection[0], yMin, yMax),sideAndDirection[1]);
			yield return StartCoroutine (WaitUntilTimeRange(tMin, tMax));
		}
		yield return null;
	}

	public void Birth(int birdType){
		currentWaveAllSpawnCount[birdType]++;
		allSpawnCount[birdType]++;
		currentWaveSpawnCount++;
		spawnCount++;
		
		currentWaveAllAliveCount [birdType]++;
		allAliveCount [birdType]++;
		currentWaveAliveCount++;
		aliveCount++;
	}

	public void Death(int birdType){
		currentWaveAllAliveCount [birdType]--;
		allAliveCount[birdType]--;
		currentWaveAliveCount--;
		aliveCount--;
	}

	public void Kill(int birdType){
		currentWaveAllKillCount [birdType]++;
		allKillCount[birdType]++;
		currentWaveKillCount++;
		killCount++;
	}

	public void AddPoints(int birdType, int thesePoints, float pointMultiplier){
		//special case for killing birds of point multipliers
		int totalFromMultiplier = 0;
		if (birdType == (int)BirdType.Seagull || birdType == (int)BirdType.Tentacles){ 
			foreach (Bird bird in FindObjectsOfType<Bird>()){
				totalFromMultiplier += Mathf.CeilToInt(bird.MyBirdStats.RemainingPoints * pointMultiplier);
			}
		}
		currentWaveAllPoints [birdType] += thesePoints + totalFromMultiplier;
		allPoints[birdType] += thesePoints + totalFromMultiplier;
		currentWavePoints += thesePoints + totalFromMultiplier;
		points += thesePoints + totalFromMultiplier;
	}
}