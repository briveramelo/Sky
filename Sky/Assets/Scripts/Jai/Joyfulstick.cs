using UnityEngine;
using System.Collections;
using GenericFunctions;

public class Joyfulstick : MonoBehaviour {
	
	public Jai jaiScript;
	public Spear spearScript;
	public Collider2D basketCollider;
	public Collider2D worldBoundsCollider;
	public Transform fingerWiperTransform;

	public Rigidbody2D basketBody;

	public SpriteRenderer controlStickSprite;

	public Vector2 startingJoystickSpot; //position of the joystick on screen
	//public Vector2 touchSpot;
	public Vector2 moveDir;
	public Vector2 rawFinger;
	public Vector2 startingTouchPoint;
	public Vector2 releaseTouchPoint;
	public Vector2 attackDir;
	public Vector2 correctionPixels;
	public Vector2 velVector;

	public float distFromStick; //distance your finger is from the joystick
	public float distToThrow;
	public float releaseDist;
	public float moveForce;
	public float maxBalloonSpeed;
	public float joystickMaxMoveDistance; //maximum distance you can move the joystick
	public float joystickMaxStartDist;
	public float correctionPixelFactor;
	public float speed;
	public float maxVectorSpeed;


	public int joystickFinger;
	public int spearFinger;
	public int pooOnYou;

	public bool beingHeld;

	// Use this for initialization
	void Awake () {
		basketBody = GameObject.Find ("BalloonBasket").GetComponent<Rigidbody2D>();
		controlStickSprite = GameObject.Find ("ControlStick").GetComponent<SpriteRenderer>();
		basketCollider = GameObject.Find ("Basket").GetComponent<BoxCollider2D> ();
		worldBoundsCollider = GameObject.Find ("WorldBounds").GetComponent<EdgeCollider2D> ();
		jaiScript = GameObject.Find ("Jai").GetComponent<Jai> ();
		fingerWiperTransform = transform.GetChild (1);
		maxBalloonSpeed = 3f;
		maxVectorSpeed = Mathf.Sqrt (maxBalloonSpeed);

		startingJoystickSpot = transform.position;
		joystickMaxMoveDistance = 1.1f;
		joystickMaxStartDist = 2.3f;
		distToThrow = .1f;
		joystickFinger = -1;
		spearFinger = -2;
		moveForce = 20f;//10f;
		correctionPixels = new Vector2 (Screen.width/2,(-3*Screen.height/2));
		correctionPixelFactor = 10f / Screen.height;
	}

	Vector2 ConvertFingerPosition(Vector2 fingerIn){
		return (fingerIn + correctionPixels) * correctionPixelFactor;
	}

	void DoPhysics(){
		if (Mathf.Sign (velVector.x) != Mathf.Sign (moveDir.x)){
			basketBody.AddForce (Vector2.right * moveDir.x * moveForce);
		}
		else {
			if (Mathf.Abs (speed)<maxBalloonSpeed){
				basketBody.AddForce (Vector2.right * moveDir.x * moveForce);
			}
		}
		if (Mathf.Sign (velVector.y) != Mathf.Sign (moveDir.y)){
			basketBody.AddForce (Vector2.up * moveDir.y * moveForce);
		}
		else {
			if (Mathf.Abs (speed)<maxBalloonSpeed){
				basketBody.AddForce (Vector2.up * moveDir.y * moveForce);
			}
		}
	}

	// Update is called once per frame
	void Update () {
		Physics2D.IgnoreCollision (basketCollider, worldBoundsCollider, (basketBody.velocity.y > 0||beingHeld));
		velVector = basketBody.velocity;
		speed = velVector.magnitude;

		if (Input.touchCount>0){
			foreach (Touch finger in Input.touches){
				//rawFinger = finger.position;
				//touchSpot = ConvertFingerPosition(finger.position);
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
					else if (finger.phase == TouchPhase.Moved){ //while your finger is moving on the screen (joystick only)
						if (finger.fingerId == joystickFinger){ //move the joystick
							moveDir = Vector2.ClampMagnitude(ConvertFingerPosition(finger.position) - startingJoystickSpot,joystickMaxMoveDistance);
							controlStickSprite.transform.position = startingJoystickSpot + moveDir;
							DoPhysics();
						}
						else if (pooOnYou>0){
							fingerWiperTransform.position = ConvertFingerPosition(finger.position);
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
	}
}
