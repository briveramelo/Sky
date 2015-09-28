using UnityEngine;
using System.Collections;
using GenericFunctions;

public class WorldEffects : MonoBehaviour {

	public GameObject bird;
	public BoxCollider2D[] worldBoundsForcePushers;
	public EdgeCollider2D worldEdgeCollider;
	public string[] birdNames; //file location so you can load the right bird
	
	[Range(0,12)]
	public int birdType;
	public int targetPooInt;
	public bool spawnBirds;
	
	// Use this for initialization
	void Awake () {
		birdNames = Constants.birdNamePrefabs;
		worldBoundsForcePushers = GetComponentsInChildren<BoxCollider2D> ();
		worldBoundsForcePushers [0].offset = new Vector2 (0f, Constants.worldDimensions.y);
		worldBoundsForcePushers [1].offset = new Vector2 (Constants.worldDimensions.x, 0f);
		worldBoundsForcePushers [2].offset = new Vector2 (0f, -Constants.worldDimensions.y);
		worldBoundsForcePushers [3].offset = new Vector2 (-Constants.worldDimensions.x, 0f);

		worldBoundsForcePushers [0].size = new Vector2 (Constants.worldDimensions.x * 2, Constants.worldDimensions.y * .15625f);
		worldBoundsForcePushers [1].size = new Vector2 (Constants.worldDimensions.y * .15625f, Constants.worldDimensions.y * 2);
		worldBoundsForcePushers [2].size = new Vector2 (Constants.worldDimensions.x * 2, Constants.worldDimensions.y * .15625f);
		worldBoundsForcePushers [3].size = new Vector2 (Constants.worldDimensions.y * .15625f, Constants.worldDimensions.y * 2);

		worldEdgeCollider = GetComponent<EdgeCollider2D> ();
		worldEdgeCollider.points = Constants.worldEdgePoints;
	}

	public IEnumerator Murder(){
		GameObject crows = Instantiate (Resources.Load (Constants.murderPrefab), Vector3.zero, Quaternion.identity) as GameObject;
		yield return null;
	}

	public IEnumerator SlowTime(float slowDuration, float timeScale){
		StartCoroutine (Wait4RealSeconds (slowDuration));
		Time.timeScale = timeScale;
		yield return null;
	}
	
	public IEnumerator Wait4RealSeconds(float slowDuration){
		float startTime = Time.realtimeSinceStartup;
		while (Time.realtimeSinceStartup - startTime < slowDuration){
			yield return null;
		}
		Time.timeScale = 1f;
	}



	void Update(){
		if (spawnBirds){
			spawnBirds = false;
			StartCoroutine(SpawnNextBird(birdType));
		}
	}

	public IEnumerator SpawnNextBird(int birdTypeInput){
		float xSpot = -Constants.worldDimensions.x;
		float ySpot = Random.Range (-Constants.worldDimensions.y, Constants.worldDimensions.y) * 0.8f;
		if (birdTypeInput == Constants.tentacles){
			xSpot = 0f;
			ySpot = 0f;
		}
		bird = Instantiate (Resources.Load (birdNames [birdTypeInput]), new Vector3(/*Mathf.Sign (Random.insideUnitCircle.x) * 9f*/xSpot,ySpot,0f), Quaternion.identity) as GameObject;
		yield return null;
	}
}
