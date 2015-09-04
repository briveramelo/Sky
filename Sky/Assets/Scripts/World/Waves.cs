using UnityEngine;
using System.Collections;

public class Waves : MonoBehaviour {

	public bool spawnBirds;

	public int[][] waveBirds; //tells the total number of birds alive in wave 1 (and so on for each)
	public int currentWave;

	public int birdType;
	public int[] currentWaveBirdsToSpawn;
	public int[] currentWaveBirdsStillAlive;
	public int numberOfBirdsToSpawn;
	public int numberOfBirdsToSpawnOfThisType;
	public int numberOfBirdsStillAlive;
	public int numberOfBirdsStillAliveOfThisType;

	public string prefix;
	public string[] birdNames;

	public GameObject bird;

	// Use this for initialization
	void Awake () {      //pigeon, duck1, duck2, BlueParrot, GreenParrot, Stork1, Stork2 
		prefix = "Prefabs/Birds/"; //		  0       1          2         3         		4          5       6
		birdNames =  new string[]{		"Pigeon", "Duck1",   "Duck2" , "BlueParrot", "GreenParrot","Stork1", "Stork2"};
		waveBirds = new int[][]{new int[]{ 	  3    ,  0  ,  	 0   ,     0     , 		    0      ,   0   ,   0  },  //0
							 new int[]{ 	  0    ,  3  ,  	 0   ,     0     ,  	    0      ,   0   ,   0  },  //1
							 new int[]{ 	  0    ,  0  , 		 3   ,     0     ,    	    0      ,   0   ,   0  },  //2
							 new int[]{ 	  2    ,  2  ,  	 2   ,     0     ,  	    0      ,   0   ,   0  },  //3
							 new int[]{ 	  0    ,  0  ,  	 0   ,     3     , 		    0      ,   0   ,   0  },  //4
							 new int[]{ 	  0    ,  0  ,  	 0   ,     0     , 		    3      ,   0   ,   0  },  //5
							 new int[]{ 	  0    ,  0  ,  	 0   ,     0     , 		    0      ,   0   ,   0  },  //6
							 new int[]{ 	  0    ,  0  ,  	 0   ,     0     , 		    0      ,   0   ,   0  },  //7
							 new int[]{ 	  0    ,  0  ,  	 0   ,     0     , 		    0      ,   0   ,   0  },  //8
							 new int[]{ 	  0    ,  0  ,  	 0   ,     0     , 		    0      ,   0   ,   0  },  //9
							 new int[]{ 	  0    ,  0  ,  	 0   ,     0     , 		    0      ,   0   ,   0  },  //10
							 new int[]{  	  0    ,  0  ,  	 0   ,     0     ,  	    0      ,   0   ,   0  }}; //11
		int i = 0;
		foreach (string birdName in birdNames){
			birdNames[i] = prefix+birdName;
			i++;
		}
		currentWaveBirdsToSpawn = new int[waveBirds [0].Length];
		currentWaveBirdsStillAlive = new int[waveBirds [0].Length];
		birdType = 0;
		currentWave = 0;
		StartCoroutine (PrepareNextWave ());
	}

	/*void Update(){
		if (spawnBirds){
			spawnBirds = false;
			StartCoroutine(SpawnBirdsInThisWave());
		}
	}*/

	public IEnumerator SpawnBirdsInThisWave(){
		StartCoroutine (CheckBirds ());
		if (numberOfBirdsToSpawn>0){ //spawn remaining birds
			if (numberOfBirdsToSpawnOfThisType>0){ //if more pigeons to spawn, spawn them!
				bird = Instantiate (Resources.Load (birdNames [birdType]), Vector3.zero, Quaternion.identity) as GameObject;
				currentWaveBirdsToSpawn [birdType]--;
				yield return new WaitForSeconds (2f);
			}
			else if (birdType<7){ //if no more pigeons to spawn, spawn ducks!
				birdType++;
			}
			StartCoroutine(SpawnBirdsInThisWave());
		}
		yield return null;
	}

	public IEnumerator PrepareNextWave(){
		yield return new WaitForSeconds (3f);
		currentWave++;
		birdType=0;
		int k = 0;
		foreach (int j in waveBirds[currentWave]){
			currentWaveBirdsToSpawn[k] = j;
			k++;
		}
		k = 0;
		foreach (int j in currentWaveBirdsToSpawn){
			currentWaveBirdsStillAlive[k] = currentWaveBirdsToSpawn[k];
			k++;
		}
		StartCoroutine (SpawnBirdsInThisWave ());
		yield return null;
	}

	public IEnumerator CheckBirds(){
		numberOfBirdsToSpawn = 0;
		foreach (int birdSpawnCount in currentWaveBirdsToSpawn){
			numberOfBirdsToSpawn += birdSpawnCount;
		}

		numberOfBirdsStillAlive = 0;
		foreach (int birdAliveCount in currentWaveBirdsStillAlive){
			numberOfBirdsStillAlive += birdAliveCount;
		}

		numberOfBirdsToSpawnOfThisType = currentWaveBirdsToSpawn [birdType];
		numberOfBirdsStillAliveOfThisType = currentWaveBirdsStillAlive [birdType];
		if (numberOfBirdsStillAlive<1){
			StartCoroutine(PrepareNextWave());
		}
		yield return null;
	}
}
