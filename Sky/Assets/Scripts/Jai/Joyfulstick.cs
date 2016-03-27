using UnityEngine;
using System.Collections;
using GenericFunctions;

public class Joyfulstick : TouchInput {

	[SerializeField] private Transform controlStickTransform;

	void Awake () {
		transform.position = startingJoystickSpot;
		myFingerID = -1;
	}

	protected override  void OnTouchBegin(int fingerID){
		float distFromStick = Vector2.Distance(touchSpot,startingJoystickSpot);
		if (distFromStick<joystickMaxStartDist){
			myFingerID = fingerID;
			SetStickPosition();
		}
	}
	protected override  void OnTouchMovedStationary(int fingerID){
		if (fingerID == myFingerID){
			SetStickPosition();
		}
	}
	protected override  void OnTouchEnd(int fingerID){
		if (fingerID == myFingerID){
			controlStickTransform.transform.position = startingJoystickSpot;
			myFingerID = -1;
		}
	}
	void SetStickPosition(){
		Vector2 moveDir = Vector2.ClampMagnitude(touchSpot - startingJoystickSpot,joystickMaxMoveDistance);
		controlStickTransform.transform.position = startingJoystickSpot + moveDir;
	}
}
