using UnityEngine;
using System.Collections;

public class GetHurt : MonoBehaviour {

	public GameObject guts;
	public Basket basketScript;
	public SummonTheCrows summonTheCrowsScript;
	public DuckLeader duckLeaderScript;
	public Duck duckScript;
	public Waves wavesScript;

	public string[] gutSplosions;
	public string gutSplosionParentString;
	public string thisGutSplosion;

	public int health;
	public int gutValue;
	public int birdType;

	public bool summonCrows;
	public bool invincible;
	public bool spawnBalloon;

	// Use this for initialization
	void Awake () {
		health = 1;
		//wavesScript = GameObject.Find ("WorldBounds").GetComponent<Waves> ();
		summonTheCrowsScript = GameObject.Find ("WorldBounds").GetComponent<SummonTheCrows> ();
		duckLeaderScript = GetComponent<DuckLeader> ();
		duckScript = GetComponent<Duck> ();

		gutSplosionParentString = "Prefabs/Birds/GutSplosionParent";
		gutSplosions = new string[]{
			"Prefabs/GutSplosions/GutSplosion1a", //small birds  //0
			"Prefabs/GutSplosions/GutSplosion2a", //medium birds //1
			"Prefabs/GutSplosions/GutSplosion2b",				 //1
			"Prefabs/GutSplosions/GutSplosion2c",				 //2
			"Prefabs/GutSplosions/GutSplosion2d",				 //3
			"Prefabs/GutSplosions/GutSplosion2e",				 //4
			"Prefabs/GutSplosions/GutSplosion3a", //big birds	 //5
			"Prefabs/GutSplosions/GutSplosion3b",				 //6
		};
		if (GetComponent<Pigeon>()){
			birdType = 0;
			gutValue = 3;
		}
		else if (GetComponent<Duck> ()) {
			birdType = 1;
			gutValue = 5;
		} 
		else if (GetComponent<DuckLeader>()){
			birdType = 2;
			gutValue = 7;
		}
		else if (GetComponent<Albatross>()){
			birdType = 3;
			health = 7;
			gutValue = 11;
		}
		else if (GetComponent<BabyCrow> ()) {
			birdType = 4;
			gutValue = 3;
			summonCrows = true;
		}
		else if (GetComponent<Crow>()){
			birdType = 5;
			gutValue = 7;
		}
		else if (GetComponent<Eagle>()){
			birdType = 6;
			health = 3;
			gutValue = 9;
		}
		else if (GetComponent<BirdOfParadise>()){
			birdType = 7;
			gutValue = 15;
			spawnBalloon = true;
		}

		basketScript = GameObject.Find ("BalloonBasket").GetComponent<Basket> ();
	}
	
	public IEnumerator TakeDamage(Vector2 gutDirection){
		if (!invincible){
			invincible = true;
			StartCoroutine (Vincible());
			health--;
			if (health>0){
				gutValue = 0;
			}
			guts = Instantiate (Resources.Load (gutSplosions[6]), transform.position, Quaternion.identity) as GameObject;
			guts.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2 (gutDirection.x * .3f,5f);
			StartCoroutine (guts.GetComponent<GutSplosion> ().GenerateGuts (gutValue));
			if (health<1){
				StartCoroutine (basketScript.SlowTime(.1f,.5f));
				//wavesScript.waveBirdsStillAlive[wavesScript.currentWave-1][birdType]--;
				//wavesScript.numberOfBirdsStillAlive--;
				if (summonCrows){
					StartCoroutine (summonTheCrowsScript.Murder());
					//wavesScript.waveBirdsStillAlive[wavesScript.currentWave-1] [4] = 6;//add 6 crows to the wave tracker
				}
				else if (spawnBalloon){
					StartCoroutine (basketScript.SpawnNewBalloon());
				}
				else if (duckLeaderScript){
					StartCoroutine (duckLeaderScript.BreakTheV());
				}
				else if (duckScript){
					if (duckScript.duckLeaderScript){
						StartCoroutine(duckScript.duckLeaderScript.ReShuffle(duckScript.formationNumber));
					}
				}
				Destroy(gameObject);
			}
		}
		yield return null;
	}

	public IEnumerator Vincible(){
		yield return new WaitForSeconds (.5f);
		invincible = false;
	}
}
