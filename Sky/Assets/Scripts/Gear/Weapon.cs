using UnityEngine;
using System.Collections;
using GenericFunctions;

public interface IUsable {
	void UseMe(PointVector2 spotSwipe);
}
public struct PointVector2 {
    public Vector2 origin;
    public Vector2 destination;
    public Vector2 vector;
    public PointVector2(Vector2 origin, Vector2 destination){
        this.origin = origin;
        this.destination = destination;
        this.vector = (destination - origin).normalized;
    }
}

public abstract class Weapon : MonoBehaviour, IUsable {

    [SerializeField] protected Collider2D attackCollider;
    [SerializeField] AudioClip useSound;
    [SerializeField] WeaponType MyWeaponType;
    protected WeaponStats MyWeaponStats = new WeaponStats();
    protected int birdsHit;
    protected static int timesUsed;
    protected abstract int weaponNumber {get; }

    void IUsable.UseMe(PointVector2 spotSwipe) { UseMe(spotSwipe); }

    void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.layer == Constants.birdLayer){
            Hit();
            DeliverDamage(col);
		}
        else if (col.gameObject.layer == Constants.balloonFloatingLayer) {
            Hit();
            PopBalloon(col);
        }
	}
    void Hit() {
		birdsHit++;
        MyWeaponStats.ReDefineWeapon(attackCollider, MyVelocity, birdsHit, MyWeaponType);
		ScoreSheet.Streaker.ReportHit(timesUsed);
    }
    void PopBalloon(Collider2D col) {
        col.GetComponent<Balloon>().Pop();
    }

    protected abstract Vector2 MyVelocity { get;}
    /// <summary>
    /// increments "timesUsed" and plays audio clip
    /// </summary>
    protected virtual void UseMe(PointVector2 spotSwipe) {
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
