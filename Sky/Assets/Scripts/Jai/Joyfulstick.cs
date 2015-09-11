using UnityEngine;
using System.Collections;

public class Joyfulstick : MonoBehaviour {
	
	public Jai jaiScript;
	public Spear spearScript;

	public Rigidbody2D basketBody;

	public SpriteRenderer controlStickSprite;

	public Vector2 startingJoystickSpot; //position of the joystick on screen
	public Vector2 touchSpot;
	public Vector2 moveDir;
	public Vector2 rawFinger;
	public Vector2 startingTouchPoint;
	public Vector2 releaseTouchPoint;
	public Vector2 attackDir;
	public Vector2 correctionPixels;

	public float distFromStick; //distance your finger is from the joystick
	public float distToThrow;
	public float releaseDist;
	public float moveForce;
	public float maxBalloonSpeed;
	public float joystickAppearanceThreshhold; //maximum distance you can move the joystick
	public float joystickMaxThumbDist;
	public float correctionPixelFactor;
	
	public int n;
	public int joystickFinger;
	public int spearFinger;

	// Use this for initialization
	void Awake () {
		basketBody = GameObject.Find ("BalloonBasket").GetComponent<Rigidbody2D>();
		controlStickSprite = GameObject.Find ("ControlStick").GetComponent<SpriteRenderer>();
		jaiScript = GameObject.Find ("Jai").GetComponent<Jai> ();
		maxBalloonSpeed = 3f;

		correctionPixels = new Vector2 (560,-960); //half of the screen width is 560 and yeah the heights weird [1280-640]


		startingJoystickSpot = transform.position;
		joystickAppearanceThreshhold = .8f + 0.3f;
		joystickMaxThumbDist = 1.3f + 1f;
		distToThrow = .1f;
		joystickFinger = -1;
		spearFinger = -2;
		moveForce = 10f;
		correctionPixelFactor = 5f / 320f; //5 game units divided by 320 pixels

		//correctionPixels = new Vector2 (Screen.width/2,-Screen.height/2);
		//correctionPixelFactor = 5 / Screen.height;
	}

	Vector2 ConvertFingerPosition(Vector2 fingerIn){
		return (fingerIn + correctionPixels) * correctionPixelFactor;
	}

	// Update is called once per frame
	void Update () {

		if (Input.touchCount>0){
			foreach (Touch finger in Input.touches){
				rawFinger = finger.position;
				if (finger.phase == TouchPhase.Began){
					touchSpot = ConvertFingerPosition(finger.position);
					distFromStick = Vector2.Distance(touchSpot,startingJoystickSpot);
					if (distFromStick<joystickMaxThumbDist){
						joystickFinger = finger.fingerId;
						moveDir = Vector2.ClampMagnitude(touchSpot - startingJoystickSpot,joystickAppearanceThreshhold);
						controlStickSprite.transform.position = startingJoystickSpot + moveDir;
					}
					else{
						if (Input.touchCount<3){
							startingTouchPoint = ConvertFingerPosition(finger.position);
							spearFinger = finger.fingerId;
						}
					}
				}
				else if (finger.phase == TouchPhase.Moved){ //while your finger is moving on the screen (joystick only)
					if (finger.fingerId == joystickFinger){ //move the joystick
						touchSpot = ConvertFingerPosition(finger.position);
						moveDir = Vector2.ClampMagnitude(touchSpot - startingJoystickSpot,joystickAppearanceThreshhold);
						/*if (moveDir.magnitude>joystickAppearanceThreshhold){
							moveDir = moveDir.normalized * joystickAppearanceThreshhold;
						}*/
						distFromStick = moveDir.magnitude;
						controlStickSprite.transform.position = startingJoystickSpot + moveDir;
						if (basketBody.velocity.magnitude<maxBalloonSpeed){
							basketBody.AddForce (moveDir * moveForce);
						}
					}
				}
				else if (finger.phase == TouchPhase.Ended){ //when your finger comes off the screen
					if (finger.fingerId == joystickFinger){ //release the joystick
						controlStickSprite.transform.position = transform.position;
						joystickFinger = -1;
					}
					else if (finger.fingerId == spearFinger ){ //use the spear
						spearFinger = -2;
						releaseTouchPoint = ConvertFingerPosition(finger.position);
						attackDir = releaseTouchPoint - startingTouchPoint;
						releaseDist = Vector2.Distance (releaseTouchPoint,startingTouchPoint);
						if (!spearScript.throwing){
							if ( releaseDist > distToThrow){ //throw the spear
								StartCoroutine(jaiScript.ThrowSpear(attackDir.normalized));
							}
						}
					}
				}
			}
		}
	}
}
