using UnityEngine;
using System.Collections;
using GenericFunctions;
public class BasketEngine : TouchInput {

	[SerializeField] private Rigidbody2D basketBody;
	const float moveSpeed = 2.7f;

	protected override void OnTouchBegin(int fingerID){
		float distFromStick = Vector2.Distance(touchSpot,startingJoystickSpot);
		if (distFromStick<joystickMaxStartDist){
			myFingerID = fingerID;
		}
	}

	protected override void OnTouchMovedStationary(int fingerID){
		if (fingerID == myFingerID){
			Vector2 moveDir = Vector2.ClampMagnitude(touchSpot - startingJoystickSpot,joystickMaxMoveDistance);
			basketBody.velocity = moveDir * moveSpeed;
		}
	}

	protected override void OnTouchEnd(int fingerID){
		if (fingerID == myFingerID){
			myFingerID = -1;
		}
	}
}
