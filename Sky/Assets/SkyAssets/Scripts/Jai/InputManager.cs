using UnityEngine;
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
    [SerializeField] Selector[] selectors;

	List<IBegin> beginners;
	List<IHold> holders;
    List<IEnd> enders;
	IEnd stickEnd;
    IEnd basketEnd;
	IEnd jaiEnd;
	#endregion

	int stickEngineFinger =-1;
	int jaiFinger =-1;

    Vector2 correctionPixels;
    float correctionPixelFactor;

    void Awake(){
		beginners = new List<IBegin>(new IBegin[]{
			joyfulstick,
			jai,
			pauser
		});
		holders = new List<IHold>(new IHold[]{
			basketEngine,
			joyfulstick,
		});
        enders = new List<IEnd>(selectors);
		stickEnd = joyfulstick;
		jaiEnd = jai;
        basketEnd = basketEngine;

        Corrections pixelFix = new Corrections(true);
        correctionPixels = pixelFix.correctionPixels;
        correctionPixelFactor = pixelFix.correctionPixelFactor;
    }

    #region IFreezable
    bool isFrozen; bool IFreezable.IsFrozen{get => isFrozen;
	    set => isFrozen = value;
    }
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
                touchSpot = (finger.position + correctionPixels) * correctionPixelFactor;
                //Debug.Log(touchSpot);
				if (finger.phase == TouchPhase.Began){
					beginners.ForEach(beginner=> beginner.OnTouchBegin(finger.fingerId));
				}
				else if (finger.phase == TouchPhase.Moved || finger.phase == TouchPhase.Stationary){
					if (finger.fingerId == stickEngineFinger && !isFrozen){
						holders.ForEach(holder=> holder.OnTouchHeld());
					}
				}
				else if (finger.phase == TouchPhase.Ended){
					if (finger.fingerId == stickEngineFinger && !isFrozen){
						stickEnd.OnTouchEnd();
                        basketEnd.OnTouchEnd();
						stickEngineFinger =-1;
					}
					else if (finger.fingerId == jaiFinger && !isFrozen){
						jaiEnd.OnTouchEnd();
						jaiFinger =-1;
					}
                    else {
                        enders.ForEach(ender=> ender.OnTouchEnd());
                    }
				}
			}
		}
		else{
			touchSpot = Vector2.up * Constants.WorldDimensions.y * 10f;
		}
	}
}