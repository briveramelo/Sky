using UnityEngine;
using GenericFunctions;

public class Joyfulstick : MonoBehaviour, IBegin, IHold, IEnd {

    [SerializeField] Transform stickBase;

	public readonly static Vector2 startingJoystickSpot = new Vector2 (-Constants.WorldDimensions.x * (2f/3f),-Constants.WorldDimensions.y * (2f/5f));
	public const float joystickMaxStartDist = 1.25f;
	public const float joystickMaxMoveDistance = .75f; //maximum distance you can move the joystick
	private IStickEngineID inputManager;

	void Awake () {
        stickBase.position = startingJoystickSpot;
		inputManager = FindObjectOfType<InputManager>().GetComponent<IStickEngineID>();
	}

	void IBegin.OnTouchBegin(int fingerID){
		float distFromStick = Vector2.Distance(InputManager.touchSpot,startingJoystickSpot);
		if (distFromStick<joystickMaxStartDist){
			inputManager.SetStickEngineID(fingerID);
            transform.position = SetStickPosition();
		}
	}
	void IHold.OnTouchHeld(){
        transform.position = SetStickPosition();
	}
	void IEnd.OnTouchEnd(){
		transform.position = startingJoystickSpot;
	}

	Vector2 SetStickPosition(){
		return startingJoystickSpot + Vector2.ClampMagnitude(InputManager.touchSpot - startingJoystickSpot, joystickMaxMoveDistance);
	}
}
