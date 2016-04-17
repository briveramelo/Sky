using UnityEngine;
using System.Collections;
using GenericFunctions;

public class Jai : MonoBehaviour, IBegin, IEnd, IFreezable {

	[SerializeField] GameObject spear;
	[SerializeField] private Animator jaiAnimator;
	[SerializeField] private Spear mySpear; IThrowable mySpearHandle;
	private IJaiID inputManager;

	const float throwForce = 1400f; //Force with which Jai throws the spear
	const float distToThrow = .03f;
	bool throwing, stabbing, beingHeld;
	bool IFreezable.IsFrozen{get{return beingHeld;}set{beingHeld = value;}}
	private enum Throw{
		Idle=0,
		Down=1,
		Up=2,
	}
	Vector2 startingTouchPoint; Tentacles tent;

	void Awake(){
		Constants.jaiTransform = transform;
		inputManager = FindObjectOfType<InputManager>().GetComponent<IJaiID>();
		mySpearHandle = (IThrowable)mySpear;
	}

	void IBegin.OnTouchBegin(int fingerID){
        if (!Pauser.Paused) {
            if (!beingHeld){
			    float distFromStick = Vector2.Distance(InputManager.touchSpot,Joyfulstick.startingJoystickSpot);
			    float distFromPause = Vector2.Distance(InputManager.touchSpot,Pauser.pauseSpot);
			    if (distFromStick>Joyfulstick.joystickMaxStartDist && distFromPause > Pauser.pauseRadius){
				    if (Input.touchCount<3){
					    startingTouchPoint = InputManager.touchSpot;
					    inputManager.SetJaiID(fingerID);
				    }
			    }
		    }
		    else{
			    if (!stabbing){
				    StartCoroutine(StabTheBeast());
			    }
		    }
        }
	}
	void IEnd.OnTouchEnd(){
        if (!Pauser.Paused) {
            Vector2 releaseTouchPoint = InputManager.touchSpot;
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