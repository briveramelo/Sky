﻿using UnityEngine;
using System.Collections;
using GenericFunctions;

public class TeleportSideToSide : MonoBehaviour {

	[SerializeField] private Collider2D buddyCollider;
	[SerializeField] private Teleporter TeleporterType;
	private Vector2 destination;
	private enum Teleporter{
		Pigeon,
		DuckLeader
	}

	void Awake () {
		destination = new Vector2 (buddyCollider.transform.position.x, 0f);
	}

	void OnTriggerEnter2D(Collider2D col){
		if (TeleporterType == Teleporter.Pigeon){
			if (col.gameObject.GetComponent<Pigeon>()){//teleport pigeons across sides
				StartCoroutine (TemporaryTeleport(col));
			}
		}
		if (TeleporterType == Teleporter.DuckLeader){
			if (col.gameObject.GetComponent<DuckLeader>()){//teleport duck squads across sides
				StartCoroutine (TemporaryTeleport(col));
			}
		}
	}
	
	IEnumerator TemporaryTeleport(Collider2D col){
		col.gameObject.transform.position = destination + Vector2.up * col.gameObject.transform.position.y;
		Physics2D.IgnoreCollision (col, buddyCollider, true);
		yield return new WaitForSeconds (3f);
		Physics2D.IgnoreCollision (col, buddyCollider, false);
	}
}