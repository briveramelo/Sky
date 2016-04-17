using UnityEngine;
using GenericFunctions;

public abstract class LinearBird : Bird{

	protected float moveSpeed;

	protected override void Awake () {
		SetVelocity (Vector2.right);
		base.Awake();
	}

	public void SetVelocity(Vector2 desiredDirection){
		rigbod.velocity = desiredDirection.normalized * moveSpeed;
		transform.FaceForward(desiredDirection.x<0);
	}
}
