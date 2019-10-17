﻿using UnityEngine;
using System.Collections;
using GenericFunctions;

public interface IStabbable {
	void GetStabbed();
}
public interface ISensorToTentacle {
	IEnumerator GoForTheKill();
	IEnumerator ResetPosition(bool defeated);
}
public interface ITipToTentacle{
	IEnumerator PullDownTheKill();
}
public interface IReleasable {
    void ReleaseJai();
}

public class Tentacles : Bird, ISensorToTentacle, IStabbable, ITipToTentacle, IReleasable {

    public static Tentacles Instance;
	public static IStabbable StabbableTentacle;
    public static IReleasable Releaser;
	ISensorToTentacle me; //just because I wanted to use ResetPosition locally with less mess...
	[SerializeField] TentaclesSensor ts; private IToggleable sensor; private IJaiDetected sensorOnJai;
	IFreezable inputManager;
	IFreezable jai;

	[SerializeField] Transform tipTransform;
	[SerializeField] Collider2D tipCollider;
	WeaponStats fakeWeapon = new WeaponStats();

	Vector2 homeSpot = new Vector2 (0f,-.75f - Constants.WorldDimensions.y);
	
	float descendSpeed = 1f;
	float attackSpeed = 1.5f;
	float resetSpeed = 1f;
	float defeatedHeight;
	float resetHeight;

	int stabsTaken;
	const int stabs2Retreat = 4;

	private bool holdingJai;

	protected override void Awake(){
		base.Awake();

        Instance = this;
        StabbableTentacle = this;
        Releaser = this;
		me = this;
		sensor = ts;
		sensorOnJai = ts;
		inputManager = FindObjectOfType<InputManager>().GetComponent<IFreezable>();
		jai = FindObjectOfType<Jai>().GetComponent<IFreezable>();

		resetHeight = .5f + homeSpot.y;
		defeatedHeight = .25f + homeSpot.y;
	}

	void FaceTowardYou(bool toward){
		transform.FaceForward (toward ? (Constants.basketTransform.position.x - transform.position.x) > 0 : (Constants.basketTransform.position.x - transform.position.x) < 0);
	}

	#region ISensorToTentacle
	IEnumerator ISensorToTentacle.GoForTheKill(){ 
		while (sensorOnJai.JaiInRange && !holdingJai){
			rigbod.velocity = (Constants.basketTransform.position -Vector3.one*0.2f- tipTransform.position).normalized * attackSpeed;
			FaceTowardYou(true);
			yield return null;
		}
	}

	IEnumerator ISensorToTentacle.ResetPosition(bool defeated){
		float finishHeight = defeated ? resetHeight: defeatedHeight;

		while (tipTransform.position.y>finishHeight && (defeated ? true : !sensorOnJai.JaiInRange)){
			rigbod.velocity = ((Vector3)homeSpot - tipTransform.position).normalized * resetSpeed;
			FaceTowardYou(!defeated);
			yield return null;
		}
		rigbod.velocity = Vector2.zero;
		sensor.ToggleSensor(true);
	}
	#endregion

	#region ITipToTentacle
	IEnumerator ITipToTentacle.PullDownTheKill(){
		stabsTaken = 0;
		holdingJai = true;
		inputManager.IsFrozen = true;
		jai.IsFrozen = true;
		Basket.TentacleToBasket.AttachToTentacles(transform);
        ScoreSheet.Tallier.TallyThreat(Threat.BasketGrabbed);

        GameClock.Instance.SlowTime (0.4f, 0.5f);
		GameCamera.Instance.ShakeTheCamera();
		Constants.bottomOfTheWorldCollider.enabled = false;

		while (tipTransform.position.y>defeatedHeight && stabsTaken<stabs2Retreat){
			rigbod.velocity = Vector2.down * descendSpeed;
			yield return null;
		}
		if (stabsTaken<stabs2Retreat){
			Basket.TentacleToBasket.LoseAllBalloons();
		}

		Constants.bottomOfTheWorldCollider.enabled = true;
	}
	#endregion

	#region IStabbable
	void IStabbable.GetStabbed(){
        stabsTaken++;
		TakeDamage(ref fakeWeapon);
		if (stabsTaken>=stabs2Retreat){
            ReleaseBasket();
        }
	}
    #endregion

    void IReleasable.ReleaseJai() {
        if (holdingJai) {
            ReleaseBasket();
        }
    }

    void ReleaseBasket() {
        holdingJai = false;
        inputManager.IsFrozen = false;
        jai.IsFrozen = false;
        Basket.TentacleToBasket.DetachFromTentacles();
        Basket.TentacleToBasket.KnockDown(5f);

        sensor.ToggleSensor(false);
        StartCoroutine(DisableTentacles());
        StartCoroutine(me.ResetPosition(true));
    }

	IEnumerator DisableTentacles(){
		tipCollider.enabled = false;
		yield return new WaitForSeconds (1.5f);
		tipCollider.enabled = true;
	}

	#region TakeDamage
	protected override int TakeDamage(ref WeaponStats weaponStats){
        bool holding = weaponStats.Velocity == Vector2.zero;
		Vector2 spawnSpot;
		Vector2 gutVel;
        int damageDealt;
		if (holding){
			gutVel = new Vector2 (-Mathf.Sign(transform.localScale.x) * Random.value, Random.value).normalized;
			spawnSpot = tipTransform.position;
            damageDealt = 0;
        }
		else{
			gutVel = weaponStats.Velocity;
			spawnSpot = birdCollider.bounds.ClosestPoint(weaponStats.WeaponCollider.transform.position);
            damageDealt = weaponStats.Damage;
		}

        birdStats.Health -= damageDealt;
		(Instantiate (guts, spawnSpot, Quaternion.identity)).GetComponent<IBleedable>().GenerateGuts(ref birdStats, gutVel);
        return damageDealt;
	}
	#endregion

	protected override void DieUniquely(){
		if (Constants.bottomOfTheWorldCollider!=null){
			Constants.bottomOfTheWorldCollider.enabled = true;
		}
        if (holdingJai) {
            ReleaseBasket();
        }
		Destroy(transform.parent.gameObject);
	}
}