using System;
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

public class BasketEngine : MonoBehaviour, IBumpable, IDie
{
    [SerializeField] private Rigidbody2D _rigbod;
    
    private const float _moveSpeed = 2.7f;
    private bool _movingEnabled = true;

    private void Start()
    {
        Joystick.Instance.OnTouchHold += OnTouchHeld;
        Joystick.Instance.OnTouchEnded += OnTouchEnd;
    }

    private void OnDestroy()
    {
        Joystick.Instance.OnTouchHold -= OnTouchHeld;
        Joystick.Instance.OnTouchEnded -= OnTouchEnd;
    }

    private void OnTouchHeld(Vector2 moveDirection)
    {
        if (_movingEnabled)
        {
            _rigbod.velocity = moveDirection * _moveSpeed;
        }
    }

    private void OnTouchEnd()
    {
        _rigbod.velocity = Vector2.zero;
    }

    void IBumpable.Bump(Vector2 bumpDir)
    {
        StopAllCoroutines();
        _rigbod.velocity = bumpDir;
        StartCoroutine(Bool.Toggle(boolState => _movingEnabled = boolState, .5f));
        ScoreSheet.Tallier.TallyThreat(Threat.BasketBumped);
        Invoke("StabilizeBumpThreat", 2f);
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
}