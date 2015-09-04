using UnityEngine;
using System.Collections;

public class GetHurt : MonoBehaviour {

	public int health;
	public int birdType;
	public string[] gutSplosions;
	public int randomGutNumber;
	public string thisGutSplosion;
	public BalloonBasket balloonBasketScript;
	public Waves wavesScript;

	// Use this for initialization
	void Awake () {
		health = 1;
		wavesScript = GameObject.Find ("WorldBounds").GetComponent<Waves> ();
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
			thisGutSplosion = gutSplosions [0];
		}
		else if (GetComponent<Duck> ()) {
			randomGutNumber = Random.Range (1, 5);
			birdType = 1;
			thisGutSplosion = gutSplosions [randomGutNumber];
		} 
		else if (GetComponent<Stork>()){
			birdType = 5;
			health = 2;
			randomGutNumber = Random.Range (5, 7);
			thisGutSplosion = gutSplosions [randomGutNumber];
		}
		balloonBasketScript = GameObject.Find ("BalloonBasket").GetComponent<BalloonBasket> ();
	}

	public IEnumerator TakeDamage(Vector2 gutDirection){
		health--;
		if (health>0){
			randomGutNumber = 0;
		}
		GameObject guts = Instantiate (Resources.Load (gutSplosions [randomGutNumber]), transform.position, Quaternion.identity) as GameObject;
		guts.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2 (gutDirection.x * .3f,5f);
		if (health<1){
			StartCoroutine (balloonBasketScript.SlowTime(.1f,.5f));
			wavesScript.currentWaveBirdsStillAlive[birdType]--;
			StartCoroutine(wavesScript.CheckBirds());
			Destroy(gameObject);
		}
		yield return null;
	}
}
