using UnityEngine;
using System.Collections;
using GenericFunctions;

public class Duck : MonoBehaviour {

	public Rigidbody2D rigbod;
	public DuckLeader duckLeaderScript;

	public Vector3 targetSpot;

	public Vector2[] chooseDir;
	public Vector2[] scatterDir;
	public Vector2 moveDir;
	
	public float moveSpeed;
	public float maxTransition;
	public float transitionSpeed;
	public float distanceToSpot;

	public int formationNumber;

	public bool bouncing;


	// Use this for initialization
	void Awake () {
		moveSpeed = 2.5f;
		maxTransition = 4f;
		rigbod = GetComponent<Rigidbody2D> ();
		chooseDir = new Vector2[]
		{
			new Vector2 (1,1).normalized,
			new Vector2 (-1,1).normalized,
			new Vector2 (1,-1).normalized,
			new Vector2 (-1,-1).normalized
		};
		scatterDir = new Vector2[]{
			chooseDir [0],
			chooseDir [1],
			chooseDir [2],
			chooseDir [3],
			chooseDir [0],
			chooseDir [1]
		};
		moveDir = chooseDir [0] * moveSpeed;
		if (!transform.parent){
			formationNumber = Random.Range(0,4);
			StartCoroutine(Scatter());
		}
	}

	void Update(){
		if (bouncing)
		{
			VelocityBouncer ();
			AnimateBouncing();
		}
		else{
			StayInFormation();
			AnimateInFormation ();
		}

	}

	void AnimateBouncing(){
		if (rigbod.velocity.x>0){
			transform.localScale = Constants.Pixel625(false);
		}
		else {
			transform.localScale = Constants.Pixel625(true);
		}
	}

	void AnimateInFormation(){
		if (rigbod.velocity.x>0){
			transform.localScale = Constants.Pixel1(false);
		}
		else {
			transform.localScale = Constants.Pixel1(true);
		}
	}

	void StayInFormation(){
		targetSpot = duckLeaderScript.setPositions [formationNumber] + duckLeaderScript.transform.position;
		distanceToSpot = Vector3.Distance (targetSpot, transform.position);
		transitionSpeed = Mathf.Clamp (4*Mathf.Pow (10,distanceToSpot), moveSpeed + 0.5f, maxTransition);
		transform.position = Vector3.MoveTowards (transform.position, targetSpot, transitionSpeed * Time.deltaTime);
	}

	public IEnumerator Scatter(){
		rigbod.velocity = scatterDir[formationNumber] * moveSpeed;
		bouncing = true;
		yield return null;
	}

	void VelocityBouncer(){
		if (transform.position.y>5){
			rigbod.velocity = new Vector2 (rigbod.velocity.x, -moveDir.y);
		}
		else if (transform.position.y<-5f){
			rigbod.velocity = new Vector2 (rigbod.velocity.x, moveDir.y);
		}
		else if (transform.position.x>8.8f){
			rigbod.velocity = new Vector2 (-moveDir.x, rigbod.velocity.y);
			transform.localScale = Constants.Pixel625(true);
		}
		else if (transform.position.x<-8.8f){
			rigbod.velocity = new Vector2 (moveDir.x, rigbod.velocity.y);
			transform.localScale = Constants.Pixel625(false);
		}
	}
}
