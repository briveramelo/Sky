using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GenericFunctions;

public class DuckLeader : Bird, IDuckToLeader {

	[SerializeField] private Duck[] duckScripts; private List<ILeaderToDuck> leaderToDucks;

	private Vector2[] setPositions;
	private float moveSpeed = 2.5f;

	protected override void Awake () {
		birdStats = new BirdStats(BirdType.DuckLeader);
		leaderToDucks = new List<ILeaderToDuck>((ILeaderToDuck[])duckScripts);
		SetFormation (transform.position.x > 0);
		FanTheV ();
		base.Awake();
	}
	
	public void SetFormation(bool goLeft){
		Vector3 topSide = ConvertAnglesAndVectors.ConvertAngleToVector2 (goLeft ? 30 : 150);
		Vector3 bottomSide = ConvertAnglesAndVectors.ConvertAngleToVector2 (goLeft ? -30 : 210);
		Vector2 direction = new Vector2 (goLeft ? -1 : 1, 0);
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

		float separationDistance = 0.9f;
		for (int i=0; i<leaderToDucks.Count; i++){
			leaderToDucks[i].FormationNumber = i;
			setPositions[i] *= separationDistance;
		}
	}

	void FanTheV(){
		leaderToDucks.ForEach (duck => duck.Bouncing = false);
	}

	#region IDuckToLeader
	Vector2[] IDuckToLeader.SetPositions {get{return setPositions;}}
	void IDuckToLeader.ReShuffle(ILeaderToDuck deadDuck){
		int deadFormNum = deadDuck.FormationNumber;
		int odds=0;	int evens=0;

		for (int i=0; i<leaderToDucks.Count; i++){
			evens += leaderToDucks[i].FormationNumber % 2 ==0 ? 1:0;
			odds += leaderToDucks[i].FormationNumber % 2 !=0 ? 1:0;

			if (leaderToDucks[i].FormationNumber>deadFormNum && leaderToDucks[i].FormationNumber % 2 == deadFormNum % 2){
				leaderToDucks[i].FormationNumber -= 2;
			}
		}

		if (odds<evens && deadFormNum % 2 != 0){
			int highestEven = (evens-1)*2;
			leaderToDucks.Find(duck => duck.FormationNumber==highestEven).FormationNumber -= 1;
		}
		else if (evens<odds && deadFormNum % 2 == 0){
			int highestOdd = odds*2-1;
			leaderToDucks.Find(duck => duck.FormationNumber==highestOdd).FormationNumber -= 3;
		}

		leaderToDucks.Remove(deadDuck);
	}
	#endregion

	void BreakTheV(){
		transform.DetachChildren ();
		leaderToDucks.ForEach(duck => duck.Scatter());
	}

	protected override void PayTheIronPrice (){
		BreakTheV();
	}
}