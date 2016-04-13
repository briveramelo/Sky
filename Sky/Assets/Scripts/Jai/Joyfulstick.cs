using UnityEngine;
using System.Collections;
using GenericFunctions;

public class Joyfulstick : MonoBehaviour, IBegin, IHold, IEnd {

	public readonly static Vector2 startingJoystickSpot = new Vector2 (-Constants.WorldDimensions.x * (2f/3f),-Constants.WorldDimensions.y * (2f/5f));
	public const float joystickMaxStartDist = 1.25f;
	public const float joystickMaxMoveDistance = .75f; //maximum distance you can move the joystick
	private IStickEngineID inputManager;

	[SerializeField] private Transform controlStickTransform;
	void Awake () {
		transform.position = startingJoystickSpot;
		inputManager = FindObjectOfType<InputManager>().GetComponent<IStickEngineID>();
	}

	void IBegin.OnTouchBegin(int fingerID){
		float distFromStick = Vector2.Distance(InputManager.touchSpot,startingJoystickSpot);
		if (distFromStick<joystickMaxStartDist){
			inputManager.SetStickEngineID(fingerID);
			SetStickPosition();
		}
	}
	void IHold.OnTouchHeld(){
		SetStickPosition();
	}
	void IEnd.OnTouchEnd(){
		controlStickTransform.transform.position = startingJoystickSpot;
	}

	void SetStickPosition(){
		Vector2 moveDir = Vector2.ClampMagnitude(InputManager.touchSpot - startingJoystickSpot,joystickMaxMoveDistance);
		controlStickTransform.transform.position = startingJoystickSpot + moveDir;
	}
}
