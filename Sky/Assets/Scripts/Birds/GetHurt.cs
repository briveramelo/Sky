using UnityEngine;
using System.Collections;

public class GetHurt : MonoBehaviour {

	public GameObject guts;
	public Basket basketScript;
	public SummonTheCrows summonTheCrowsScript;
	public DuckLeader duckLeaderScript;
	public Duck duckScript;
	public Waves wavesScript;

	public string gutSplosionParentString;

	public int health;
	public int killGutValue;
	public int damageGutValue;
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

		gutSplosionParentString = "Prefabs/GutSplosions/GutSplosionParent";
		if (GetComponent<Pigeon>()){
			birdType = 0;
			killGutValue = 3;
		}
		else if (GetComponent<Duck> ()) {
			birdType = 1;
			killGutValue = 4;
		} 
		else if (GetComponent<DuckLeader>()){
			birdType = 2;
			killGutValue = 7;
		}
		else if (GetComponent<Albatross>()){
			birdType = 3;
			health = 7;
			damageGutValue = 4;
			killGutValue = 20;
		}
		else if (GetComponent<BabyCrow> ()) {
			birdType = 4;
			killGutValue = 2;
			summonCrows = true;
		}
		else if (GetComponent<Crow>()){
			birdType = 5;
			killGutValue = 5;
		}
		else if (GetComponent<Eagle>()){
			birdType = 6;
			health = 3;
			damageGutValue = 4;
			killGutValue = 70;
		}
		else if (GetComponent<BirdOfParadise>()){
			birdType = 7;
			killGutValue = 40;
			spawnBalloon = true;
		}

		basketScript = GameObject.Find ("BalloonBasket").GetComponent<Basket> ();
	}
	
	public IEnumerator TakeDamage(Vector2 gutDirection){
		if (!invincible){
			invincible = true;
			StartCoroutine (Vincible());
			health--;
			guts = Instantiate (Resources.Load (gutSplosionParentString), transform.position, Quaternion.identity) as GameObject;

			if (health>0){
				StartCoroutine (guts.GetComponent<GutSplosion> ().GenerateGuts (damageGutValue, gutDirection));
			}
			else{
				StartCoroutine (guts.GetComponent<GutSplosion> ().GenerateGuts (killGutValue, gutDirection));
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
