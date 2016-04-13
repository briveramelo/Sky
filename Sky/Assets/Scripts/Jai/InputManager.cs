using UnityEngine;
using System.Collections;
using GenericFunctions;
using System.Collections.Generic;

public interface IBegin{
	void OnTouchBegin(int fingerID);
}
public interface IHold{
	void OnTouchHeld();
}
public interface IEnd{
	void OnTouchEnd();
}

public interface IFreezable{
	bool IsFrozen{get;set;}
}
public interface IStickEngineID{
	void SetStickEngineID(int ID);
}
public interface IJaiID{
	void SetJaiID(int ID);
}

public class InputManager : MonoBehaviour, IFreezable, IStickEngineID, IJaiID {

	public static Vector2 touchSpot;

	#region Touch Handlers
	[SerializeField] BasketEngine basketEngine;
	[SerializeField] Jai jai;
	[SerializeField] Joyfulstick joyfulstick;
	[SerializeField] Pauser pauser;
	List<IBegin> beginners;
	List<IHold> holders;
	IEnd stickEnd;
	IEnd jaiEnd;

	int stickEngineFinger =-1;
	int jaiFinger =-1;

	void Awake(){
		beginners = new List<IBegin>(new IBegin[]{
			(IBegin)joyfulstick,
			(IBegin)jai,
			(IBegin)pauser
		});
		holders = new List<IHold>(new IHold[]{
			(IHold)basketEngine,
			(IHold)joyfulstick,
		});
		stickEnd = (IEnd)joyfulstick;
		jaiEnd = (IEnd)jai;
	}
	#endregion

	#region IFreezable
	bool isFrozen; bool IFreezable.IsFrozen{get{return isFrozen;}set{isFrozen = value;}}
	#endregion

	#region ISetIDs
	void IJaiID.SetJaiID(int ID){
		jaiFinger = ID;
	}
	void IStickEngineID.SetStickEngineID(int ID){
		stickEngineFinger = ID;
	}
	#endregion

	void Update () {
		if (Input.touchCount>0){
			foreach (Touch finger in Input.touches){
				touchSpot = (finger.position + Constants.correctionPixels) * Constants.correctionPixelFactor;
				if (finger.phase == TouchPhase.Began){
					beginners.ForEach(beginner=> beginner.OnTouchBegin(finger.fingerId));
				}
				if(!isFrozen){
					if (finger.phase == TouchPhase.Moved || finger.phase == TouchPhase.Stationary){
						if (finger.fingerId == stickEngineFinger){
							holders.ForEach(holder=> holder.OnTouchHeld());
						}
					}
					else if (finger.phase == TouchPhase.Ended){
						if (finger.fingerId == stickEngineFinger){
							stickEnd.OnTouchEnd();
							stickEngineFinger =-1;
						}
						else if (finger.fingerId == jaiFinger){
							jaiEnd.OnTouchEnd();
							jaiFinger =-1;
						}
					}
				}
			}
		}
		else{
			touchSpot = Vector2.up * Constants.WorldDimensions.y * 10f;
		}
	}
}