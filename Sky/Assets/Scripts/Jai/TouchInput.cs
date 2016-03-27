using UnityEngine;
using System.Collections;
using GenericFunctions;

public interface ITouchable {
	Vector2 TouchSpot{get;}
}
public abstract class TouchInput : MonoBehaviour,ITouchable {

	public static ITouchable Toucher;
	Vector2 ITouchable.TouchSpot{get{return touchSpot;}}

	protected Vector2 startingJoystickSpot = new Vector2 (-Constants.WorldDimensions.x * (2f/3f),-Constants.WorldDimensions.y * (2f/5f));
	protected const float joystickMaxStartDist = 1.25f;
	protected const float joystickMaxMoveDistance = .75f; //maximum distance you can move the joystick
	protected Vector2 touchSpot;
	protected int myFingerID;

	void Awake(){
		Toucher = this;
	}

	void Update () {
		if (Input.touchCount>0){
			foreach (Touch finger in Input.touches){
				touchSpot = ConvertFingerPosition(finger.position);
				if(!Jai.JaiLegs.BeingHeld){
					if (finger.phase == TouchPhase.Began){
						OnTouchBegin(finger.fingerId);
					}
					else if (finger.phase == TouchPhase.Moved || finger.phase == TouchPhase.Stationary){ //while your finger is on the screen (joystick only)
						OnTouchMovedStationary(finger.fingerId);
					}
					else if (finger.phase == TouchPhase.Ended){ //when your finger comes off the screen
						OnTouchEnd(finger.fingerId);
					}
				}
				else{
					if (finger.phase == TouchPhase.Began){
						OnTouchHeld();
					}
				}
			}
		}
		else{
			touchSpot = Vector2.up * Constants.WorldDimensions.y * 10f;
		}
	}

	protected abstract void OnTouchBegin(int fingerID);
	protected virtual void OnTouchHeld(){}
	protected virtual void OnTouchMovedStationary(int fingerID){}
	protected abstract void OnTouchEnd(int fingerID);

	protected Vector2 ConvertFingerPosition(Vector2 fingerIn){
		return (fingerIn + Constants.correctionPixels) * Constants.correctionPixelFactor;
	}
}