using UnityEngine;
using GenericFunctions;

public abstract class LinearBird : Bird
{
    protected abstract float MoveSpeed { get; }
    public Vector2 MoveDirection => _rigbod.velocity;

    protected virtual void Start()
    {
        SetDirection(transform.position.x > 0 ? Vector2.left : Vector2.right);
    }

    private void SetDirection(Vector2 desiredDirection)
    {
        _rigbod.velocity = Constants.SpeedMultiplier * MoveSpeed * desiredDirection.normalized;
        transform.FaceForward(desiredDirection.x < 0);
    }
}