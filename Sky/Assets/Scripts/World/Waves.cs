using UnityEngine;
using System.Collections;

public class Waves : MonoBehaviour {

	public GameObject bird;

	public Vector3[][] spawnPoints;

	public string[] birdNames;
	public string prefix;

	public int[][] waveBirdsToSpawn; //tells the total number of birds alive in wave 1 (and so on for each)
	public int[][] waveBirdsStillAlive;

	public int currentWave;
	public int birdType;
	public int numberOfBirdsToSpawn;
	public int currentWaveBirdsSpawned;
	public int numberOfBirdsToSpawnOfThisType;
	public int numberOfBirdsStillAlive;

	public bool spawnBirds;

	// Use this for initialization
	void Awake () {      					//pigeon, duck2, albatross,  
		prefix = "Prefabs/Birds/"; //		  0       1          2         3         		4          5       6
		birdNames =  new string[]{		"Pigeon", "Duck2","Albatross", "BabyCrow",       "Crow"    ,"Eagle", "BirdOfParadise"};
		waveBirdsToSpawn = new int[][]{
							 new int[]{ 	  3    ,  0  ,  	 0   ,     0     , 		    0      ,   0   ,   0  				},  //0
							 new int[]{ 	  0    ,  3  ,  	 0   ,     0     ,  	    0      ,   0   ,   0 				 },  //1
							 new int[]{ 	  2    ,  2  , 		 1   ,     0     ,    	    0      ,   0   ,   0 				 },  //2
							 new int[]{ 	  0    ,  0  ,  	 0   ,     0     ,  	    0      ,   0   ,   1 				 },  //3
							 new int[]{ 	  2    ,  3  ,  	 0   ,     1     , 		    0      ,   0   ,   0 				 },  //4
							 new int[]{ 	  0    ,  0  ,  	 0   ,     0     , 		    0      ,   0   ,   1 				 },  //5
							 new int[]{ 	  3    ,  2  ,  	 0   ,     0     , 		    0      ,   1   ,   0 				 },  //6
							 new int[]{ 	  0    ,  0  ,  	 0   ,     0     , 		    0      ,   0   ,   0 				 },  //7
							 new int[]{ 	  0    ,  0  ,  	 0   ,     0     , 		    0      ,   0   ,   0 				 },  //8
							 new int[]{ 	  0    ,  0  ,  	 0   ,     0     , 		    0      ,   0   ,   0 				 },  //9
							 new int[]{ 	  0    ,  0  ,  	 0   ,     0     , 		    0      ,   0   ,   0 				 },  //10
							 new int[]{  	  0    ,  0  ,  	 0   ,     0     ,  	    0      ,   0   ,   0 				 }}; //11
		waveBirdsStillAlive = new int[][]{
			new int[]{ 	  3    ,  0  ,  	 0   ,     0     , 		    0      ,   0   ,   0  				},  //0
			new int[]{ 	  0    ,  3  ,  	 0   ,     0     ,  	    0      ,   0   ,   0 				 },  //1
			new int[]{ 	  2    ,  2  , 		 1   ,     0     ,    	    0      ,   0   ,   0 				 },  //2
			new int[]{ 	  0    ,  0  ,  	 0   ,     0     ,  	    0      ,   0   ,   1 				 },  //3
			new int[]{ 	  2    ,  3  ,  	 0   ,     1     , 		    0      ,   0   ,   0 				 },  //4
			new int[]{ 	  0    ,  0  ,  	 0   ,     0     , 		    0      ,   0   ,   1 				 },  //5
			new int[]{ 	  3    ,  2  ,  	 0   ,     0     , 		    0      ,   1   ,   0 				 },  //6
			new int[]{ 	  0    ,  0  ,  	 0   ,     0     , 		    0      ,   0   ,   0 				 },  //7
			new int[]{ 	  0    ,  0  ,  	 0   ,     0     , 		    0      ,   0   ,   0 				 },  //8
			new int[]{ 	  0    ,  0  ,  	 0   ,     0     , 		    0      ,   0   ,   0 				 },  //9
			new int[]{ 	  0    ,  0  ,  	 0   ,     0     , 		    0      ,   0   ,   0 				 },  //10
			new int[]{ 	  0    ,  0  ,  	 0   ,     0     ,  	    0      ,   0   ,   0 				 }}; //11
		int i = 0;
		foreach (string birdName in birdNames){
			birdNames[i] = prefix+birdName;
			i++;
		}

		spawnPoints = new Vector3[][]{
			new Vector3[]{ new Vector3 (-8, 0, 0),new Vector3 (-8, -2, 0),new Vector3 (-8, 2, 0), new Vector3 (-8, 0, 0),new Vector3 (-8, -2, 0),new Vector3 (-8, 2, 0) }, //0
			new Vector3[]{ new Vector3 (8, -2, 0), new Vector3 (8, -3, 0), new Vector3 (8, 4, 0), new Vector3 (-8, 0, 0),new Vector3 (-8, -2, 0),new Vector3 (-8, 2, 0) },  //1
			new Vector3[]{ new Vector3 (-8, 2, 0),new Vector3 (-8, 4, 0), new Vector3 (-8, -1, 0),new Vector3 (-8, 0, 0),new Vector3 (-8, -2, 0),new Vector3 (-8, 2, 0) },  //2
			new Vector3[]{ new Vector3 (-8, 2, 0),new Vector3 (-8, 4, 0), new Vector3 (-8, -1, 0),new Vector3 (-8, 2, 0),new Vector3 (-8, 4, 0), new Vector3 (-8, -1, 0) }, //3
			new Vector3[]{ new Vector3 (-8, 2, 0),new Vector3 (-8, 4, 0), new Vector3 (-8, -1, 0),new Vector3 (-8, 2, 0),new Vector3 (-8, 4, 0), new Vector3 (-8, -1, 0) }, //4
			new Vector3[]{ new Vector3 (-8, 2, 0),new Vector3 (-8, 4, 0), new Vector3 (-8, -1, 0),new Vector3 (-8, 2, 0),new Vector3 (-8, 4, 0), new Vector3 (-8, -1, 0) }  //5
		};
		currentWaveBirdsSpawned = 0;
		birdType = 0;
		currentWave = 0;
		StartCoroutine (PrepareNextWave ());
	}

