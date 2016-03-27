using UnityEngine;
using System.Collections;
using GenericFunctions;

public interface IHoldable  {
	bool BeingHeld{get;set;}
}

public class Jai : TouchInput, IHoldable {

	public static IHoldable JaiLegs;

	[SerializeField] GameObject spear;
	[SerializeField] private Animator jaiAnimator;
	[SerializeField] private Spear mySpear; IThrowable mySpearHandle;
	
	private float throwForce = 1400f; //Force with which Jai throws the spear
	const float distToThrow = .03f;
	private bool throwing, stabbing;
	private enum Throw{
		Idle=0,
		Down=1,
		Up=2,
	}
	Vector2 startingTouchPoint;
	private bool beingHeld; public bool BeingHeld{get{return beingHeld;}set{beingHeld = value;}}

	void Awake(){
		JaiLegs = this;
		Constants.jaiTransform = transform;
		mySpearHandle = (IThrowable)mySpear;
	}

	protected override void OnTouchBegin(int fingerID){
		float distFromStick = Vector2.Distance(touchSpot,startingJoystickSpot);
		if (distFromStick>joystickMaxStartDist){
			if (Input.touchCount<3){
				startingTouchPoint = touchSpot;
				myFingerID = fingerID;
			}
		}
	}

	protected override void OnTouchHeld(){
		if (!stabbing){
			StartCoroutine(StabTheBeast());
		}
	}

	protected override void OnTouchEnd(int fingerID){
		if (fingerID == myFingerID ){ //use the spear
			myFingerID = -1;
			Vector2 releaseTouchPoint = touchSpot;
			Vector2 attackDir = (releaseTouchPoint - startingTouchPoint).normalized;
			float releaseDist = Vector2.Distance (releaseTouchPoint,startingTouchPoint);
			if (!throwing){
				if ( releaseDist > distToThrow ){ //throw the spear
					StartCoroutine(ThrowSpear(attackDir));
					StartCoroutine(PullOutNewSpear());
				}
			}
		}
	}

	IEnumerator ThrowSpear(Vector2 throwDir){
		throwing = true;
		Throw ThrowState = throwDir.y<=.2f ? Throw.Down : Throw.Up;
		transform.FaceForward(throwDir.x>0);
		mySpearHandle.FlyFree(throwDir * throwForce);

		jaiAnimator.SetInteger("AnimState",(int)ThrowState);
		yield return new WaitForSeconds (Constants.time2ThrowSpear);
		jaiAnimator.SetInteger("AnimState",(int)Throw.Idle);
		throwing = false;
	}

	IEnumerator StabTheBeast(){
		stabbing = true;
		//jaiAnimator.SetInteger("AnimState",5);
		Tentacles.StabbableTentacle.GetStabbed(); //stab the tentacle!
		yield return new WaitForSeconds (.1f);
		stabbing = false;
		//jaiAnimator.SetInteger("AnimState",0);
	}

	IEnumerator PullOutNewSpear(){
		yield return new WaitForSeconds (Constants.time2ThrowSpear);
		mySpearHandle = (Instantiate (spear, transform.position + (Vector3)Constants.stockSpearPosition, Quaternion.identity) as GameObject).GetComponent<IThrowable>();
	}
}