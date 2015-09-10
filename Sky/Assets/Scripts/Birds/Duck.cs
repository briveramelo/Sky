using UnityEngine;
using System.Collections;

public class Duck : MonoBehaviour {

	public Rigidbody2D rigbod;
	public DuckLeader duckLeaderScript;

	public Vector3 pixelScale;
	public Vector3 pixelScaleReversed;

	public Vector2[] chooseDir;
	public Vector2 moveDir;

	public float moveSpeed;
	public float distanceToSpot;

	public int formationNumber;

	public bool bouncing;
	public bool allGood;


	// Use this for initialization
	void Awake () {
		pixelScale = Vector3.one * 3.125f;
		pixelScaleReversed = new Vector3 (-3.125f,3.125f,1f);
		moveSpeed = 2f;
		rigbod = GetComponent<Rigidbody2D> ();
		distanceToSpot = 10f;
		allGood = true;
		chooseDir = new Vector2[]
		{
			new Vector2 (1,1).normalized,
			new Vector2 (-1,1).normalized,
			new Vector2 (1,-1).normalized,
			new Vector2 (-1,-1).normalized
		};
		if (!transform.parent){
			StartCoroutine(Scatter());
		}
	}

	void Update(){
		if (bouncing)
		{
			VelocityBouncer ();
		}
	}

	public IEnumerator GetInTheV(){
		while (allGood) {
			distanceToSpot = Vector3.Distance(transform.position,duckLeaderScript.setPositions[formationNumber] + duckLeaderScript.transform.position);
			rigbod.velocity = (duckLeaderScript.setPositions[formationNumber] + duckLeaderScript.transform.position - transform.position).normalized * moveSpeed;
			yield return null;
		}

	}

	public IEnumerator Scatter(){
		rigbod.velocity = chooseDir[Random.Range(0,5)] * moveSpeed;
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
