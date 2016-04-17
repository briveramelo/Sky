using UnityEngine;
using GenericFunctions;

public class Incubator : MonoBehaviour {

	public static Incubator Instance;

	[SerializeField] private bool Pigeon;
	[SerializeField] private bool Duck;
	[SerializeField] private bool DuckLeader;
	[SerializeField] private bool Albatross;
	[SerializeField] private bool BabyCrow;
	[SerializeField] private bool Murder;
	[SerializeField] private bool Seagull;
	[SerializeField] private bool Tentacles;
	[SerializeField] private bool Pelican;
	[SerializeField] private bool Shoebill;
	[SerializeField] private bool Bat;
	[SerializeField] private bool Eagle;
	[SerializeField] private bool BirdOfParadise;

	[SerializeField] private GameObject[] birds; public GameObject[] Birds{get{return birds;}}

	void Awake(){
		Instance = this;
	}

	#region Live Spawning
	void Update(){
		if (Pigeon){
			Pigeon = false;
			SpawnNextBird(BirdType.Pigeon);
		}
		if (Duck){
			Duck = false;
			SpawnNextBird(BirdType.Duck);
		}
		if (DuckLeader){
			DuckLeader = false;
			SpawnNextBird(BirdType.DuckLeader);
		}
		if (Albatross){
			Albatross = false;
			SpawnNextBird(BirdType.Albatross);
		}
		if (BabyCrow){
			BabyCrow = false;
			SpawnNextBird(BirdType.BabyCrow);
		}
		if (Murder){
			Murder = false;
			SpawnNextBird(BirdType.Crow);
		}
		if (Seagull){
			Seagull = false;
			SpawnNextBird(BirdType.Seagull);
		}
		if (Tentacles){
			Tentacles = false;
			SpawnNextBird(BirdType.Tentacles);
		}
		if (Pelican){
			Pelican = false;
			SpawnNextBird(BirdType.Pelican);
		}
		if (Shoebill){
			Shoebill = false;
			SpawnNextBird(BirdType.Shoebill);
		}
		if (Bat){
			Bat = false;
			SpawnNextBird(BirdType.Bat);
		}
		if (Eagle){
			Eagle = false;
			SpawnNextBird(BirdType.Eagle);
		}
		if (BirdOfParadise){
			BirdOfParadise = false;
			SpawnNextBird(BirdType.BirdOfParadise);
		}

	}
	#endregion

	public void SpawnNextBird(BirdType birdType){
		float xSpot = -Constants.WorldDimensions.x;
		float ySpot = Random.Range (-Constants.WorldDimensions.y, Constants.WorldDimensions.y) * 0.6f;
		if (birdType == BirdType.Tentacles || birdType == BirdType.Crow){
			xSpot = 0f;
			ySpot = 0f;
		}
		else if (birdType == BirdType.Eagle){
			xSpot= -Constants.WorldDimensions.x *5f;
		}
		Instantiate (birds [(int)birdType], new Vector3(xSpot,ySpot,0f), Quaternion.identity);
	}
}
