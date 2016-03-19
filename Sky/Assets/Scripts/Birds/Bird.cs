using UnityEngine;
using System.Collections;

public interface IHurtable {
	void GetHurt(SpearItems spearItems);
}

public abstract class Bird : MonoBehaviour, IHurtable {

	protected BirdStats birdStats; public BirdStats MyBirdStats{get{return birdStats;}}

	[SerializeField] protected Rigidbody2D rigbod;
	[SerializeField] protected Collider2D birdCollider;
	[SerializeField] protected GameObject guts;

	protected virtual void Awake(){
		ScoreSheet.Tallier.TallyBirth(birdStats);
	}

	void IHurtable.GetHurt(SpearItems spearItems){
		TakeDamage(spearItems);
		birdStats.birdPosition = transform.position;
		birdStats.ModifyForStreak(ScoreSheet.Streaker.GetHitStreak());
		birdStats.ModifyForCombo(spearItems.BirdsHit);
		ScoreSheet.Tallier.TallyPoints (birdStats);
		if (birdStats.Health<=0){
			GameClock.Instance.SlowTime(.1f,.5f);
			ScoreSheet.Tallier.TallyKill (birdStats);
			DieUniquely();
		}
	}

	protected virtual void TakeDamage(SpearItems spearItems){
		birdStats.Health--;
		(Instantiate (guts, transform.position, Quaternion.identity) as GameObject).GetComponent<IBleedable>().GenerateGuts(birdStats, spearItems.SpearVelocity);
	}

	protected virtual void DieUniquely(){
		Destroy(gameObject);
	}

	protected void OnDestroy(){
		if (ScoreSheet.Instance){
			ScoreSheet.Tallier.TallyDeath (birdStats);
		}
		StopAllCoroutines();
	}
}
