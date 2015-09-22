using UnityEngine;
using System.Collections;
using GenericFunctions;

public class Joyfulstick : MonoBehaviour {
	
	public Jai jaiScript;
	public Spear spearScript;
	public Collider2D basketCollider;
	public Collider2D worldBoundsCollider;

	public Rigidbody2D basketBody;

	public SpriteRenderer controlStickSprite;

	public Vector2 startingJoystickSpot; //position of the joystick on screen
	public Vector2 touchSpot;
	public Vector2 moveDir;
	public Vector2 rawFinger;
	public Vector2 startingTouchPoint;
	public Vector2 releaseTouchPoint;
	public Vector2 attackDir;
	public Vector2 velVector;

	public float distFromStick; //distance your finger is from the joystick
	public float distToThrow;
	public float releaseDist;
	public float moveForce;
	public float maxBalloonSpeed;
	public float joystickMaxMoveDistance; //maximum distance you can move the joystick
	public float joystickMaxStartDist;
	public float speed;

	public int joystickFinger;
	public int spearFinger;
	public int pooOnYou;

	public bool beingHeld;

	void Awake () {
		transform.position = new Vector3 (-Constants.worldDimensions.x * (2f/3f),-Constants.worldDimensions.y * (2f/5f),0f);
		basketBody = GameObject.Find ("BalloonBasket").GetComponent<Rigidbody2D>();
		controlStickSprite = GameObject.Find ("ControlStick").GetComponent<SpriteRenderer>();
		basketCollider = GameObject.Find ("Basket").GetComponent<BoxCollider2D> ();
		worldBoundsCollider = GameObject.Find ("WorldBounds").GetComponent<EdgeCollider2D> ();
		jaiScript = GameObject.Find ("Jai").GetComponent<Jai> ();
		maxBalloonSpeed = 1.5f;

		startingJoystickSpot = transform.position;
		joystickMaxMoveDistance = .75f;
		joystickMaxStartDist = 1.25f;
		distToThrow = .03f;
		joystickFinger = -1;
		spearFinger = -2;
		moveForce = 20f;
	}

	Vector2 ConvertFingerPosition(Vector2 fingerIn){
		return (fingerIn + Constants.correctionPixels) * Constants.correctionPixelFactor;
	}

	void DoPhysics(){
		if (Mathf.Abs (velVector.x)<maxBalloonSpeed || Mathf.Sign (velVector.x) != Mathf.Sign (moveDir.x)){
			basketBody.AddForce (Vector2.right * moveDir.x * moveForce);
		}
		if (Mathf.Abs (velVector.y)<maxBalloonSpeed || Mathf.Sign (velVector.y) != Mathf.Sign (moveDir.y)){
			basketBody.AddForce (Vector2.up * moveDir.y * moveForce);
		}
	}

	// Update is called once per frame
	void Update () {
		Physics2D.IgnoreCollision (basketCollider, worldBoundsCollider, (basketBody.velocity.y > 0||beingHeld));
		velVector = basketBody.velocity;
		speed = velVector.magnitude;
		startingJoystickSpot = transform.position;
		if (Input.touchCount>0){
			foreach (Touch finger in Input.touches){
				//rawFinger = finger.position;
				touchSpot = ConvertFingerPosition(finger.position);
				if(!beingHeld){
					if (finger.phase == TouchPhase.Began){
						distFromStick = Vector2.Distance(ConvertFingerPosition(finger.position),startingJoystickSpot);
						if (distFromStick<joystickMaxStartDist){
							joystickFinger = finger.fingerId;
							moveDir = Vector2.ClampMagnitude(ConvertFingerPosition(finger.position) - startingJoystickSpot,joystickMaxMoveDistance);
							controlStickSprite.transform.position = startingJoystickSpot + moveDir;
						}
						else{
							if (Input.touchCount<3){
								startingTouchPoint = ConvertFingerPosition(finger.position);
								spearFinger = finger.fingerId;
							}
						}
					}
					else if (finger.phase == TouchPhase.Moved || finger.phase == TouchPhase.Stationary){ //while your finger is on the screen (joystick only)
						if (finger.fingerId == joystickFinger){ //move the joystick
							moveDir = Vector2.ClampMagnitude(ConvertFingerPosition(finger.position) - startingJoystickSpot,joystickMaxMoveDistance);
							controlStickSprite.transform.position = startingJoystickSpot + moveDir;
							DoPhysics();
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
				else{
					if (finger.phase == TouchPhase.Began){
						StartCoroutine (jaiScript.StabTheBeast());
					}
				}
			}
		}
		else{
			touchSpot = Vector2.up * Constants.worldDimensions.y * 10f;
		}
	}
}
