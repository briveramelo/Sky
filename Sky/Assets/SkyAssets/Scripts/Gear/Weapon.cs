using UnityEngine;
using GenericFunctions;

public interface IUsable {
	void UseMe(Vector2 spotSwipe);
}

public abstract class Weapon : MonoBehaviour, IUsable {

    [SerializeField] private WeaponType MyWeaponType;
    [SerializeField] protected Collider2D attackCollider;
    [SerializeField] private AudioClip useSound;
    protected WeaponStats MyWeaponStats = new WeaponStats();
    protected int birdsHit;
    protected static int timesUsed;
    protected abstract int weaponNumber {get; }

    void IUsable.UseMe(Vector2 spotSwipe) { UseMe(spotSwipe); }

    private void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.layer == Constants.birdLayer){
            Hit();
            DeliverDamage(col);
		}
        else if (col.gameObject.layer == Constants.balloonFloatingLayer) {
            Hit();
            PopBalloon(col);
        }
	}

    private void Hit() {
		birdsHit++;
        MyWeaponStats.ReDefineWeapon(attackCollider, MyVelocity, birdsHit, MyWeaponType);
		ScoreSheet.Streaker.ReportHit(timesUsed);
    }

    private void PopBalloon(Collider2D col) {
        col.GetComponent<Balloon>().Pop();
    }

    protected abstract Vector2 MyVelocity { get;}
    /// <summary>
    /// increments "timesUsed" and plays audio clip
    /// </summary>
    protected virtual void UseMe(Vector2 swipeDir) {
        timesUsed++;
        AudioManager.PlayAudio(useSound);
    }
    /// <summary>
    /// calls the collider's IHurtable GetHurt(ref MyWeaponStats)
    /// </summary>
    protected virtual void DeliverDamage(Collider2D col) {
        col.GetComponent<IHurtable>().GetHurt(ref MyWeaponStats);
    }
}
