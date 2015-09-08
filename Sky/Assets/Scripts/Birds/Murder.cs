using UnityEngine;
using System.Collections;

public class Murder : MonoBehaviour {

	public GameObject[] crowSticks;
	public Vector3[] crowStartingPositions;
	public Crow[] crowScripts;
	public int i;
	public bool[] crowHasGone;
	public float switchTime;
	public int cycles;
	public int maxCycles;
	public int numGone;
	public bool switchCrows;

	// Use this for initialization
	void Awake () {
		crowSticks = new GameObject[]{
			transform.GetChild (0).gameObject,
			transform.GetChild (1).gameObject,
			transform.GetChild (2).gameObject,
			transform.GetChild (3).gameObject,
			transform.GetChild (4).gameObject,
			transform.GetChild (5).gameObject
		};

		crowScripts = new Crow[crowSticks.Length];
		crowStartingPositions = new Vector3[crowSticks.Length];
		int j = 0;
		foreach (GameObject crow in crowSticks){
			crowScripts[j] = crow.transform.GetChild(0).GetComponent<Crow>();
			j++;
		}
		j = 0;
		foreach (Crow crowScript in crowScripts){
			crowScript.crowNumber = j;
			j++;
		}

		j = 0;
		foreach (GameObject crow in crowSticks){
			crowStartingPositions[j] = crow.transform.position;
			j++;
		}

		switchTime = 4f;
		crowHasGone = new bool[6];
		i = Random.Range (0,6);
		cycles = 0;
		maxCycles = 200;
		StartCoroutine(ChooseNextCrow ());
	}

	public IEnumerator ChooseNextCrow(){
		FindNumberOfCrowsThatHaveGone ();
		int count = 0;
		while (crowHasGone[i]){
			i = Random.Range (0,6);
			count++;
			if (count>100){
				StartCoroutine (ResetTheCycle ());
			}
		}
		crowHasGone[i] = true;
		crowScripts [i].swooping = true;
		FindNumberOfCrowsThatHaveGone ();
		switchCrows = false;
		//yield return StartCoroutine (SwitchCrowPause ());
		while (crowScripts[i].transform.rotation.eulerAngles.z<350f){
			yield return null;
		}
		crowScripts [i].targetAngle = 0;
		crowScripts [i].stick.rotation = Quaternion.identity;
		crowScripts [i].swooping = false;
		StartCoroutine (ResetTheCycle ());


		yield return null;
	}

	public IEnumerator SwitchCrowPause(){
		yield return new WaitForSeconds (5f);
		switchCrows = true;
	}

	void FindNumberOfCrowsThatHaveGone(){
		numGone = 0;
		foreach (bool gone in crowHasGone){
			if (gone){
				numGone++;
			}
		}
	}
	
	public IEnumerator ResetTheCycle(){
		if (numGone == crowHasGone.Length-1){ //reset the cycle
			crowHasGone = new bool[6];
			i = Random.Range (0,6);
			cycles++;
		}
		if (cycles<maxCycles){
			StartCoroutine (ChooseNextCrow());
		}
		else{
			Destroy(gameObject);
		}
		yield return null;
	}
}
