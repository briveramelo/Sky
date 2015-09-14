using UnityEngine;
using System.Collections;
using GenericFunctions;

public class GetHurt : MonoBehaviour {

	public GameObject guts;
	public Basket basketScript;
	public SummonTheCrows summonTheCrowsScript;
	public DuckLeader duckLeaderScript;
	public Duck duckScript;
	public Waves wavesScript;

	public CircleCollider2D[] spearColliders;
	public Collider2D birdCollider;

	public string gutSplosionParentString;

	public Vector2 hitPoint;

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
		birdCollider = GetComponent<Collider2D> ();

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
			health = 5;
			damageGutValue = 4;
			killGutValue = 80;
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

			hitPoint = birdCollider.bounds.ClosestPoint(spearCollider.bounds.ClosestPoint(transform.position));
			if (birdType==3 && hitPoint.y<transform.position.y && hitPoint.x>birdCollider.bounds.min.x && hitPoint.x<birdCollider.bounds.max.x){ //kill albatross with a tactical shot to the underbelly
				health = 0;
				//super kill!
			}

			spearColliders[health] = spearCollider;
			guts = Instantiate (Resources.Load (gutSplosionParentString), transform.position, Quaternion.identity) as GameObject;

			if (health>0){
				StartCoroutine (guts.GetComponent<GutSplosion> ().GenerateGuts (damageGutValue, gutDirection));
			}
			else{
				StartCoroutine (guts.GetComponent<GutSplosion> ().GenerateGuts (killGutValue, gutDirection));
				StartCoroutine (TimeEffects.SlowTime(.1f,.5f));
				//wavesScript.waveBirdsStillAlive[wavesScript.currentWave-1][birdType]--;
				//wavesScript.numberOfBirdsStillAlive--;
				if (summonCrows){
					StartCoroutine (summonTheCrowsScript.Murder());
					//wavesScript.waveBirdsStillAlive[wavesScript.currentWave-1] [4] = 6;//add 6 crows to the wave tracker
				}
				else if (spawnBalloon){
					StartCoroutine (Spawn.NewBalloon());
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
