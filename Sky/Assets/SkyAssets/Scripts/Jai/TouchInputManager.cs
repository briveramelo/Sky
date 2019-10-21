using System;
using UnityEngine;
using System.Collections.Generic;

public interface IBegin
{
    void OnTouchBegin(int fingerId, Vector2 fingerPosition);
}

public interface IHold
{
    void OnTouchHeld(Vector2 fingerPosition);
}

public interface IEnd
{
    void OnTouchEnd(Vector2 fingerPosition);
}

public interface IFreezable
{
    bool IsFrozen { get; set; }
}

public class TouchInputManager : Singleton<TouchInputManager>, IFreezable
{
    public event Action<int, Vector2> OnTouchBegin;
    public event Action<int, Vector2> OnTouchHeld;
    public event Action<int, Vector2> OnTouchEnd;

    private Dictionary<int, string> _fingerIdClaimers = new Dictionary<int, string>();
    protected override bool _destroyOnLoad => true;
    private bool _isFrozen;
    
    public bool TryClaimFingerId(int fingerId, string claimer)
    {
        bool fingerIdIsClaimed = _fingerIdClaimers.TryGetValue(fingerId, out var storedClaimer);
        if (!fingerIdIsClaimed || claimer == storedClaimer)
        {
            //Debug.LogErrorFormat("claiming fingerid:{0}, claimer:{1}", fingerId, claimer);
            _fingerIdClaimers[fingerId] = claimer;
            return true;
        }

        return false;
    }

    public void ReleaseFingerId(int fingerId, string claimer)
    {
        if (_fingerIdClaimers.TryGetValue(fingerId, out var storedClaimer) && claimer == storedClaimer)
        {
            //Debug.LogErrorFormat("releasing fingerid:{0}, claimer:{1}", fingerId, claimer);
            _fingerIdClaimers.Remove(fingerId);
        }
    }

    public void ClearFingerIdClaimers()
    {
        _fingerIdClaimers.Clear();
    }


    bool IFreezable.IsFrozen
    {
        get => _isFrozen;
        set => _isFrozen = value;
    }

    private void Update()
    {
        if (Input.touchCount <= 0 || _isFrozen)
        {
            return;
        }

        foreach (var finger in Input.touches)
        {
            var fingerId = finger.fingerId;
            var touchSpot = TouchToWorld.GetWorldPosition(finger.position);
            switch (finger.phase)
            {
                case TouchPhase.Began:
                    OnTouchBegin?.Invoke(fingerId, touchSpot);
                    break;
                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    OnTouchHeld?.Invoke(fingerId, touchSpot);
                    break;
                case TouchPhase.Ended:
                    OnTouchEnd?.Invoke(fingerId, touchSpot);
                    break;
                default:
                    Debug.LogWarningFormat("unexpected finger phase type: {0}", finger.phase);
                    break;
            }
        }
    }
}