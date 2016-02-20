using UnityEngine;
using System.Collections;
using System.Linq;
using GenericFunctions;

public class GutSplosion : MonoBehaviour {

	[SerializeField] private GameObject[] gutSplosions;
	private int[] gutIndices;

	void Awake(){
		gutIndices = Constants.NegativeOnes(100);
		Destroy(gameObject,Constants.time2Destroy);
	}

	public void GenerateGuts(int totalGutValue, Vector2 gutDirection){
		int j = 0;
		int gutValue = 0;
		int subGutValue = 0;
		while (gutValue<totalGutValue){
			subGutValue = Mathf.Clamp(Random.Range(1,4),1,totalGutValue-gutValue);
			gutIndices[j] = ConvertGutValueToIndex(subGutValue);
			gutValue += subGutValue;
			j++;
		}
		gutIndices = gutIndices.Where (number => number != -1).ToArray ();
		foreach (int i in gutIndices){
			GameObject gut = Instantiate (gutSplosions[i],new Vector2 (Random.insideUnitCircle.x,Random.insideUnitCircle.y) * .2f + (Vector2)transform.position,Quaternion.identity) as GameObject;
			gut.GetComponent<Rigidbody2D>().velocity = new Vector2 (Random.Range(gutDirection.x * .1f,gutDirection.x * .4f),Random.Range(3f,8f));
			gut.transform.parent = transform;
		}
	}

	int ConvertGutValueToIndex(int subGutValue){
		switch (subGutValue){
		case 1:
			return 0;
		case 2:
			return Random.Range(1,5);
		case 3:
			return Random.Range(5,7);
		}
		return 0;
	}
}
