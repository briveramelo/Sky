using UnityEngine;
using System.Collections;
using GenericFunctions;

public class Tentacles : MonoBehaviour {

	public CircleCollider2D[] spearColliders;
	public TimeEffects timeEffectsScript;
	public Joyfulstick joyfulStickScript;
	public Basket basketScript;

	public Rigidbody2D basketBody;
	public Rigidbody2D rigbod;

	public string gutSplosionParentString;

	public float descendSpeed;

	public int health;
	public int stabsTaken;
	public int damageGutValue;
	public int killGutValue;
	public int stabs2Retreat;

	public bool holding;
	public bool rising;

	void Awake(){
		health = 25;
		stabs2Retreat = 10;
		damageGutValue = 4;
		killGutValue = 60;
		descendSpeed = 2f;
		rigbod = GetComponent<Rigidbody2D> ();
		basketBody = GameObject.Find ("BalloonBasket").GetComponent<Rigidbody2D> ();
		basketScript = GameObject.Find ("BalloonBasket").GetComponent<Basket> ();
		timeEffectsScript = GameObject.Find ("Dummy").GetComponent<TimeEffects> ();
		joyfulStickScript = GameObject.Find ("StickHole").GetComponent<Joyfulstick> ();
		gutSplosionParentString = "Prefabs/GutSplosions/GutSplosionParent";
		spearColliders = new CircleCollider2D[health];

	}
	
	public IEnumerator Surface(int targetType, int birdType){ //13 means basket, 16 means bird
		rising = true;

		if (birdType == 0 || birdType == 1 || birdType == 2 || birdType == 4 || birdType == 5 || birdType == 6){
			//rise up to grab birds
		}
		else if (birdType == -1){
			//rise up to grab you
		}

		while (!holding && rising){
			yield return null;
		}

		yield return null;
	}

	void OnTriggerEnter2D(Collider2D col){
		if (!holding && col.gameObject.layer == 13){
			StartCoroutine (SnatchAndGrab());
		}
	}

	public IEnumerator SnatchAndGrab(){
		holding = true;
		rising = false;
		yield return null;
		//animate
		joyfulStickScript.beingHeld = true;
		StartCoroutine (Descend ());
		//prevent him from moving, throwing spears
		//only allow him to stab
		//start the descent
	}

	public IEnumerator Descend(){
		rising = false;
		while (!holding && !rising){
			basketBody.velocity = Vector2.down * descendSpeed;
			rigbod.velocity = Vector2.down * descendSpeed;
			if (transform.position.y<-6f){ //kill him
				foreach (Balloon balloonScript in basketScript.balloonScripts){ //float the balloons
					if (balloonScript){
						balloonScript.transform.parent = null;
						balloonScript.moving = true;
						StartCoroutine (balloonScript.MoveUp());
					}
				}
				Spawn.EndGame();
			}
			yield return null;
		}
		basketBody.velocity = Vector2.zero;
		rigbod.velocity = Vector2.zero;
		yield return null;
	}

	public IEnumerator TakeStabs(){
		stabsTaken++;
		if (stabsTaken>10){
			StartCoroutine (Retreat());
			holding = false;
			joyfulStickScript.beingHeld = false;
		}
		yield return null;
	}

	public IEnumerator Retreat(){
		stabsTaken = 0;
		yield return null;
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
			GameObject guts = Instantiate (Resources.Load (gutSplosionParentString), transform.position, Quaternion.identity) as GameObject;
			
			if (health>0){
				StartCoroutine (guts.GetComponent<GutSplosion> ().GenerateGuts (damageGutValue, gutDirection));
			}
			else{
				StartCoroutine (guts.GetComponent<GutSplosion> ().GenerateGuts (killGutValue, gutDirection));
				StartCoroutine (timeEffectsScript.SlowTime(.1f,.5f));
				Destroy(gameObject);
			}
		}
		yield return null;
	}
}
