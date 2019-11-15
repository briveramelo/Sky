using GenericFunctions;
using UnityEngine;

public class WorldBounds : MonoBehaviour
{
    [SerializeField] private EdgeCollider2D _worldBounds;

    private void Start()
    {
        var worldSize = ScreenSpace.WorldEdge;
        var xDist = worldSize.x;
        var yDist = worldSize.y;
        var points = new[]
        {
            new Vector2(-xDist, yDist),
            new Vector2(xDist, yDist),
            new Vector2(xDist, -yDist),
            new Vector2(-xDist, -yDist),
            new Vector2(-xDist, yDist),
        };
        _worldBounds.points = points;
    }
}
