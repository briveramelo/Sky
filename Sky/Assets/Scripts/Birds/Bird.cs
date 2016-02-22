using UnityEngine;
using System.Collections;

public abstract class Bird : MonoBehaviour, IHurtable {

	protected BirdStats birdStats; public BirdStats MyBirdStats{get{return birdStats;}}

	[SerializeField] protected Rigidbody2D rigbod;
	[SerializeField] protected Collider2D birdCollider;
	[SerializeField] protected GameObject gutSplosionParent;

	protected virtual void Awake(){
		WaveEngine.ScoreBoard.TallyBirth(MyBirdStats.MyBirdType);
	}

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
			WaveEngine.ScoreBoard.TallyPoints (birdStats.MyBirdType,birdStats.DamagePointValue,0);
		}
		else{
			WaveEngine.ScoreBoard.TallyPoints (birdStats.MyBirdType,birdStats.KillPointValue,birdStats.KillPointMultiplier);
			WaveEngine.ScoreBoard.TallyKill (birdStats.MyBirdType);
		}
	}

	protected virtual void OnDestroy(){
		if (WaveEngine.Instance){
			WaveEngine.ScoreBoard.TallyDeath (birdStats.MyBirdType);
		}
	}
}
