using UnityEngine;
using System.Collections;
using GenericFunctions;

public class Joyfulstick : MonoBehaviour, ITouchable {

	public static ITouchable Instance;

	[SerializeField] private Transform controlStickTransform;

	private Collider2D basketCollider;
	private Rigidbody2D basketBody;

	private Vector2 startingJoystickSpot; //position of the joystick on screen
	private Vector2 touchSpot; 
	#region ITouchable
	Vector2 ITouchable.TouchSpot{get{return touchSpot;}}
	#endregion
	private Vector2 moveDir;
	private Vector2 rawFinger;
	private Vector2 startingTouchPoint;
	private Vector2 releaseTouchPoint;
	private Vector2 attackDir;
	private Vector2 velVector;

	private float distFromStick; //distance your finger is from the joystick
	private float maxBalloonSpeed = 1.5f;
	private float distToThrow = .03f;
	private float moveForce = 20f;
	private float joystickMaxMoveDistance = .75f; //maximum distance you can move the joystick
	private float joystickMaxStartDist = 1.25f;
	private float releaseDist;

	private int joystickFinger;
	private int spearFinger;

	void Awake () {
		Instance = this;

		basketBody = GameObject.Find("BalloonBasket").GetComponent<Rigidbody2D>();
		GameObject basket = GameObject.Find("Basket");
		basketCollider = basket.GetComponent<Collider2D>();

		transform.position = new Vector2 (-Constants.worldDimensions.x * (2f/3f),-Constants.worldDimensions.y * (2f/5f));
		startingJoystickSpot = transform.position;
		joystickFinger = -1;
		spearFinger = -2;
	}

	void Update () {
		Physics2D.IgnoreCollision (basketCollider, Constants.worldBoundsCollider, (basketBody.velocity.y > 0||Jai.JaiLegs.BeingHeld));
		velVector = basketBody.velocity;
		startingJoystickSpot = transform.position;
		if (Input.touchCount>0){
			foreach (Touch finger in Input.touches){
				//rawFinger = finger.position;
				touchSpot = ConvertFingerPosition(finger.position);
				if(!Jai.JaiLegs.BeingHeld){
					if (finger.phase == TouchPhase.Began){
						distFromStick = Vector2.Distance(ConvertFingerPosition(finger.position),startingJoystickSpot);
						if (distFromStick<joystickMaxStartDist){
							joystickFinger = finger.fingerId;
							moveDir = Vector2.ClampMagnitude(ConvertFingerPosition(finger.position) - startingJoystickSpot,joystickMaxMoveDistance);
							controlStickTransform.transform.position = startingJoystickSpot + moveDir;
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
							controlStickTransform.transform.position = startingJoystickSpot + moveDir;
							DoPhysics();
						}
					}
					else if (finger.phase == TouchPhase.Ended){ //when your finger comes off the screen
						if (finger.fingerId == joystickFinger){ //release the joystick
							controlStickTransform.transform.position = transform.position;
							joystickFinger = -1;
						}
						else if (finger.fingerId == spearFinger ){ //use the spear
							spearFinger = -2;
							releaseTouchPoint = ConvertFingerPosition(finger.position);
							attackDir = releaseTouchPoint - startingTouchPoint;
							releaseDist = Vector2.Distance (releaseTouchPoint,startingTouchPoint);
							if (!Jai.JaiArms.Throwing){
								if ( releaseDist > distToThrow){ //throw the spear
									StartCoroutine(Jai.JaiController.ThrowSpear(attackDir.normalized));
								}
							}
						}
					}
				}
				else{
					if (finger.phase == TouchPhase.Began){
						if (!Jai.JaiArms.Stabbing){
							StartCoroutine (Jai.JaiController.StabTheBeast());
						}
					}
				}
			}
		}
		else{
			touchSpot = Vector2.up * Constants.worldDimensions.y * 10f;
		}
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

}
