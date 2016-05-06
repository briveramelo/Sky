using UnityEngine;
using GenericFunctions;
using Vexe.Runtime.Types;

public class Incubator : BaseBehaviour {

	public static Incubator Instance;

	[SerializeField] GameObject[] birds; public GameObject[] Birds{get{return birds;}}

    [Show] void Pigeon() {SpawnNextBird(BirdType.Pigeon); }
    [Show] void Duck() {SpawnNextBird(BirdType.Duck); }
    [Show] void DuckLeader() {SpawnNextBird(BirdType.DuckLeader); }
    [Show] void Albatross() {SpawnNextBird(BirdType.Albatross); }
    [Show] void BabyCrow() {SpawnNextBird(BirdType.BabyCrow); }
    [Show] void Crow() {SpawnNextBird(BirdType.Crow); }
    [Show] void Tentacles() {SpawnNextBird(BirdType.Tentacles); }
    [Show] void Seagull() {SpawnNextBird(BirdType.Seagull); }
    [Show] void Pelican() {SpawnNextBird(BirdType.Pelican); }
    [Show] void Shoebill() {SpawnNextBird(BirdType.Shoebill); }
    [Show] void Bat() {SpawnNextBird(BirdType.Bat); }
    [Show] void Eagle() {SpawnNextBird(BirdType.Eagle); }
    [Show] void BirdOfParadise() {SpawnNextBird(BirdType.BirdOfParadise); }

	void Awake(){
		Instance = this;
	}

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
