using UnityEngine;
using GenericFunctions;

public class WorldWrapper : MonoBehaviour
{
    [SerializeField] private BoxCollider2D[] _worldBoundsForcePushers;

    private void Awake()
    {
        _worldBoundsForcePushers[0].offset = new Vector2(0f, Constants.WorldDimensions.y);
        _worldBoundsForcePushers[1].offset = new Vector2(Constants.WorldDimensions.x, 0f);
        _worldBoundsForcePushers[2].offset = new Vector2(0f, -Constants.WorldDimensions.y);
        _worldBoundsForcePushers[3].offset = new Vector2(-Constants.WorldDimensions.x, 0f);

        _worldBoundsForcePushers[0].size = new Vector2(Constants.WorldDimensions.x * 2, Constants.WorldDimensions.y * .15625f);
        _worldBoundsForcePushers[1].size = new Vector2(Constants.WorldDimensions.y * .15625f, Constants.WorldDimensions.y * 2);
        _worldBoundsForcePushers[2].size = new Vector2(Constants.WorldDimensions.x * 2, Constants.WorldDimensions.y * .15625f);
        _worldBoundsForcePushers[3].size = new Vector2(Constants.WorldDimensions.y * .15625f, Constants.WorldDimensions.y * 2);

        Constants.BottomOfTheWorldCollider = _worldBoundsForcePushers[2];
    }
}