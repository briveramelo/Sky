using UnityEngine;
using System.Collections;

public class GetHurt : MonoBehaviour {

	public GameObject guts;
	public Basket basketScript;
	public SummonTheCrows summonTheCrowsScript;
	public DuckLeader duckLeaderScript;
	public Duck duckScript;
	public Waves wavesScript;

	public CircleCollider2D[] spearColliders;

	public string gutSplosionParentString;

	public int health;
	public int killGutValue;
	public int damageGutValue;
	public int birdType;

	public bool summonCrows;
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
			killGutValue = 50;
		}
		else if (GetComponent<BirdOfParadise>()){
			birdType = 7;
			killGutValue = 40;
			spawnBalloon = true;
		}
		spearColliders = new CircleCollider2D[health];
		basketScript = GameObject.Find ("BalloonBasket").GetComponent<Basket> ();
	}
	
	public IEnumerator TakeDamage(Vector2 gutDirection, CircleCollider2D spearCollider){
		bool invincible = false;
		int i = 0;
		for (i=0;i<spearColliders.Length;i++){
			if (spearCollider==spearColliders[i]){
				invincible = true;
				break;
			}
		}

		if (!invincible){
			health--;
			spearColliders[health] = spearCollider;
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
}
