using UnityEngine;

public class Rope : MonoBehaviour {

	float lastXPosition;
	const float speedThreshold = 0.019f;
	[SerializeField] private Animator ropeAnimator;
	private enum RopeAnimState{
		Idle=0,
		Waving=1
	}

	void Update () {
		bool fastEnough = Mathf.Abs(transform.position.x-lastXPosition) > speedThreshold;
		ropeAnimator.SetInteger("AnimState", fastEnough ? (int)RopeAnimState.Waving : (int)RopeAnimState.Idle);
		lastXPosition = transform.position.x;
	}
}
