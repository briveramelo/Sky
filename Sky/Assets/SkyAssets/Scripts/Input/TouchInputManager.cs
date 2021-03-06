﻿using System;
using UnityEngine;
using System.Collections.Generic;
using BRM.DebugAdapter;
using BRM.DebugAdapter.Interfaces;

public interface IFreezable
{
    bool IsFrozen { get; set; }
}

public class TouchInputManager : Singleton<TouchInputManager>, IFreezable
{
    public event Action<int, Vector2> OnTouchWorldBegin;
    public event Action<int, Vector2> OnTouchWorldHeld;
    public event Action<int, Vector2> OnTouchWorldEnd;

    protected override bool _destroyOnLoad => true;
    
    private Dictionary<int, string> _fingerIdClaimers = new Dictionary<int, string>();
    private IDebug _debugger = new UnityDebugger {Enabled = false};
    private bool _isFrozen;
    
    public bool TryClaimFingerId(int fingerId, string claimer)
    {
        _debugger.LogFormat("requesting fingerid:{0}, claimer:{1}", fingerId, claimer);
        bool fingerIdIsClaimed = _fingerIdClaimers.TryGetValue(fingerId, out var storedClaimer);
        if (!fingerIdIsClaimed || claimer == storedClaimer)
        {
            _debugger.LogErrorFormat("claiming fingerid:{0}, claimer:{1}", fingerId, claimer);
            _fingerIdClaimers[fingerId] = claimer;
            return true;
        }

        return false;
    }

    public void ReleaseFingerId(int fingerId, string claimer)
    {
        if (_fingerIdClaimers.TryGetValue(fingerId, out var storedClaimer) && claimer == storedClaimer)
        {
            _debugger.LogErrorFormat("releasing fingerid:{0}, claimer:{1}", fingerId, claimer);
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
            var worldTouchSpot = finger.position.PixelsToWorldPosition();
            switch (finger.phase)
            {
                case TouchPhase.Began:
                    OnTouchWorldBegin?.Invoke(fingerId, worldTouchSpot);
                    break;
                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    OnTouchWorldHeld?.Invoke(fingerId, worldTouchSpot);
                    break;
                case TouchPhase.Ended:
                    OnTouchWorldEnd?.Invoke(fingerId, worldTouchSpot);
                    break;
                default:
                    Debug.LogWarningFormat("unexpected finger phase type: {0}", finger.phase);
                    break;
            }
        }
    }
}