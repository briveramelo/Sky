using UnityEngine;
using System.Collections;
using System.Linq;
using GenericFunctions;


public interface IBleedable{
	void GenerateGuts(ref BirdStats birdStats, Vector2 gutDirection);
}
public class Guts : MonoBehaviour, IBleedable {

	[SerializeField] private GameObject[] gutSplosions;

	void Awake(){
		Destroy(gameObject,Constants.time2Destroy);
	}

	void IBleedable.GenerateGuts(ref BirdStats birdStats, Vector2 gutDirection){
		int totalGutValue = birdStats.GutsToSpill;
		int j = 0;
		int gutValue = 0;
		int subGutValue = 0;
		GameObject gut;
		while (gutValue<totalGutValue){
			subGutValue = Mathf.Clamp(Random.Range(1,4),1,totalGutValue-gutValue);
			gutValue += subGutValue;

			gut = Instantiate (gutSplosions[ConvertGutValueToIndex(subGutValue)],Random.insideUnitCircle.normalized * .2f + (Vector2)transform.position,Quaternion.identity) as GameObject;
			gut.GetComponent<Rigidbody2D>().velocity = new Vector2 (Random.Range(gutDirection.x * .1f,gutDirection.x * .4f),Random.Range(3f,8f));
			gut.transform.parent = transform;

			j++;
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
