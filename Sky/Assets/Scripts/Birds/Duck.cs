using UnityEngine;
using System.Collections;
using GenericFunctions;

public class Duck : Bird, ILeaderToDuck {

	[SerializeField] private DuckLeader duckLeaderScript; private IDuckToLeader duckToLeader;

	private Vector2[] scatterDir = new Vector2[]{
		new Vector2 (1,1).normalized,
		new Vector2 (-1,1).normalized,
		new Vector2 (1,-1).normalized,
		new Vector2 (-1,-1).normalized,
		new Vector2 (1,1).normalized,
		new Vector2 (-1,1).normalized
	};
		
	private Vector2 moveDir;
	
	private float moveSpeed = 2.5f;
	private float maxTransition = 4f;

	private int formationNumber;

	private bool bouncing;

	protected override void Awake () {
		birdStats = new BirdStats(BirdType.Duck);
		duckToLeader = (IDuckToLeader)duckLeaderScript;

		moveDir = scatterDir [0] * moveSpeed;
		if (!transform.parent) {
			bouncing = true;
			rigbod.velocity = scatterDir[0] * moveSpeed;
			birdStats.KillPointValue = 3;
		}
		base.Awake();
	}

	void Update(){
		if (bouncing){
			BounceOnTheWalls ();
		}
		else{
			StayInFormation();
		}
	}

	void BounceOnTheWalls(){
		if (transform.position.y>Constants.worldDimensions.y){
			rigbod.velocity = new Vector2 (rigbod.velocity.x, -moveDir.y);
		}
		else if (transform.position.y<-Constants.worldDimensions.y){
			rigbod.velocity = new Vector2 (rigbod.velocity.x, moveDir.y);
		}
		else if (transform.position.x>Constants.worldDimensions.x){
			rigbod.velocity = new Vector2 (-moveDir.x, rigbod.velocity.y);
		}
		else if (transform.position.x<-Constants.worldDimensions.x){
			rigbod.velocity = new Vector2 (moveDir.x, rigbod.velocity.y);
		}
		transform.Face4ward(rigbod.velocity.x<0);
	}

	void StayInFormation(){
		Vector3 targetSpot = (Vector3)duckToLeader.SetPositions [formationNumber] + duckLeaderScript.transform.position;
		float distanceToSpot = Vector3.Distance (targetSpot, transform.position);
		float transitionSpeed = Mathf.Clamp (4*Mathf.Pow (10,distanceToSpot), moveSpeed + 0.5f, maxTransition);
		transform.position = Vector3.MoveTowards (transform.position, targetSpot, transitionSpeed * Time.deltaTime);
	}

	#region ILeaderToDuck
	int ILeaderToDuck.FormationNumber {get{return formationNumber;}set{formationNumber = value;}}
	bool ILeaderToDuck.Bouncing {set{bouncing = value;}}
	void ILeaderToDuck.Scatter(){
		rigbod.velocity = scatterDir[formationNumber] * moveSpeed;
		birdStats.KillPointValue = 3;
		bouncing = true;
	}
	#endregion

	protected override void PayTheIronPrice (){
		if (duckLeaderScript){
			duckToLeader.ReShuffle(this);
		}
	}
}
