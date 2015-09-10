using UnityEngine;
using System.Collections;

public class TeleportSideToSide : MonoBehaviour {

	public Collider2D buddyCollider;
	
	public Vector3 destination;

	// Use this for initialization
	void Start () {

		if (name.Contains("Right")){
			buddyCollider = GameObject.Find ("Teleport_LeftSide").GetComponent<BoxCollider2D>();
		}
		else if (name.Contains("Left")){
			buddyCollider = GameObject.Find ("Teleport_RightSide").GetComponent<BoxCollider2D>();
		}
		destination = new Vector3 (buddyCollider.transform.position.x, 0, 0);
	}
	
	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.GetComponent<Pigeon>()){//teleport pigeons across sides
			StartCoroutine (TemporaryTeleport(col));
		}
	}

	public IEnumerator TemporaryTeleport(Collider2D col){
		col.gameObject.transform.position = destination + Vector3.up * col.gameObject.transform.position.y;
		Physics2D.IgnoreCollision (col, buddyCollider, true);
		yield return new WaitForSeconds (2f);
		Physics2D.IgnoreCollision (col, buddyCollider, false);
	}
}
