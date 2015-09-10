using UnityEngine;
using System.Collections;
using GenericFunctions;

public class DuckLeader : MonoBehaviour {

	public Duck[] duckScripts;

	public Rigidbody2D rigbod;

	public Vector3[] setPositions;
	public bool[] formations;

	public float moveSpeed;
	public float separationDistance;

	// Use this for initialization
	void Awake () {
		separationDistance = .6f;
		duckScripts = new Duck[]{
			transform.GetChild(0).GetComponent<Duck>(),
			transform.GetChild(1).GetComponent<Duck>(),
			transform.GetChild(2).GetComponent<Duck>(),
			transform.GetChild(3).GetComponent<Duck>(),
			transform.GetChild(4).GetComponent<Duck>(),
			transform.GetChild(5).GetComponent<Duck>(),
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
		formations = new bool[6];
		int i = 0;
		foreach (Duck duck in duckScripts){
			duck.formationNumber = i;
			duck.transform.position = transform.position - Vector3.right * separationDistance * i;
			duck.duckLeaderScript = this;
			setPositions[i] *= separationDistance;
			formations[i] = true;

			i++;
		}
		Invoke ("Empower",.1f);
		rigbod = GetComponent<Rigidbody2D> ();
		moveSpeed = 2f;
		rigbod.velocity = Vector2.right * moveSpeed;
		StartCoroutine (FanTheV ());
	}

	void Empower(){
		transform.DetachChildren ();
	}

	public IEnumerator FanTheV(){
		foreach (Duck duck in duckScripts){
			if (duck){
				duck.bouncing = false;
			}
		}
		yield return null;
	}

	public IEnumerator ReShuffle(int formToFill){
		switch (formToFill){
		case 0:
			if (formations[2]){
				Shuffle (formToFill,2);
				if (formations[4]){
					Shuffle(2,4);
				}
			}
			else if (formations[5]){
				Shuffle (formToFill,3);
				Shuffle (3,5);
			}
			else if (formations[3]){
				Shuffle (formToFill,3);
			}
			break;
		case 1:
			if (formations[3]){
				Shuffle (formToFill,3);
				if (formations[5]){
					Shuffle(3,5);
				}
			}
			else if (formations[4]){
				Shuffle (formToFill,2);
				Shuffle (2,4);
			}
			else if (formations[2]){
				Shuffle (formToFill,2);
			}
			break;
		case 2:
			if (formations[4]){
				Shuffle (formToFill,4);
			}
			else if(formations[5]){
				Shuffle (formToFill,5);
			}
			break;
		case 3:
			if (formations[5]){
				Shuffle (formToFill,5);
			}
			else if(formations[4]){
				Shuffle (formToFill,4);
			}			
			break;
		}
		yield return null;
	}

	/// <summary>Flying V formation- takes a dead duck's formation number (int)formToFill
	/// <para> and gives it to the duck of your choice (int)formMovingUp </para>
	/// </summary>
	void Shuffle(int formToFill, int formMovingUp){
		duckScripts[formMovingUp].formationNumber = formToFill;
		duckScripts[formToFill] = duckScripts[formMovingUp];
		duckScripts[formMovingUp] = null;
		formations [formToFill] = true;
		formations [formMovingUp] = false;
	}

	public IEnumerator BreakTheV(){
		foreach (Duck duck in duckScripts){
			if (duck){
				StartCoroutine(duck.Scatter());
			}
		}
		yield return null;
	}
}
