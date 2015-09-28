using UnityEngine;
using System.Collections;
using GenericFunctions;

public class TeleportSideToSide : MonoBehaviour {

	public Collider2D buddyCollider;
	
	public Vector3 destination;
	public bool pigeons;
	public bool duckLeaders;
	
	// Use this for initialization
	void Awake () {
		if (name.Contains("Right")){
			buddyCollider = transform.parent.GetChild(1).GetComponent<BoxCollider2D>();
			transform.position = pigeons ? new Vector3 (Constants.worldDimensions.x * 1.1f,0f,0f) : new Vector3 (Constants.worldDimensions.x * 1.6f,0f,0f);
		}
		else if (name.Contains("Left")){
			buddyCollider = transform.parent.GetChild(0).GetComponent<BoxCollider2D>();
			transform.position = pigeons ? new Vector3 (-Constants.worldDimensions.x * 1.1f,0f,0f) : new Vector3 (-Constants.worldDimensions.x * 1.6f,0f,0f);
		}
		Invoke ("SetDestination", 0.1f);
	}

	void SetDestination (){
		destination = new Vector3 (buddyCollider.transform.position.x, 0, 0);
	}

	void OnTriggerEnter2D(Collider2D col){
		if (pigeons){
			if (col.gameObject.GetComponent<Pigeon>()){//teleport pigeons across sides
				StartCoroutine (TemporaryTeleport(col));
			}
		}
		if (duckLeaders){
			if (col.gameObject.GetComponent<DuckLeader>()){//teleport duck squads across sides
				StartCoroutine (TemporaryTeleport(col));
			}
		}
	}
	
	public IEnumerator TemporaryTeleport(Collider2D col){
		col.gameObject.transform.position = destination + Vector3.up * col.gameObject.transform.position.y;
		Physics2D.IgnoreCollision (col, buddyCollider, true);
		yield return new WaitForSeconds (3f);
		Physics2D.IgnoreCollision (col, buddyCollider, false);
	}
}
