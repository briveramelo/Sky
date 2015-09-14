using UnityEngine;
using System.Collections;
using System.Linq;
using GenericFunctions;

public class Murder : MonoBehaviour {

	public Crow[] crowScripts;
	
	public float switchTime;

	public int[] crowsToGo;
	public int i;
	public int cycles;
	public int maxCycles;
	public int numGone;

	public bool killerIsAlive;
	public bool triggerNext;

	// Use this for initialization
	void Awake () {
		crowScripts = new Crow[]{
			transform.GetChild(0).GetComponent<Crow>(),
			transform.GetChild(1).GetComponent<Crow>(),
			transform.GetChild(2).GetComponent<Crow>(),
			transform.GetChild(3).GetComponent<Crow>(),
			transform.GetChild(4).GetComponent<Crow>(),
			transform.GetChild(5).GetComponent<Crow>()
		};

		int j = 0;
		foreach (Crow crowScript in crowScripts){
			crowScript.crowNumber = j;
			crowScript.transform.position = Spawn.crowPositions[j];
			crowScript.startPosition = Spawn.crowPositions[j];
			j++;
		}

		switchTime = 4f;
		crowsToGo = new int[]{
			0,1,2,3,4,5
		};
		killerIsAlive = true;
		i = Random.Range (0,6);
		cycles = 0;
		maxCycles = 3;
		StartCoroutine(ChooseNextCrow (i));
	}

	public IEnumerator ChooseNextCrow(int crowToGo){
		crowsToGo = crowsToGo.Where(number => number!=crowToGo).ToArray();
		crowScripts [crowToGo].swooping = true;
		crowScripts [crowToGo].crowCollider.enabled = true;
		numGone = 6-crowsToGo.Length;
		if (numGone==2 && killerIsAlive){
			crowScripts[crowsToGo[Random.Range(0,crowsToGo.Length)]].isKiller = true;
			//put on the right sprite here
		}
		while (!triggerNext){
			if (crowScripts[crowToGo]){
				triggerNext = crowScripts [crowToGo].triggeredNextCrow;
			}
			else{
				triggerNext = true;
			}
			yield return null;
		}
		triggerNext = false;
		if (numGone<6){
			i = crowsToGo[Random.Range (0,crowsToGo.Length)];
			StartCoroutine (ChooseNextCrow (i));
		}	
		else{
			StartCoroutine (ResetTheCycle ());
		}
		yield return null;
	}
	
	public IEnumerator ResetTheCycle(){
		crowsToGo = new int[6];
		int j=0;
		foreach (Crow crowScript in crowScripts){
			if (crowScript){
				crowsToGo[j] = j;
			}
			else{
				crowsToGo[j] = -1;
			}
			j++;
		}
		crowsToGo = crowsToGo.Where(number => number!=-1).ToArray();
		if (crowsToGo.Length<=0){
			Destroy(gameObject);
		}
		cycles++;

		yield return new WaitForSeconds (3f);
		i = crowsToGo[Random.Range (0,crowsToGo.Length)];
		if (cycles<maxCycles){
			StartCoroutine (ChooseNextCrow(i));
		}
		else{
			Destroy(gameObject,5f);
		}
		yield return null;
	}
}
