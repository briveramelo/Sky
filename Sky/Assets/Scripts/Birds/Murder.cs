using UnityEngine;
using System.Collections;
using System.Linq;

public class Murder : MonoBehaviour {

	public Crow[] crowScripts;
	public Vector3[] crowPositions;
	public int i;
	public int[] crowsToGo;
	public float switchTime;
	public int cycles;
	public int maxCycles;
	public int numGone;

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

		crowPositions = new Vector3[]{
			new Vector3(0f   ,  7f , 0f),
			new Vector3(9.2f ,  5f , 0f),
			new Vector3(9f   , -5f , 0f),
			new Vector3(0f   , -7f , 0f),
			new Vector3(-9.2f, -5f , 0f),
			new Vector3(-9.2f,  5f , 0f),
		};

		int j = 0;
		foreach (Crow crowScript in crowScripts){
			crowScript.crowNumber = j;
			crowScript.transform.position = crowPositions[j];
			j++;
		}

		switchTime = 4f;
		crowsToGo = new int[]{
			0,1,2,3,4,5
		};
		i = Random.Range (0,6);
		cycles = 0;
		maxCycles = 3;
		StartCoroutine(ChooseNextCrow (i));
	}

	public IEnumerator ChooseNextCrow(int crowToGo){
		crowsToGo = crowsToGo.Where(number => number!=crowToGo).ToArray();
		crowScripts [crowToGo].swooping = true;
		numGone = 6-crowsToGo.Length;
		if (numGone==2){
			crowScripts[crowsToGo[Random.Range(0,crowsToGo.Length)]].isKiller = true;
		}
		while (!crowScripts [crowToGo].turned){
			yield return null;
		}
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
		if (crowsToGo.Length>0){
			i = crowsToGo[Random.Range (0,crowsToGo.Length)];
		}
		else{
			Destroy(gameObject);
		}
		cycles++;

		yield return new WaitForSeconds (3f);

		if (cycles<maxCycles){
			StartCoroutine (ChooseNextCrow(i));
		}
		else{
			Destroy(gameObject,5f);
		}
		yield return null;
	}
}
