using UnityEngine;
using System.Collections;
using GenericFunctions;
public abstract class LinearBird : Bird{

	protected float moveSpeed;

	protected virtual void Awake () {
		rigbod.velocity = Vector2.right * moveSpeed;
		transform.Face4ward(false);
	}

	public void SetVelocity(Vector2 desiredDirection){
		rigbod.velocity = desiredDirection.normalized * moveSpeed;
		transform.Face4ward(desiredDirection.x>0);
	}
}
