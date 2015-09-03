using UnityEngine;
using System.Collections;

public class GetHurt : MonoBehaviour {

	public int health;
	public int birdType;
	public string[] gutSplosions;
	public int randomGutNumber;
	public string thisGutSplosion;

	// Use this for initialization
	void Awake () {
		health = 1;
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

		if (GetComponent<Duck> ()) {
			birdType = 1;
			thisGutSplosion = gutSplosions [0];
		}
		else if (GetComponent<Pigeon>()){
			randomGutNumber = Random.Range (1, 5);
			birdType = 2;
			thisGutSplosion = gutSplosions [randomGutNumber];
		}
		else if (GetComponent<Stork>()){
			birdType = 3;
			health = 2;
			randomGutNumber = Random.Range (5, 7);
			thisGutSplosion = gutSplosions [randomGutNumber];
		}
	}

	public IEnumerator TakeDamage(Vector2 gutDirection){
		health--;
		if (health>0){
			randomGutNumber = 0;
		}
		GameObject guts = Instantiate (Resources.Load (gutSplosions [randomGutNumber]), transform.position, Quaternion.identity) as GameObject;
		guts.gameObject.GetComponent<Rigidbody2D>().velocity = gutDirection;
		if (health<1){
			Destroy(gameObject);
		}
		yield return null;
	}
}
