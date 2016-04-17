using UnityEngine;
using System.Collections.Generic;
using GenericFunctions;

public interface IDuckToLeader {
	void OrganizeDucks(ILeaderToDuck deadDuck);
	Transform[] FormationTransforms{get;}
}
// The DuckLeader communicates with Ducks through this interface

public class DuckLeader : Bird, IDuckToLeader {
	// The DuckLeader ensures all ducks follow as closely behind in an evenly distributed Flying V Formation
	// The DuckLeader will fly linearly across the screen

	[SerializeField] private Duck[] duckScripts; private List<ILeaderToDuck> ducks;
	[SerializeField] private Transform[] formationTransforms;

	protected override void Awake () {
		birdStats = new BirdStats(BirdType.DuckLeader);
		ducks = new List<ILeaderToDuck>((ILeaderToDuck[])duckScripts);
		bool goLeft = transform.position.x > 0;
		transform.FaceForward(goLeft);
		rigbod.velocity = new Vector2 (goLeft ? -1 : 1, 0) * 2.5f;
		SetDuckFormation (goLeft);
		base.Awake();
	}

	// Set ducks into the "Flying V" Formation, like so:
	//	  [4]
	//	  	[2]
	//	  	  [0]
	//  		[leader]  --->
	// 	  	  [1]
	//		[3]
	//	  [5]

	void SetDuckFormation(bool goLeft){
		Vector2 topSide = ConvertAnglesAndVectors.ConvertAngleToVector2 (goLeft ? 30 : 150);
		Vector2 bottomSide = ConvertAnglesAndVectors.ConvertAngleToVector2 (goLeft ? -30 : 210);

		float separationDistance = 0.225f;
		for (int i=0; i<formationTransforms.Length; i++){
			formationTransforms[i].localPosition = (goLeft ? 1:-1) * (i%2==0 ? topSide : bottomSide) * (Mathf.Floor(i/2)+1) * separationDistance;
			ducks[i].FormationIndex = i;
		}
	}

	#region IDuckToLeader
	Transform[] IDuckToLeader.FormationTransforms {get{return formationTransforms;}}

	// Review the above "Flying V" Formation and the attached video "FlyingV_United" to see this logic in action
	void IDuckToLeader.OrganizeDucks(ILeaderToDuck deadDuck){
		int deadNumber = deadDuck.FormationIndex;
		int topCount=0;
		int bottomCount=0;

		for (int i=0; i<ducks.Count; i++){
			topCount += ducks[i].FormationIndex % 2 ==0 ? 1:0; 	//birds on top use even indices
			bottomCount += ducks[i].FormationIndex % 2 !=0 ? 1:0;	//		bottom use odd indices

			if (ducks[i].FormationIndex>deadNumber && ducks[i].FormationIndex % 2 == deadNumber % 2){
				ducks[i].FormationIndex -= 2;
			}
		}

		if (topCount<bottomCount && deadNumber % 2 == 0){
			int highestOdd = bottomCount*2-1;
			ducks.Find(duck => duck.FormationIndex==highestOdd).FormationIndex -= 3;
		}
		else if (bottomCount<topCount && deadNumber % 2 != 0){
			int highestEven = (topCount-1)*2;
			ducks.Find(duck => duck.FormationIndex==highestEven).FormationIndex -= 1;
		}

		ducks.Remove(deadDuck);
	}
	#endregion

	// Remember that final action some "Bird"s need to perform?
	// The DuckLeader Breaks the Flying V Formation and tells each Duck to Scatter when he/she dies
	protected override void DieUniquely (){
		BreakTheV();
		base.DieUniquely();
	}

	void BreakTheV(){
		transform.DetachChildren ();
		ducks.ForEach(duck => duck.Scatter());
		for (int i=0; i<formationTransforms.Length; i++){
			Destroy(formationTransforms[i].gameObject);
		}
	}
}