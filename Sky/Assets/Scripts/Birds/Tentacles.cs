using UnityEngine;
using System.Collections;

public class Tentacles : MonoBehaviour {

	public CircleCollider2D[] spearColliders;
	public TimeEffects timeEffectsScript;

	public string gutSplosionParentString;

	public int health;
	public int stabsTaken;
	public int damageGutValue;
	public int killGutValue;

	public bool grabbing;
	public bool rising;

	void Awake(){
		health = 25;
		damageGutValue = 4;
		killGutValue = 60;
		timeEffectsScript = GameObject.Find ("Dummy").GetComponent<TimeEffects> ();
		gutSplosionParentString = "Prefabs/GutSplosions/GutSplosionParent";
		spearColliders = new CircleCollider2D[health];

	}
	
	public IEnumerator Surface(int targetType, int birdType){ //13 means basket, 16 means bird
		rising = true;
		while (!grabbing && rising)
		if (birdType == 0 || birdType == 1 || birdType == 2 || birdType == 4 || birdType == 5 || birdType == 6){
			//rise up to grab birds
		}
		else if (birdType == -1){
			//rise up to grab you
		}
		yield return null;
	}

	void OnTriggerEnter2D(Collider2D col){
		if (!grabbing && col.gameObject.layer == 13){
			StartCoroutine (SnatchAndGrab());
		}
	}

	public IEnumerator SnatchAndGrab(){
		grabbing = true;
		rising = false;
		yield return null;
		//animate
		//prevent him from moving, throwing spears
		//only allow him to stab
		//start the descent
	}

	public IEnumerator Descend(){
		grabbing = false;
		rising = false;
		yield return null;
	}

	public IEnumerator TakeStabs(){
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
