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
        ScoreSheet.Tallier.TallyBirdThreat(ref birdStats, BirdThreat.Spawn);
    }

	void IHurtable.GetHurt(ref SpearItems spearItems) {
        birdStats.DamageTaken = TakeDamage(ref spearItems);
        birdStats.BirdPosition = transform.position;
		birdStats.ModifyForStreak(ScoreSheet.Streaker.GetHitStreak());
		birdStats.ModifyForCombo(spearItems.BirdsHit);
		ScoreSheet.Tallier.TallyPoints (ref birdStats);
        ScoreSheet.Tallier.TallyBirdThreat(ref birdStats, BirdThreat.Damage);
        if (birdStats.Health<=0){
			GameClock.Instance.SlowTime(.1f,.5f);
			ScoreSheet.Tallier.TallyKill (ref birdStats);
            DieUniquely();
		}
	}

	protected virtual int TakeDamage(ref SpearItems spearItems){
        int damageDealt = Mathf.Clamp(spearItems.Damage, 0, birdStats.Health);
        birdStats.Health -= damageDealt;
		(Instantiate (guts, transform.position, Quaternion.identity) as GameObject).GetComponent<IBleedable>().GenerateGuts(ref birdStats, spearItems.SpearVelocity);
        return damageDealt;
    }

	protected virtual void DieUniquely(){
		Destroy(gameObject);
	}

	protected void OnDestroy(){
		if (ScoreSheet.Instance){
			ScoreSheet.Tallier.TallyDeath (ref birdStats);
            if (birdStats.Health > 0) {
                ScoreSheet.Tallier.TallyBirdThreat(ref birdStats, BirdThreat.Leave);
            }
        }
		StopAllCoroutines();
	}
}
