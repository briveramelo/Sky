using UnityEngine;
using System.Collections;

public class Duck : MonoBehaviour {

	public Rigidbody2D rigbod;
	public DuckLeader duckLeaderScript;

	public Vector3 pixelScale;
	public Vector3 pixelScaleReversed;
	public Vector3 targetSpot;

	public Vector2[] chooseDir;
	public Vector2[] scatterDir;
	public Vector2 moveDir;
	
	public float moveSpeed;
	public float transitionSpeed;

	public int formationNumber;

	public bool bouncing;


	// Use this for initialization
	void Awake () {
		pixelScale = Vector3.one * 3.125f;
		pixelScaleReversed = new Vector3 (-3.125f,3.125f,1f);
		moveSpeed = 2f;
		transitionSpeed = 2.5f;
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
		moveDir = chooseDir [0];
		if (!transform.parent){
			formationNumber = Random.Range(0,4);
			StartCoroutine(Scatter());
		}
	}

	void Update(){
		if (bouncing)
		{
			VelocityBouncer ();
		}
		else{
			StayInFormation();
		}
	}

	void StayInFormation(){
		targetSpot = duckLeaderScript.setPositions [formationNumber] + duckLeaderScript.transform.position;
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
			transform.localScale = pixelScale;
		}
		else if (transform.position.x<-8.8f){
			rigbod.velocity = new Vector2 (moveDir.x, rigbod.velocity.y);
			transform.localScale = pixelScaleReversed;
		}
	}
}
