using UnityEngine;

public class PixelTransform
{
    public PixelTransform(Transform transform)
    {
        Transform = transform;
        LastPosition = Position;
        TargetPosition = Position;
    }

    public Transform Transform;
    public Vector2 LastPosition;
    public Vector2 TargetPosition;
    public Vector2 Position
    {
        get => Transform.position;
        set => Transform.position = value;
    }

    public void TryUpdate()
    {
        if (Vector2.Distance(LastPosition, TargetPosition) >= 0.01f)
        {
            Position = TargetPosition;
        }
    }
}