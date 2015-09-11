using UnityEngine;
using System.Collections;
using System.Linq;
using GenericFunctions;

public class GutSplosion : MonoBehaviour {

	public GameObject[] guts;

	public string[] gutSplosions;

	public int[] gutIndices;
	public int gutValue;
	public int subGutValue;

	void Awake(){
		gutIndices = Constants.NegativeOnes(100);
		Destroy(gameObject,Constants.time2Destroy);
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

	public int ConvertGutValueToIndex(int subGutterValue){
		int gutIndex = 0;
		switch (subGutterValue){
		case 1:
			gutIndex = 0;
			break;
		case 2:
			gutIndex = Random.Range(1,5);
			break;
		case 3:
			gutIndex = Random.Range(5,7);
			break;
		}
		return gutIndex;
	}

	public IEnumerator GenerateGuts(int totalGutValue, Vector2 gutDirection){
		int j = 0;
		while (gutValue<totalGutValue){
			subGutValue = Mathf.Clamp(Random.Range(1,4),1,totalGutValue-gutValue);
			gutIndices[j] = ConvertGutValueToIndex(subGutValue);
			gutValue += subGutValue;
			j++;
		}
		gutIndices = gutIndices.Where (number => number != -1).ToArray ();
		guts = new GameObject[gutIndices.Length];
		j = 0;
		foreach (int i in gutIndices){
			guts[j] = Instantiate (Resources.Load(gutSplosions[i]),new Vector3 (Random.insideUnitCircle.x,Random.insideUnitCircle.y,0f) * .2f + transform.position,Quaternion.identity) as GameObject;
			guts[j].GetComponent<Rigidbody2D>().velocity = new Vector2 (Random.Range(gutDirection.x * .1f,gutDirection.x * .4f),Random.Range(3f,8f));
			guts[j].transform.parent = transform;
			j++;
		}

		yield return null;
	}
}