	public IEnumerator SpawnBirdsInThisWave(){
		StartCoroutine (CheckBirds ());
		if (numberOfBirdsToSpawn>0){ //spawn remaining birds
			if (numberOfBirdsToSpawnOfThisType>0){ //if more pigeons to spawn, spawn them!
				bird = Instantiate (Resources.Load (birdNames [birdType]),spawnPoints[currentWave-1][currentWaveBirdsSpawned], Quaternion.identity) as GameObject;
				currentWaveBirdsSpawned++;
				waveBirdsToSpawn[currentWave-1][birdType]--;
				yield return new WaitForSeconds (4f);
			}
			else if (birdType<6){ //if no more pigeons to spawn, spawn ducks!
				birdType++;
			}
			StartCoroutine(SpawnBirdsInThisWave());
		}
		yield return null;
	}

	public IEnumerator PrepareNextWave(){
		yield return new WaitForSeconds (3f);
		currentWave++;
		currentWaveBirdsSpawned = 0;
		birdType=0;
		numberOfBirdsToSpawn = 0;
		int k = 0;
		foreach (int j in waveBirdsToSpawn[currentWave-1]){
			numberOfBirdsToSpawn += j;
			k++;
		}

		numberOfBirdsToSpawnOfThisType = waveBirdsToSpawn[currentWave-1] [0];
		StartCoroutine (SpawnBirdsInThisWave ());
		yield return null;
	}

	public IEnumerator CheckBirds(){
		numberOfBirdsToSpawn = 0;
		foreach (int birdSpawnCount in waveBirdsToSpawn[currentWave-1]){
			numberOfBirdsToSpawn += birdSpawnCount;
		}

		numberOfBirdsStillAlive = 0;
		foreach (int birdAliveCount in waveBirdsStillAlive[currentWave-1]){
			numberOfBirdsStillAlive += birdAliveCount;
		}

		numberOfBirdsToSpawnOfThisType = waveBirdsToSpawn[currentWave-1] [birdType];
		if (numberOfBirdsStillAlive<1){
			StartCoroutine(PrepareNextWave());
		}
		yield return null;
	}
}
