﻿using UnityEngine;
using System.Collections;
using GenericFunctions;

public class Jai : MonoBehaviour, IControllable, ISpawnable, IHoldable, IArms, IHideable {

	public static IControllable JaiController;
	public static ISpawnable SpearGenerator;
	public static IHoldable JaiLegs;
	public static IArms JaiArms;
	public static IHideable ToHide;

	[SerializeField] private Animator jaiAnimator;
	[SerializeField] private Collider2D myCollider;
	[SerializeField] GameObject spear;

	private float throwForce = 1400f; //Force with which Jai throws the spear

	private int throws; //counts number of throws he's done

	#region IHideable
	Collider2D IHideable.ColliderToHide{get{return myCollider;}}
	#endregion
	#region IArms
	private bool throwing; public bool Throwing{get{return throwing;}}
	private bool stabbing; public bool Stabbing{get{return stabbing;}}
	#endregion
	#region IHoldable
	private bool beingHeld; public bool BeingHeld{get{return beingHeld;}set{beingHeld = value;}}
	#endregion

	private enum HighLow{
		Low=1,
		High=2
	}
	private enum Throw{
		Idle=0,
		DownLeft=1,
		DownRight=2,
		UpLeft=3,
		UpRight=4
	}
	private Throw ThrowState;
	private HighLow highLow;

	void Awake(){
		JaiController = this;
		SpearGenerator = this;
		JaiArms = this;
		JaiLegs = this;
		ToHide = this;
		Constants.jaiTransform = transform;
	}

	#region IControllable
	IEnumerator IControllable.ThrowSpear(Vector2 throwDir){
		throwing = true;
		highLow = throwDir.y<=.2f ? HighLow.Low : HighLow.High;
		if (highLow == HighLow.Low){
			ThrowState = throwDir.x<=0 ? Throw.DownLeft : Throw.DownRight;
		}
		else {
			ThrowState = throwDir.x<=0 ? Throw.UpLeft : Throw.UpRight;
		}

		StartCoroutine (((IThrowable)(Spear.Instance)).FlyFree(throwDir, throwForce, (int)highLow));

		jaiAnimator.SetInteger("AnimState",(int)ThrowState);
		yield return new WaitForSeconds (Constants.time2ThrowSpear);
		throwing = false;
		jaiAnimator.SetInteger("AnimState",(int)Throw.Idle);
		throws++;
		yield return null;
	}

	IEnumerator IControllable.StabTheBeast(){
		stabbing = true;
		//jaiAnimator.SetInteger("AnimState",5);
		Tentacles.StabbableTentacle.GetStabbed(); //stab the tentacle!
		yield return new WaitForSeconds (.1f);
		stabbing = false;
		//jaiAnimator.SetInteger("AnimState",0);
		yield return null;
	}
	#endregion

	#region ISpawnable
	IEnumerator ISpawnable.PullOutNewSpear(){
		yield return new WaitForSeconds (Constants.time2ThrowSpear);
		Instantiate (spear, transform.position + (Vector3)Constants.stockSpearPosition, Quaternion.identity);
	}
	#endregion
}