using UnityEngine;
using System.Collections;

public class Joyfulstick : MonoBehaviour {

	public Vector2 startingJoystickSpot; //position of the joystick on screen
	public Vector2 touchSpot;
	public Vector2 moveDir;

	public Vector2 startingTouchPoint;
	public Vector2 releaseTouchPoint;
	public Vector2 attackDir;

	public Vector2 correctionSpot;

	public float distFromStick; //distance your finger is from the joystick
	public float distToThrow;
	public float releaseDist;
	public float moveForce;
	public float maxBalloonSpeed;
	public float joystickThreshhold; //maximum distance you can move the joystick

	public SpriteRenderer joystickThumbSprite;

	public Jai jaiScript;
	public Transform jaiTransform;

	public int n;
	public int joystickFinger;
	public int spearFinger;

	public Rigidbody2D balloonBasketBody;

	// Use this for initialization
	void Start () {
		startingJoystickSpot = transform.position;
		joystickThreshhold = .8f;
		distToThrow = .1f;
		joystickFinger = -1;
		spearFinger = -2;
		moveForce = 5f;
		jaiScript = GameObject.Find ("Jai").GetComponent<Jai> ();
		joystickThumbSprite = GameObject.Find ("ThumbJoystick").GetComponent<SpriteRenderer>();
		jaiTransform = jaiScript.transform;
		balloonBasketBody = GameObject.Find ("BalloonBasket").rigidbody2D;
		correctionSpot = new Vector2 (9.1f,-14.61f);
		maxBalloonSpeed = balloonBasketBody.GetComponent<BalloonBasket>().maxBalloonSpeed;
		Physics2D.IgnoreLayerCollision (14, 13); //ignore basket and spear collision
	}

	void OnDrawGizmos(){
		Gizmos.DrawWireSphere (startingJoystickSpot, joystickThreshhold);
	}

	// Update is called once per frame
	void Update () {
		if (Input.touchCount>0){
			foreach (Touch finger in Input.touches){
				if (finger.phase == TouchPhase.Began){
					touchSpot = finger.position/23f + correctionSpot;
					distFromStick = Vector2.Distance(touchSpot,startingJoystickSpot);
					if (distFromStick<joystickThreshhold){
						joystickFinger = finger.fingerId;
						joystickThumbSprite.enabled = true;
					}
					else{
						if (Input.touchCount<3){
							startingTouchPoint = finger.position/23f + correctionSpot;
							spearFinger = finger.fingerId;
						}
					}
				}
				else if (finger.phase == TouchPhase.Moved){ //while your finger is moving on the screen (joystick only)
					if (finger.fingerId == joystickFinger){ //move the joystick
						touchSpot = finger.position/23f + correctionSpot;
						moveDir = touchSpot - startingJoystickSpot;
						if (moveDir.magnitude>joystickThreshhold){
							moveDir = moveDir.normalized * joystickThreshhold;
						}
						distFromStick = moveDir.magnitude;
						joystickThumbSprite.transform.position = startingJoystickSpot + moveDir;
						if (balloonBasketBody.velocity.magnitude<maxBalloonSpeed){
							balloonBasketBody.AddForce (moveDir * moveForce);
						}
					}
				}
				else if (finger.phase == TouchPhase.Ended){ //when your finger comes off the screen
					if (finger.fingerId == joystickFinger){ //release the joystick
						joystickThumbSprite.enabled = false;
						joystickFinger = -1;
					}
					else if (finger.fingerId == spearFinger ){ //use the spear
						if (!jaiScript.throwing && !jaiScript.stabbing){
							spearFinger = -2;
							releaseTouchPoint = finger.position/23f + correctionSpot;
							attackDir = releaseTouchPoint - startingTouchPoint;
							releaseDist = Vector2.Distance (releaseTouchPoint,startingTouchPoint);
							if ( releaseDist < distToThrow){ //stab the spear
								attackDir = (Vector2)jaiTransform.position - releaseTouchPoint;
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
