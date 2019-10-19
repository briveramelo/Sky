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

public class BasketEngine : MonoBehaviour, IBumpable, IHold, IEnd, IDie
{
    [SerializeField] private Rigidbody2D _rigbod;
    private const float _moveSpeed = 2.7f;
    private bool _movingEnabled = true;

    void IHold.OnTouchHeld()
    {
        if (_movingEnabled)
        {
            var moveDir = Vector2.ClampMagnitude(InputManager.TouchSpot - Joyfulstick.StartingJoystickSpot, Joyfulstick.JoystickMaxMoveDistance);
            _rigbod.velocity = moveDir * _moveSpeed;
        }
    }

    void IEnd.OnTouchEnd()
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