using UnityEngine;
using System.Collections.Generic;
using GenericFunctions;

public class MenuInputHandler : MonoBehaviour, IFreezable
{
    public static Vector2 TouchSpot;

    [SerializeField] private Selector[] _selectors;
    private List<IEnd> enders;

    private Vector2 _correctionPixels;
    private float _correctionPixelFactor;
    private bool _isFrozen;

    bool IFreezable.IsFrozen
    {
        get => _isFrozen;
        set => _isFrozen = value;
    }

    private void Awake()
    {
        enders = new List<IEnd>(_selectors);
        var pixelFix = new Corrections(false);
        _correctionPixels = pixelFix.CorrectionPixels;
        _correctionPixelFactor = pixelFix.CorrectionPixelFactor;
    }

    private void Update()
    {
        if (!_isFrozen)
        {
            if (Input.touchCount > 0)
            {
                foreach (var finger in Input.touches)
                {
                    TouchSpot = (finger.position + _correctionPixels) * _correctionPixelFactor;
                    if (finger.phase == TouchPhase.Began)
                    {
                        //beginners.ForEach(beginner => beginner.OnTouchBegin(finger.fingerId));
                    }
                    else if (finger.phase == TouchPhase.Moved || finger.phase == TouchPhase.Stationary)
                    {
                        //holders.ForEach(holder => holder.OnTouchHeld());
                    }
                    else if (finger.phase == TouchPhase.Ended)
                    {
                        enders.ForEach(ender => ender.OnTouchEnd());
                    }
                }
            }
            else
            {
                TouchSpot = 10f * Constants.WorldDimensions.y * Vector2.up;
            }
        }
    }
}