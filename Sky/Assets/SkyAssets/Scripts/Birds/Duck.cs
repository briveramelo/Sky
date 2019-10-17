using UnityEngine;
using GenericFunctions;

public interface ILeaderToDuck {
	void Scatter();
	int FormationIndex{get;set;}
}
// The Duck receives communication from the DuckLeader through this interface

public enum DuckDirection{
	UpRight=0,
	UpLeft=1,
	DownRight=2,
	DownLeft=3
}

public interface IDirectable{
	void SetDuckDirection(DuckDirection scatterDirection);
}

public class Duck : Bird, ILeaderToDuck, IDirectable {
	// The Duck will follow his/her DuckLeader, until the DuckLeader dies. 
	// Then, the Duck will aimlessly bounce around the screen until killed

	[SerializeField] LeadDuck leaderScript; IDuckToLeader leader;
	Transform myFormationTransform;

	Vector2[] scatterDir = new Vector2[]{
		new Vector2 (1,1).normalized,
		new Vector2 (-1,1).normalized,
		new Vector2 (1,-1).normalized,
		new Vector2 (-1,-1).normalized,
		new Vector2 (1,1).normalized,
		new Vector2 (-1,1).normalized
	};
		
	private Vector2 CurrentVelocity{
		set{rigbod.velocity = value;
			transform.FaceForward(rigbod.velocity.x<0);
		}
	}
	const float moveSpeed = 2.5f;
	const float maxSpeed = 4f;
	int formationIndex;
	bool bouncing;

	protected override void Awake () {
		base.Awake();
		if (transform.parent) {
			leader = leaderScript;
		}
		else{
			Scatter();
		}
	}

	void Update(){
		if (bouncing){
			BounceOnTheWalls ();
		}
		else{
			StayInFormation();
		}
	}

	// This function restricts a duck's movement to the confines of the screen- an omage to DuckHunt
	// Although it seems that flipping the sign of the duck's x or y velocity when the duck's position exceed the WorldDimensions
	// would eliminate extra lines of code, this is not the case.
	// In that scenario, the duck often travels far beyond the WorldDimension, reversing direction and velocity,
	// only to be trapped flipping its velocity each frame for some time. 
	// This solution eliminates that concern.
	void BounceOnTheWalls(){
		bool overX = transform.position.x> Constants.WorldDimensions.x;
		bool underX= transform.position.x<-Constants.WorldDimensions.x;
		bool overY = transform.position.y> Constants.WorldDimensions.y;
		bool underY= transform.position.y<-Constants.WorldDimensions.y;
		if (overX || underX || overY || underY){
			CurrentVelocity = new Vector2 (
				underX ? 1 : overX ? -1 : Mathf.Sign(rigbod.velocity.x),
				underY ? 1 : overY ? -1 : Mathf.Sign(rigbod.velocity.y)).normalized * moveSpeed;
		}
	}

	void StayInFormation(){
		transform.position = Vector3.MoveTowards(transform.position, myFormationTransform.position, maxSpeed * Time.deltaTime);
	}

	#region IDirectable
	void IDirectable.SetDuckDirection(DuckDirection scatterDirection){
		CurrentVelocity = scatterDir[(int)scatterDirection] * moveSpeed;
	}
	#endregion

	#region ILeaderToDuck
	void ILeaderToDuck.Scatter(){
        ScoreSheet.Tallier.TallyThreat(Threat.FreeDuck);
        Scatter();
	}
    void Scatter() {
        CurrentVelocity = scatterDir[formationIndex] * moveSpeed;
        birdStats.ModifyForEvent(3);
        bouncing = true;
    }
	int ILeaderToDuck.FormationIndex {
		get => formationIndex;
		set{formationIndex = value;
			myFormationTransform = leader.FormationTransforms[formationIndex];
		}
	}
	#endregion

	// Remember that final action some "Bird"s need to perform?
	// When a Duck with a DuckLeader dies, he/she lets the DuckLeader know to reorganize the Flying V formation
	protected override void DieUniquely (){
		if (leaderScript){
			leader.OrganizeDucks(this);
		}
		base.DieUniquely();
	}
}