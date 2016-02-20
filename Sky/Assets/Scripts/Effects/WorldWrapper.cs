using UnityEngine;
using System.Collections;
using GenericFunctions;

public class WorldWrapper : MonoBehaviour {

	[SerializeField] private BoxCollider2D[] worldBoundsForcePushers;

	void Awake () {
		worldBoundsForcePushers [0].offset = new Vector2 (0f, Constants.worldDimensions.y);
		worldBoundsForcePushers [1].offset = new Vector2 (Constants.worldDimensions.x, 0f);
		worldBoundsForcePushers [2].offset = new Vector2 (0f, -Constants.worldDimensions.y);
		worldBoundsForcePushers [3].offset = new Vector2 (-Constants.worldDimensions.x, 0f);

		worldBoundsForcePushers [0].size = new Vector2 (Constants.worldDimensions.x * 2, Constants.worldDimensions.y * .15625f);
		worldBoundsForcePushers [1].size = new Vector2 (Constants.worldDimensions.y * .15625f, Constants.worldDimensions.y * 2);
		worldBoundsForcePushers [2].size = new Vector2 (Constants.worldDimensions.x * 2, Constants.worldDimensions.y * .15625f);
		worldBoundsForcePushers [3].size = new Vector2 (Constants.worldDimensions.y * .15625f, Constants.worldDimensions.y * 2);

		Constants.bottomOfTheWorldCollider = worldBoundsForcePushers[2];
	}
}
