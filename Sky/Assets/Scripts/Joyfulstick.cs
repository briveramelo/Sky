using UnityEngine;
using System.Collections;

public class Joyfulstick : MonoBehaviour {

	public float distFromStick; //distance your finger is from the joystick
	public Vector2 startingJoystickSpot; //position of the joystick on screen
	public float joystickThreshhold; //maximum distance you can move the joystick
	public Vector2 moveDir;
	public int joystickFinger;
	public int spearFinger;
	public SpriteRenderer joystickThumb;
	public Vector2 startingTouchPoint;
	public Vector2 releaseTouchPoint;
	public Jai jaiScript;
	public Transform jaiTransform;
	public float distToThrow;
	
	// Use this for initialization
	void Start () {
		startingJoystickSpot = transform.position;
		joystickThreshhold = .5f;
		distToThrow = .1f;
		joystickFinger = -1;
		jaiScript = GameObject.Find ("Jai").GetComponent<Jai> ();
		jaiTransform = jaiScript.transform;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.touchCount>0){
			foreach (Touch finger in Input.touches){
				if (finger.phase == TouchPhase.Began){
					moveDir = Vector2.ClampMagnitude(finger.rawPosition-startingJoystickSpot,joystickThreshhold);
					distFromStick = moveDir.magnitude;
					if (distFromStick<joystickThreshhold){
						joystickFinger = finger.fingerId;
						joystickThumb.enabled = true;
						joystickThumb.transform.position = startingJoystickSpot + moveDir;
					}
					else{
						if (Input.touchCount<3){
							startingTouchPoint = finger.rawPosition;
							spearFinger = finger.fingerId;
						}
					}
				}
				else if (finger.phase == TouchPhase.Moved){ //while your finger is moving on the screen (joystick only)
					if (finger.fingerId == joystickFinger){ //move the joystick
						moveDir = Vector2.ClampMagnitude(finger.rawPosition-startingJoystickSpot,joystickThreshhold);
						distFromStick = moveDir.magnitude;
						joystickThumb.transform.position = startingJoystickSpot + moveDir;
					}
				}
				else if (finger.phase == TouchPhase.Ended){ //when your finger comes off the screen
					if (finger.fingerId == joystickFinger){ //release the joystick
						joystickThumb.enabled = false;
						joystickFinger = -1;
					}
					else if (finger.fingerId == spearFinger ){ //use the spear
						if (!jaiScript.throwing && !jaiScript.stabbing){
							releaseTouchPoint = finger.rawPosition;
							Vector2 attackDir = releaseTouchPoint - startingTouchPoint;
							float releaseDist = Vector2.Distance (releaseTouchPoint,startingTouchPoint);
							if ( releaseDist < distToThrow){ //stab the spear
								attackDir = jaiTransform.position - releaseTouchPoint;
								StartCoroutine(jaiScript.StabSpear(attackDir.normalized));
							}
							else { // throw the spear
								StartCoroutine(jaiScript.ThrowSpear(attackDir.normalized));
							}
						}
					}
				}
			}
		}
	}
}
