using UnityEngine;
using System.Collections;
using GenericFunctions;

public class DuckLeader : MonoBehaviour {

	public Duck[] duckScripts;
	public Vector3[] setPositions;
	public bool[] formations;
	public float separationDistance;

	// Use this for initialization
	void Awake () {
		separationDistance = .2f;
		duckScripts = new Duck[]{
			transform.GetChild(0).GetComponent<Duck>(),
			transform.GetChild(1).GetComponent<Duck>(),
			transform.GetChild(2).GetComponent<Duck>(),
			transform.GetChild(3).GetComponent<Duck>(),
			transform.GetChild(4).GetComponent<Duck>(),
			transform.GetChild(5).GetComponent<Duck>(),
			transform.GetChild(6).GetComponent<Duck>()
		};

		Vector3 topSide = ConvertAnglesAndVectors.ConvertAngleToVector3 (150);
		Vector3 bottomSide = ConvertAnglesAndVectors.ConvertAngleToVector3 (210);

		setPositions = new Vector3[]
		{
			topSide,
			bottomSide,
			topSide * 2f,
			bottomSide * 2f,
			topSide * 3f,
			bottomSide * 3f
		};
		int i = 0;
		foreach (Duck duck in duckScripts){
			duck.formationNumber = i;
			duck.transform.position = transform.position - Vector3.right * separationDistance * i;
			duck.duckLeaderScript = this;
			setPositions[i] *= separationDistance;
			i++;
		}
		StartCoroutine (FanTheV ());
	}

	public IEnumerator FanTheV(){
		foreach (Duck duck in duckScripts){
			if (duck){
				StartCoroutine(duck.GetInTheV());
			}
		}
		yield return null;
	}

	public IEnumerator ReShuffle(int formToReplace){
		switch (formToReplace){
		case 2:
			/*if (formations[4-1]){
				duckScripts[3].formationNumber = formToReplace;
				if (
			}*/
			break;
		case 3:
			break;
		case 4:
			break;
		case 5:
			break;
		case 6:
			break;
		case 7:
			break;
		}
		yield return null;
	}

	public IEnumerator BreakTheV(){
		foreach (Duck duck in duckScripts){
			if (duck){
				StartCoroutine(duck.Scatter());
			}
		}
		yield return null;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
