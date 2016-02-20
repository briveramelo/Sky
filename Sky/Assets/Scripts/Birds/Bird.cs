using UnityEngine;
using System.Collections;

public abstract class Bird : MonoBehaviour, IHurtable {

	protected BirdStats birdStats; public BirdStats MyBirdStats{get{return birdStats;}}

	[SerializeField] protected Rigidbody2D rigbod;
	[SerializeField] protected Collider2D birdCollider;
	[SerializeField] protected GameObject gutSplosionParent;

	public virtual void TakeDamage(Vector2 gutDirection, Collider2D spearCollider){
		birdStats.Health--;
		Vector2 gutSpawnSpot = transform.position;
		GutSplosion gutSplosion = (Instantiate (gutSplosionParent, gutSpawnSpot, Quaternion.identity) as GameObject).GetComponent<GutSplosion>();

		if (birdStats.Health>0){
			gutSplosion.GenerateGuts (birdStats.DamageGutValue, gutDirection);
			TrackPoints();
		}
		else{
			gutSplosion.GenerateGuts (birdStats.KillGutValue, gutDirection);
			GameClock.Instance.SlowTime(.1f,.5f);
			PayTheIronPrice();
			TrackPoints();
			Destroy(gameObject);
		}
	}

	protected virtual void PayTheIronPrice(){}

	protected void TrackPoints(){
		if (birdStats.Health>0){
			WaveManager.Instance.AddPoints ((int)birdStats.MyBirdType,birdStats.DamagePointValue,0);
		}
		else{
			WaveManager.Instance.AddPoints ((int)birdStats.MyBirdType,birdStats.KillPointValue,birdStats.KillPointMultiplier);
			WaveManager.Instance.Kill ((int)birdStats.MyBirdType);
		}
	}

	protected virtual void OnDestroy(){
		if (WaveManager.Instance){
			WaveManager.Instance.Death ((int)birdStats.MyBirdType);
		}
	}
}
