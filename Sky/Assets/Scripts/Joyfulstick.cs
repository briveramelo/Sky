using UnityEngine;
using System.Collections;

public class Joyfulstick : MonoBehaviour {

	public float distFromStick; //distance your finger is from the joystick
	public Vector2 startingJoystickSpot; //position of the joystick on screen
	public float joystickThreshhold; //maximum distance you can move the joystick
	public Vector2 moveDir;
	public int movingFinger;
	public Touch fingeringYourButthole;


	// Use this for initialization
	void Start () {
		startingJoystickSpot = transform.position;
		joystickThreshhold = .5f;
		movingFinger = -1;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.touchCount>0){
			foreach (Touch finger in Input.touches){
				if (finger.phase == TouchPhase.Began){
					moveDir = Vector2.ClampMagnitude(finger.rawPosition-startingJoystickSpot,joystickThreshhold);
					distFromStick = moveDir.magnitude;
					if (distFromStick<joystickThreshhold){
						movingFinger = finger.fingerId;
					}
				}
				else if (finger.phase == TouchPhase.Ended){
					if (finger.fingerId == movingFinger){
						movingFinger = -1;
					}
				}
			}
		}
	}
}
