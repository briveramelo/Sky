using UnityEngine;
using GenericFunctions;

public class WorldWrapper : MonoBehaviour {

	[SerializeField] private BoxCollider2D[] worldBoundsForcePushers;

	void Awake () {
		worldBoundsForcePushers [0].offset = new Vector2 (0f, Constants.WorldDimensions.y);
		worldBoundsForcePushers [1].offset = new Vector2 (Constants.WorldDimensions.x, 0f);
		worldBoundsForcePushers [2].offset = new Vector2 (0f, -Constants.WorldDimensions.y);
		worldBoundsForcePushers [3].offset = new Vector2 (-Constants.WorldDimensions.x, 0f);

		worldBoundsForcePushers [0].size = new Vector2 (Constants.WorldDimensions.x * 2, Constants.WorldDimensions.y * .15625f);
		worldBoundsForcePushers [1].size = new Vector2 (Constants.WorldDimensions.y * .15625f, Constants.WorldDimensions.y * 2);
		worldBoundsForcePushers [2].size = new Vector2 (Constants.WorldDimensions.x * 2, Constants.WorldDimensions.y * .15625f);
		worldBoundsForcePushers [3].size = new Vector2 (Constants.WorldDimensions.y * .15625f, Constants.WorldDimensions.y * 2);

		Constants.bottomOfTheWorldCollider = worldBoundsForcePushers[2];
	}
}
