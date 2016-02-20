using UnityEngine;
using System.Collections;
using GenericFunctions;

public class Albatross : Bird {

	private float moveSpeed = .71f;

	void Awake(){
		birdStats = new BirdStats(BirdType.Albatross);
	}

	void Update () {
		rigbod.velocity = (Constants.balloonCenter.position - transform.position).normalized * moveSpeed;
		transform.Face4ward(rigbod.velocity.x<0);
	}

	public override void TakeDamage (Vector2 gutDirection, Collider2D spearCollider){
		birdStats.Health--;
		Vector2 gutSpawnSpot = transform.position;
		GutSplosion gutSplosion = (Instantiate (gutSplosionParent, gutSpawnSpot, Quaternion.identity) as GameObject).GetComponent<GutSplosion>();

		Vector2 hitPoint = birdCollider.bounds.ClosestPoint(spearCollider.transform.position);
		if (gutDirection.y>0 && hitPoint.y<transform.position.y && hitPoint.x>birdCollider.bounds.min.x && hitPoint.x<birdCollider.bounds.max.x){ //kill albatross with a tactical shot to the underbelly
			birdStats.Health = 0;
			//super kill!
			//take a bite out of that soft, vulnerable tummy
		}

		if (birdStats.Health>0){
			gutSplosion.GenerateGuts (birdStats.DamageGutValue, gutDirection);
		}
		else{
			gutSplosion.GenerateGuts (birdStats.KillGutValue, gutDirection);
			GameClock.Instance.SlowTime(.1f,.5f);
			PayTheIronPrice();
			TrackPoints();
			Destroy(gameObject);
		}
	}
}