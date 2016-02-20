using UnityEngine;
using System.Collections;
using GenericFunctions;

public class DuckLeader : Bird {

	[SerializeField] private Duck[] duckScripts;

	private Vector2[] setPositions; public Vector2[] SetPositions {get{return setPositions;}}
	private bool[] formations;

	private float moveSpeed = 2.5f;
	private float separationDistance = 0.9f;

	// Use this for initialization
	void Awake () {
		birdStats = new BirdStats(BirdType.DuckLeader);

		SetFormation (transform.position.x > 0);
		FanTheV ();
	}
	
	public void SetFormation(bool goLeft){
		Vector3 topSide;
		Vector3 bottomSide;
		Vector3 direction;
		if (goLeft){
			topSide = ConvertAnglesAndVectors.ConvertAngleToVector2 (30);
			bottomSide = ConvertAnglesAndVectors.ConvertAngleToVector2 (-30);
			direction = Vector2.left;
		}
		else{
			topSide = ConvertAnglesAndVectors.ConvertAngleToVector2 (150);
			bottomSide = ConvertAnglesAndVectors.ConvertAngleToVector2 (210);
			direction = Vector2.right;
		}
		transform.Face4ward(goLeft);
		rigbod.velocity = direction * moveSpeed;
		setPositions = new Vector2[]{
			topSide,
			bottomSide,
			topSide * 2f,
			bottomSide * 2f,
			topSide * 3f,
			bottomSide * 3f
		};
		
		formations = new bool[6];
		for (int i=0; i<duckScripts.Length; i++){
			duckScripts[i].FormationNumber = i;
			duckScripts[i].DuckLeaderScript = this;
			setPositions[i] *= separationDistance;
			formations[i] = true;
		}
	}

	void FanTheV(){
		foreach (Duck duck in duckScripts){
			if (duck){
				duck.Bouncing = false;
			}
		}
	}

	public void ReShuffle(int formToFill){
		switch (formToFill){
		case 0:
			if (formations[2]){
				Shuffle (formToFill,2);
				if (formations[4]){
					Shuffle(2,4);
				}
				else if (formations[5]){
					Shuffle (2,5);
				}
			}
			else if (formations[3]){
				Shuffle (formToFill,3);
				if (formations[5]){
					Shuffle (3,5);
				}
			}
			break;
		case 1:
			if (formations[3]){
				Shuffle (formToFill,3);
				if (formations[5]){
					Shuffle(3,5);
				}
				else if (formations[4]){
					Shuffle (3,4);
				}
			}
			else if (formations[2]){
				Shuffle (formToFill,2);
				if (formations[4]){
					Shuffle (2,4);
				}
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
		case 4:
			KillOff(formToFill);
			break;
		case 5:
			KillOff(formToFill);
			break;
		}
	}

	/// <summary>Flying V formation- takes a dead duck's formation number (int)formToFill
	/// <para> and gives it to the duck of your choice (int)formMovingUp </para>
	/// </summary>
	void Shuffle(int formToFill, int formMovingUp){
		duckScripts[formMovingUp].FormationNumber = formToFill;
		duckScripts[formToFill] = duckScripts[formMovingUp];
		duckScripts[formMovingUp] = null;
		formations [formToFill] = true;
		formations [formMovingUp] = false;
	}

	void KillOff(int formNumb){
		duckScripts[formNumb] = null;
		formations [formNumb] = false;
	}

	public void BreakTheV(){
		transform.DetachChildren ();
		foreach (Duck duck in duckScripts){
			if (duck){
				duck.Scatter();
			}
		}
	}

	protected override void PayTheIronPrice (){
		BreakTheV();
	}
}
