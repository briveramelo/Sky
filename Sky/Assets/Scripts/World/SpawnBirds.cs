using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class SpawnBirds : MonoBehaviour {

	public GameObject bird;

	public string[] birdNames; //file location so you can load the right bird

	[Range(0,9)]
	public int birdType;

	public bool spawnBirds;

	// Use this for initialization
	void Awake () {
									 //		0       1          2     	    3        	 4    		  5  	   		6			7	    8		9			10 
		birdNames =  new string[]{		"Pigeon", "Duck2", "DuckLeader", "Albatross", "BabyCrow","Murder", "TentacleSensor", "Pelican", "Bat", "Eagle", "BirdOfParadise"};
		int i = 0;
		foreach (string birdName in birdNames){
			birdNames[i] = "Prefabs/Birds/"+birdName;
			i++;
		}

	}

	void Update(){
		if (spawnBirds){
			spawnBirds = false;
			StartCoroutine(SpawnNextBird(birdType));
		}
	}

	public IEnumerator SpawnNextBird(int birdTypeInput){
		bird = Instantiate (Resources.Load (birdNames [birdTypeInput]), new Vector3(/*Mathf.Sign (Random.insideUnitCircle.x) * 9f*/-9f,Random.Range (-4.5f,4.5f),0f), Quaternion.identity) as GameObject;
		//birdsSpawned++;
		yield return null;
	}
}
