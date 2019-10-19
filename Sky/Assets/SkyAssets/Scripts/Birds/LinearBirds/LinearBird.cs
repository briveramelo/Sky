using UnityEngine;
using GenericFunctions;

public abstract class LinearBird : Bird{

	protected float MoveSpeed;

	protected override void Awake () {
		base.Awake();
		SetVelocity (Vector2.right);
	}

	public void SetVelocity(Vector2 desiredDirection){
		_rigbod.velocity = desiredDirection.normalized * MoveSpeed;
		transform.FaceForward(desiredDirection.x<0);
	}
}
