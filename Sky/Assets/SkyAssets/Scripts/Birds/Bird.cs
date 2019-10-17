using UnityEngine;

public interface IHurtable {
	void GetHurt(ref WeaponStats weaponStats);
}

public abstract class Bird : MonoBehaviour, IHurtable {

	protected BirdStats birdStats; public BirdStats MyBirdStats => birdStats;

	[SerializeField] BirdType myBirdType;
	[SerializeField] protected Rigidbody2D rigbod;
	[SerializeField] protected Collider2D birdCollider;
	[SerializeField] protected GameObject guts;

	protected virtual void Awake(){
        birdStats = new BirdStats(myBirdType);
        ScoreSheet.Tallier.TallyBirth(ref birdStats);
        ScoreSheet.Tallier.TallyBirdThreat(ref birdStats, BirdThreat.Spawn);
    }

	void IHurtable.GetHurt(ref WeaponStats weaponStats) {
        birdStats.DamageTaken = TakeDamage(ref weaponStats);
        birdStats.BirdPosition = transform.position;
		birdStats.ModifyForStreak(ScoreSheet.Streaker.GetHitStreak());
		birdStats.ModifyForCombo(weaponStats.BirdsHit);
		ScoreSheet.Tallier.TallyPoints (ref birdStats);
        ScoreSheet.Tallier.TallyBirdThreat(ref birdStats, BirdThreat.Damage);
        if (birdStats.Health<=0){
			GameClock.Instance.SlowTime(.1f,.5f);
			ScoreSheet.Tallier.TallyKill (ref birdStats);
            DieUniquely();
		}
	}

	protected virtual int TakeDamage(ref WeaponStats weaponStats){
        int damageDealt = Mathf.Clamp(weaponStats.Damage, 0, birdStats.Health);
        birdStats.Health -= damageDealt;
		Instantiate (guts, transform.position, Quaternion.identity).GetComponent<IBleedable>().GenerateGuts(ref birdStats, weaponStats.Velocity);
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
