using UnityEngine;
using System.Collections;

public class GutSplosion : MonoBehaviour {

	public AudioSource gutSplode;

	public string[] gutSplosions;

	public int[] numberOfGuts;
	public int total;

	void Awake(){
		numberOfGuts = new int[3];
		StartCoroutine (DestroySelf ());
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
	}

	//3 do 1 + 2
	//5 do 1 + 2 + 2
	//7 do 2 + 2 + 3

	public IEnumerator GenerateGuts(int gutValue){
		numberOfGuts [0] = Random.Range (0, 3); //0,1,2
		foreach (int gutNum in numberOfGuts){
			total += gutNum;
		}
		numberOfGuts [1] = Random.Range (0, 3);
		if (total>=gutValue){

		}

		switch (gutValue) {
		case 3:
			numberOfGuts = new int[] { 1 , 1 , 0};
			break;
		case 5:
			numberOfGuts = new int[] { 5 , 0 , 0};
			numberOfGuts = new int[] { 3 , 1 , 0};
			numberOfGuts = new int[] { 1 , 2 , 0};
			numberOfGuts = new int[] { 2 , 0 , 1};
			numberOfGuts = new int[] { 0 , 1 , 1};
			break;
		case 7:
			numberOfGuts = new int[] { 1 , 1 , 0};
			numberOfGuts = new int[] { 1 , 1 , 0};
			break;
		}

		int random2 = Random.Range (1, 5);
		Vector3 randomPos = new Vector3 (Random.insideUnitCircle.x,Random.insideUnitCircle.y,0f) * .05f + transform.position;
		GameObject gut = Instantiate (Resources.Load(gutSplosions[random2]),randomPos,Quaternion.identity) as GameObject;
		gut.transform.SetParent (transform);
		yield return null;
	}

	public IEnumerator DestroySelf(){
		while (gutSplode.isPlaying){
			yield return null;
		}
		Destroy (gameObject);
		yield return null;
	}
}
