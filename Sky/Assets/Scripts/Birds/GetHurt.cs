﻿using UnityEngine;
using System.Collections;

public class GetHurt : MonoBehaviour {

	public int health;
	public string gutSplosionParentString;
	public int gutValue;
	public string thisGutSplosion;
	public string[] gutSplosions;
	public BalloonBasket balloonBasketScript;
	public Waves wavesScript;
	public int birdType;
	public GameObject guts;
	public bool summonCrows;
	public SummonTheCrows summonTheCrowsScript;
	public bool invincible;

	// Use this for initialization
	void Awake () {
		health = 1;
		wavesScript = GameObject.Find ("WorldBounds").GetComponent<Waves> ();
		summonTheCrowsScript = GameObject.Find ("WorldBounds").GetComponent<SummonTheCrows> ();
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
		if (GetComponent<BabyCrow> ()) {
			birdType = 5;
			gutValue = 3;
			summonCrows = true;
		}
		if (GetComponent<Pigeon>()){
			birdType = 0;
			gutValue = 3;
		}
		else if (GetComponent<Duck> ()) {
			birdType = 1;
			gutValue = 5;
		} 
		else if (GetComponent<Crow>()){
			birdType = 2;
			gutValue = 7;
		}
		else if (GetComponent<Eagle>()){
			birdType = 3;
			health = 3;
			gutValue = 9;
		}
		else if (GetComponent<Albatross>()){
			birdType = 4;
			health = 7;
			gutValue = 11;
		}
		balloonBasketScript = GameObject.Find ("BalloonBasket").GetComponent<BalloonBasket> ();
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
				StartCoroutine (balloonBasketScript.SlowTime(.1f,.5f));
				wavesScript.currentWaveBirdsStillAlive[birdType]--;
				StartCoroutine(wavesScript.CheckBirds());
				if (summonCrows){
					StartCoroutine (summonTheCrowsScript.Murder());
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
