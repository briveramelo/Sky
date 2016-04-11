using UnityEngine;
using System.Collections;
using GenericFunctions;

public interface IDisableable{
	void DisableMovement();
}

public class BasketEngine : TouchInput, IDisableable {

	[SerializeField] private Rigidbody2D basketBody;
	const float moveSpeed = 2.7f;
	bool movingEnabled =true;

	protected override void OnTouchBegin(int fingerID){
		float distFromStick = Vector2.Distance(touchSpot,startingJoystickSpot);
		if (distFromStick<joystickMaxStartDist){
			myFingerID = fingerID;
		}
	}

	protected override void OnTouchMovedStationary(int fingerID){
		if (movingEnabled){
			if (fingerID == myFingerID){
				Vector2 moveDir = Vector2.ClampMagnitude(touchSpot - startingJoystickSpot,joystickMaxMoveDistance);
				basketBody.velocity = moveDir * moveSpeed;
			}
		}
	}

	protected override void OnTouchEnd(int fingerID){
		if (fingerID == myFingerID){
			myFingerID = -1;
		}
	}

	void IDisableable.DisableMovement(){
		StopAllCoroutines();
		StartCoroutine (DisableMovement());
	}

	IEnumerator DisableMovement(){
		basketBody.velocity = Random.insideUnitCircle.normalized * 1.5f;
		yield return StartCoroutine (Bool.Toggle(boolState=>movingEnabled=boolState,.5f));
		Debug.Log(movingEnabled);
	}
}
