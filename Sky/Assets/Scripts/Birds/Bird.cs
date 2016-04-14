using UnityEngine;
using System.Collections;

public interface IHurtable {
	void GetHurt(ref SpearItems spearItems);
}

public abstract class Bird : MonoBehaviour, IHurtable {

	protected BirdStats birdStats; public BirdStats MyBirdStats{get{return birdStats;}}

	[SerializeField] protected Rigidbody2D rigbod;
	[SerializeField] protected Collider2D birdCollider;
	[SerializeField] protected GameObject guts;

	protected virtual void Awake(){
		ScoreSheet.Tallier.TallyBirth(ref birdStats);
        ScoreSheet.Tallier.TallyThreat(ref birdStats, birdStats.TotalThreatValue);
    }

	void IHurtable.GetHurt(ref SpearItems spearItems){
        int threatEliminated = birdStats.TotalThreatValue;
        int damageDealt = TakeDamage(ref spearItems);
        if (birdStats.Health > 0) {
            threatEliminated = damageDealt * birdStats.DamageThreat;
        }
        birdStats.BirdPosition = transform.position;
		birdStats.ModifyForStreak(ScoreSheet.Streaker.GetHitStreak());
		birdStats.ModifyForCombo(spearItems.BirdsHit);
		birdStats.ModifyForMultiplier();
		ScoreSheet.Tallier.TallyPoints (ref birdStats);
        ScoreSheet.Tallier.TallyThreat(ref birdStats, -threatEliminated);
        if (birdStats.Health<=0){
			GameClock.Instance.SlowTime(.1f,.5f);
			ScoreSheet.Tallier.TallyKill (ref birdStats);
            DieUniquely();
		}
	}

	protected virtual int TakeDamage(ref SpearItems spearItems){
		(Instantiate (guts, transform.position, Quaternion.identity) as GameObject).GetComponent<IBleedable>().GenerateGuts(ref birdStats, spearItems.SpearVelocity);
        int damageDealt = Mathf.Clamp(spearItems.Damage, 0, birdStats.Health);
        birdStats.Health -= damageDealt;
        return damageDealt;
    }

	protected virtual void DieUniquely(){
		Destroy(gameObject);
	}

	protected void OnDestroy(){
		if (ScoreSheet.Instance){
			ScoreSheet.Tallier.TallyDeath (ref birdStats);
            if (birdStats.Health > 0) {
                ScoreSheet.Tallier.TallyThreat(ref birdStats, -birdStats.TotalThreatValue);
            }
        }
		StopAllCoroutines();
	}
}
