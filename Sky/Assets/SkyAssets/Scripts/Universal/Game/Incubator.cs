using UnityEngine;
using GenericFunctions;
//using Vexe.Runtime.Types;

public class Incubator : MonoBehaviour {

	public static Incubator Instance;

	[SerializeField] GameObject[] birds; public GameObject[] Birds => birds;

	void Pigeon() {SpawnNextBird(BirdType.Pigeon); }
    void Duck() {SpawnNextBird(BirdType.Duck); }
    void DuckLeader() {SpawnNextBird(BirdType.DuckLeader); }
    void Albatross() {SpawnNextBird(BirdType.Albatross); }
    void BabyCrow() {SpawnNextBird(BirdType.BabyCrow); }
    void Crow() {SpawnNextBird(BirdType.Crow); }
    void Tentacles() {SpawnNextBird(BirdType.Tentacles); }
    void Seagull() {SpawnNextBird(BirdType.Seagull); }
    void Pelican() {SpawnNextBird(BirdType.Pelican); }
    void Shoebill() {SpawnNextBird(BirdType.Shoebill); }
    void Bat() {SpawnNextBird(BirdType.Bat); }
    void Eagle() {SpawnNextBird(BirdType.Eagle); }
    void BirdOfParadise() {SpawnNextBird(BirdType.BirdOfParadise); }

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
        else if (birdType == BirdType.Seagull){
			xSpot= Constants.WorldDimensions.x * (Bool.TossCoin() ? 1:-1);
		}
		Instantiate (birds [(int)birdType], new Vector3(xSpot,ySpot,0f), Quaternion.identity);
	}
}
