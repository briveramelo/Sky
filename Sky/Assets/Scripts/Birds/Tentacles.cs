using UnityEngine;
using System.Collections;
using GenericFunctions;

public class Tentacles : Bird, ISensorToTentacle, IStabbable {

	public static IStabbable StabbableTentacle;
	private ISensorToTentacle me; //just because I wanted to use ResetPosition locally with less mess...

	[SerializeField] private Transform tipTransform;
	[SerializeField] private Collider2D tentaCollider;
	[SerializeField] private TentaclesSensor ts; private ITentacleToSensor tentacleToSensor;

	private Vector2 homeSpot = new Vector2 (0f,-.75f - Constants.worldDimensions.y);
	
	private float descendSpeed = 1f;
	private float attackSpeed = 1.5f;
	private float resetSpeed = 1f;
	private float defeatedHeight;
	private float resetHeight;

	private int stabsTaken;
	private int stabs2Retreat = 5;

	private bool tentaclesDisabled;

	void Awake(){
		StabbableTentacle = (IStabbable)this;
		me = (ISensorToTentacle)this;
		tentacleToSensor = (ITentacleToSensor)ts;

		birdStats = new BirdStats(BirdType.Tentacles);

		resetHeight = .5f + homeSpot.y;
		defeatedHeight = .25f + homeSpot.y;
	}

	void FaceTowardYou(bool toward){
		transform.Face4ward (toward ? (Constants.basketTransform.position.x - transform.position.x) > 0 : (Constants.basketTransform.position.x - transform.position.x) < 0);
	}

	#region ISensorToTentacle
	IEnumerator ISensorToTentacle.GoForTheKill(){ 
		tentaCollider.enabled = true;
		while (tentacleToSensor.JaiInRange && !Jai.JaiLegs.BeingHeld){
			rigbod.velocity = (Constants.basketTransform.position - tipTransform.position).normalized * attackSpeed;
			FaceTowardYou(true);
			yield return null;
		}
	}

	IEnumerator ISensorToTentacle.ResetPosition(bool defeated){
		float finishHeight = defeated ? resetHeight: defeatedHeight;

		while (tipTransform.position.y>finishHeight && (defeated ? true : !tentacleToSensor.JaiInRange)){
			rigbod.velocity = ((Vector3)homeSpot - tipTransform.position).normalized * resetSpeed;
			FaceTowardYou(!defeated);
			yield return null;
		}
		if (defeated) tentaCollider.enabled = false;
		rigbod.velocity = Vector2.zero;
		tentacleToSensor.ToggleSensor(true);
		yield return null;
	}
	#endregion

	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.layer == Constants.jaiLayer && !Jai.JaiLegs.BeingHeld && !tentaclesDisabled){
			StartCoroutine (PullDownTheKill());
		}
	}

	IEnumerator PullDownTheKill(){
		stabsTaken = 0;
		Jai.JaiLegs.BeingHeld = true;
		Basket.TentacleToBasket.AttachToTentacles(transform);

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

	#region IStabbable
	void IStabbable.GetStabbed(){
		stabsTaken++;
		TakeDamage(Vector2.zero,tentaCollider);
		if (stabsTaken>=stabs2Retreat){
			tentacleToSensor.ToggleSensor(false);
			StartCoroutine ( DisableTentacles());
			StartCoroutine ( me.ResetPosition(true));
			Jai.JaiLegs.BeingHeld = false;
			Basket.TentacleToBasket.DetachFromTentacles();
			Basket.TentacleToBasket.KnockDown(5f);
		}
	}
	#endregion

	IEnumerator DisableTentacles(){
		tentaclesDisabled = true;
		yield return new WaitForSeconds (1.5f);
		tentaclesDisabled = false;
	}

	#region TakeDamage
	public override void TakeDamage(Vector2 gutDirection, Collider2D spearCollider){
		Vector2 gutSpawnSpot = transform.position;
		bool holding = gutDirection.x==0;
		if (!holding){
			birdStats.Health--;
		}

		if (holding){
			gutSpawnSpot = tipTransform.position;
			gutDirection = new Vector2 (-Mathf.Sign(transform.localScale.x) * Random.value, Random.value).normalized;
		}
		else{
			gutSpawnSpot = birdCollider.bounds.ClosestPoint(spearCollider.transform.position);
		}
		GutSplosion gutSplosion = (Instantiate (gutSplosionParent, gutSpawnSpot, Quaternion.identity) as GameObject).GetComponent<GutSplosion>();

		if (birdStats.Health>0){
			gutSplosion.GenerateGuts (birdStats.DamageGutValue, gutDirection);
		}
		else{
			gutSplosion.GenerateGuts (birdStats.KillGutValue, gutDirection);
			GameClock.Instance.SlowTime(.1f,.5f);
			PayTheIronPrice();
			TrackPoints();
			Destroy(transform.parent.gameObject);
		}
	}
	#endregion

	protected override void OnDestroy(){
		base.OnDestroy();
		StopAllCoroutines();
		if (Constants.bottomOfTheWorldCollider!=null){
			Constants.bottomOfTheWorldCollider.enabled = true;
		}
	}
}