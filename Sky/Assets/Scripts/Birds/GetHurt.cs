using UnityEngine;
using System.Collections;
using GenericFunctions;

public class GetHurt : MonoBehaviour {

	public GameObject guts;
	public Basket basketScript;
	public WaveManager waveManagerScript;
	public WorldEffects worldEffectsScript;
	public DuckLeader duckLeaderScript;
	public Duck duckScript;
	public Tentacles tentaclesScript;

	public CircleCollider2D[] spearColliders;
	public Collider2D birdCollider;
	
	public Vector2 hitPoint;

	public int health;
	public int killPointValue;
	public int damagePointValue;
	public float killPointMultiplier;

	public int killGutValue;
	public int damageGutValue;
	public int birdType;

	public bool summonCrows;
	public bool spawnBalloon;

	// Use this for initialization
	void Awake () {
		health = 1;
		worldEffectsScript = GameObject.Find ("WorldBounds").GetComponent<WorldEffects> ();
		waveManagerScript = GameObject.Find ("WorldBounds").GetComponent<WaveManager> ();
		duckLeaderScript = GetComponent<DuckLeader> ();
		duckScript = GetComponent<Duck> ();
		birdCollider = GetComponent<Collider2D> ();
		tentaclesScript = GetComponent<Tentacles> ();

		if (GetComponent<Pigeon>()){
			birdType = Constants.pigeon;
			killGutValue = 3;

			killPointValue = 1;
		}

		else if (GetComponent<Duck> ()) {
			birdType = Constants.duck;
			killGutValue = 4;

			killPointValue = 3; //1 if in formation- it's corrected down below :D
		} 

		else if (GetComponent<DuckLeader>()){
			birdType = Constants.duckLeader;
			killGutValue = 7;

			killPointValue = 2;
		}

		else if (GetComponent<Albatross>()){
			birdType = Constants.albatross;
			health = 7;
			damageGutValue = 4;
			killGutValue = 20;

			damagePointValue = 1;
			killPointValue = 4;
		}

		else if (GetComponent<BabyCrow> ()) {
			birdType = Constants.babyCrow;
			killGutValue = 2;

			killPointValue = 0;
			summonCrows = true;
		}

		else if (GetComponent<Crow>()){
			birdType = Constants.crow;
			killGutValue = 5;

			killPointValue = 2;
		}

		else if (GetComponent<Seagull>()){
			birdType = Constants.seagull;
			killGutValue = 4;

			killPointMultiplier = 2; //specialty (multiplier for onscreen point values)
		}

		else if (GetComponent<Tentacles>()){
			birdType = Constants.tentacles;
			health = 25;
			damageGutValue = 4;
			killGutValue = 80;

			damagePointValue= 1;
			killPointValue = 10;
			killPointMultiplier = 1.5f;
		}

		else if (GetComponent<Pelican>()){
			birdType = Constants.pelican;
			health = 3;
			damageGutValue = 4;
			killGutValue = 15;

			damagePointValue = 2;
			killPointValue = 3;
		}

		else if (GetComponent<Bat>()){
			birdType = Constants.bat;
			killGutValue = 3;

			killPointValue = 1;
		}

		else if (GetComponent<Eagle>()){
			birdType = Constants.eagle;
			health = 5;
			damageGutValue = 4;
			killGutValue = 80;

			damagePointValue = 3;
			killPointValue = 20;
		}

		else if (GetComponent<BirdOfParadise>()){
			birdType = Constants.birdOfParadise;
			killGutValue = 40;

			killPointValue = 10;
			spawnBalloon = true;
		}
		spearColliders = new CircleCollider2D[health];
		basketScript = GameObject.Find ("BalloonBasket").GetComponent<Basket> ();

		InvokeRepeating ("CheckSpot", 2f,2f);
		waveManagerScript.StartCoroutine (waveManagerScript.Birth (birdType));
	}

	void CheckSpot(){
		if (Vector3.Distance(transform.position,Vector3.zero)>30f){
			Destroy(gameObject);
		}
	}
	
	public IEnumerator TakeDamage(Vector2 gutDirection, CircleCollider2D spearCollider, bool hurtHealth){
		Vector3 spawnSpot = transform.position;

		if (hurtHealth){
			health--;
			spearColliders[health] = spearCollider;
		}

		if (birdType == Constants.tentacles && !hurtHealth){
			spawnSpot = transform.GetChild(0).position;
			gutDirection = new Vector3 (Mathf.Abs (Random.insideUnitCircle.x), Mathf.Abs (Random.insideUnitCircle.y),0f).normalized;
		}
		else if (birdType == Constants.tentacles && hurtHealth){
			spawnSpot = birdCollider.bounds.ClosestPoint(spearCollider.transform.position);
		}
		else if (birdType == Constants.albatross){
			hitPoint = birdCollider.bounds.ClosestPoint(spearCollider.transform.position);
			if (gutDirection.y>0 && hitPoint.y<transform.position.y && hitPoint.x>birdCollider.bounds.min.x && hitPoint.x<birdCollider.bounds.max.x){ //kill albatross with a tactical shot to the underbelly
				health = 0;
				//super kill!
			}
		}
		guts = Instantiate (Resources.Load (Constants.gutSplosionParentPrefab), spawnSpot, Quaternion.identity) as GameObject;

		if (health>0){
			StartCoroutine (guts.GetComponent<GutSplosion> ().GenerateGuts (damageGutValue, gutDirection));
			StartCoroutine (waveManagerScript.AddPoints (birdType,damagePointValue,0));
		}
		else{
			StartCoroutine (guts.GetComponent<GutSplosion> ().GenerateGuts (killGutValue, gutDirection));
			StartCoroutine (worldEffectsScript.SlowTime(.1f,.5f));
			if (summonCrows){
				StartCoroutine (worldEffectsScript.Murder());
			}
			else if (spawnBalloon){
				StartCoroutine (Spawn.NewBalloon());
			}
			else if (duckLeaderScript){
				StartCoroutine (duckLeaderScript.BreakTheV());
			}
			else if (duckScript){
				if (duckScript.duckLeaderScript){
					killPointValue = 1;
					StartCoroutine(duckScript.duckLeaderScript.ReShuffle(duckScript.formationNumber));
				}
			}
			else if (tentaclesScript){
				StartCoroutine(tentaclesScript.StopThemAll());
				StartCoroutine(tentaclesScript.tentaclesSensorScript.StopThem());
			}
			StartCoroutine (waveManagerScript.AddPoints (birdType,killPointValue,killPointMultiplier));
			Destroy(gameObject);
		}
		yield return null;
	}

	void OnDestroy(){
		if (waveManagerScript){
			waveManagerScript.StartCoroutine (waveManagerScript.Death (birdType));
		}
	}


}
