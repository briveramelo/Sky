using UnityEngine;
using System.Collections;
using GenericFunctions;

public class Duck : Bird {

	private DuckLeader duckLeaderScript; public DuckLeader DuckLeaderScript{get{return duckLeaderScript;}set{duckLeaderScript = value;}}

	private Vector3 targetSpot;

	private Vector2[] chooseDir = new Vector2[]{
		new Vector2 (1,1).normalized,
		new Vector2 (-1,1).normalized,
		new Vector2 (1,-1).normalized,
		new Vector2 (-1,-1).normalized
	};

	private Vector2[] scatterDir;

	private Vector2 moveDir;
	
	private float moveSpeed = 2.5f;
	private float maxTransition = 4f;
	private float transitionSpeed;
	private float distanceToSpot;

	private int formationNumber; public int FormationNumber {get{return formationNumber;}set{formationNumber = value;}}

	private bool bouncing; public bool Bouncing {set{bouncing = value;}}

	void Start () {
		birdStats = new BirdStats(BirdType.Duck);
		scatterDir = new Vector2[]{
			chooseDir [0],
			chooseDir [1],
			chooseDir [2],
			chooseDir [3],
			chooseDir [0],
			chooseDir [1]
		};
		moveDir = chooseDir [0] * moveSpeed;
		if (!transform.parent) {
			bouncing = true;
			rigbod.velocity = chooseDir[0] * moveSpeed;
			birdStats.KillPointValue = 3;
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
		targetSpot = (Vector3)duckLeaderScript.SetPositions [formationNumber] + duckLeaderScript.transform.position;
		distanceToSpot = Vector3.Distance (targetSpot, transform.position);
		transitionSpeed = Mathf.Clamp (4*Mathf.Pow (10,distanceToSpot), moveSpeed + 0.5f, maxTransition);
		transform.position = Vector3.MoveTowards (transform.position, targetSpot, transitionSpeed * Time.deltaTime);
	}

	public void Scatter(){
		rigbod.velocity = scatterDir[formationNumber] * moveSpeed;
		birdStats.KillPointValue = 3;
		bouncing = true;
	}

	protected override void PayTheIronPrice (){
		if (duckLeaderScript){
			duckLeaderScript.ReShuffle(formationNumber);
		}
	}
}
