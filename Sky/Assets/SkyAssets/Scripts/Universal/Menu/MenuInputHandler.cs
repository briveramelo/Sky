using UnityEngine;
using System.Collections.Generic;
using GenericFunctions;

public class MenuInputHandler : MonoBehaviour, IFreezable {

    public static Vector2 touchSpot;

    [SerializeField] private Selector[] selectors;
    private List<IEnd> enders;

    private Vector2 correctionPixels;
    private float correctionPixelFactor;
    private bool isFrozen;
    bool IFreezable.IsFrozen {
        get => isFrozen;

        set => isFrozen = value;
    }

    private void Awake() {
        enders = new List<IEnd>(selectors);
        Corrections pixelFix = new Corrections(false);
        correctionPixels = pixelFix.correctionPixels;
        correctionPixelFactor = pixelFix.correctionPixelFactor;
    }

    private void Update() {
        if (!isFrozen) {
            if (Input.touchCount > 0) {
                foreach (Touch finger in Input.touches) {
                    touchSpot = (finger.position + correctionPixels) * correctionPixelFactor;
                    if (finger.phase == TouchPhase.Began) {
                        //beginners.ForEach(beginner => beginner.OnTouchBegin(finger.fingerId));
                    }
                    else if (finger.phase == TouchPhase.Moved || finger.phase == TouchPhase.Stationary) {
                        //holders.ForEach(holder => holder.OnTouchHeld());
                    }
                    else if (finger.phase == TouchPhase.Ended) {
                        enders.ForEach(ender => ender.OnTouchEnd());
                    }
                }
            }
            else {
                touchSpot = Vector2.up * Constants.WorldDimensions.y * 10f;
            }
        }
    }
}
