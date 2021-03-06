﻿using System;
using UnityEngine;
using GenericFunctions;

public interface IBumpable
{
    void Bump(Vector2 bumpDir);
}

public interface IDie
{
    void Die();
    void Rebirth();
}

public class BasketEngine : MonoBehaviour, IBumpable, IDie, IFreezable
{
    [SerializeField] private Rigidbody2D _rigbod;
    
    private const float _moveSpeed = 2.7f;
    private bool _movingEnabled = true;

    private void Start()
    {
        Joystick.Instance.OnTouchDirectionHold += OnTouchDirectionHeld;
        Joystick.Instance.OnTouchEnded += OnTouchEnd;
    }

    private void OnDestroy()
    {
        if (Joystick.Instance == null)
        {
            return;
        }
        Joystick.Instance.OnTouchDirectionHold -= OnTouchDirectionHeld;
        Joystick.Instance.OnTouchEnded -= OnTouchEnd;
    }

    private void OnTouchDirectionHeld(Vector2 moveDirection)
    {
        if (_movingEnabled)
        {
            _rigbod.velocity = Constants.SpeedMultiplier * _moveSpeed * moveDirection;
        }
    }

    private void OnTouchEnd()
    {
        _rigbod.velocity = Vector2.zero;
    }

    void IBumpable.Bump(Vector2 bumpDir)
    {
        StopAllCoroutines();
        _rigbod.velocity = Constants.SpeedMultiplier * bumpDir;
        StartCoroutine(Bool.Toggle(boolState => _movingEnabled = boolState, .5f));
        ScoreSheet.Tallier.TallyThreat(Threat.BasketBumped);
        Invoke(nameof(StabilizeBumpThreat), 2f);
    }

    private void StabilizeBumpThreat()
    {
        ScoreSheet.Tallier.TallyThreat(Threat.BasketStabilized);
    }

    void IDie.Die()
    {
        _movingEnabled = false;
    }

    void IDie.Rebirth()
    {
        _movingEnabled = true;
    }

    bool IFreezable.IsFrozen
    {
        get => !_movingEnabled;
        set => _movingEnabled = !value;
    }
}