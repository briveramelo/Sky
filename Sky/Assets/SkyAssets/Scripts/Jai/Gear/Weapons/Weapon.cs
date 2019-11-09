using UnityEngine;
using GenericFunctions;

public interface IUsable
{
    void UseMe(Vector2 spotSwipe);
}

public abstract class Weapon : MonoBehaviour, IUsable
{
    [SerializeField] private WeaponType _myWeaponType;
    [SerializeField] protected Collider2D _attackCollider;
    [SerializeField] private AudioClipType _useSound;

    protected WeaponStats MyWeaponStats = new WeaponStats();
    protected int BirdsHit;
    protected static int TimesUsed;
    protected abstract int WeaponNumber { get; }

    void IUsable.UseMe(Vector2 spotSwipe)
    {
        UseMe(spotSwipe);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == Layers.BirdLayer)
        {
            Hit();
            DeliverDamage(col);
        }
        else if (col.gameObject.layer == Layers.BalloonFloatingLayer)
        {
            Hit();
            PopBalloon(col);
        }
    }

    private void Hit()
    {
        BirdsHit++;
        MyWeaponStats.ReDefineWeapon(_attackCollider, MyVelocity, BirdsHit, _myWeaponType);
        ScoreSheet.Streaker.ReportHit(TimesUsed);
    }

    private void PopBalloon(Collider2D col)
    {
        col.GetComponent<Balloon>().Pop();
    }

    protected abstract Vector2 MyVelocity { get; }

    /// <summary>
    /// increments "timesUsed" and plays audio clip
    /// </summary>
    protected virtual void UseMe(Vector2 swipeDir)
    {
        TimesUsed++;
        AudioManager.PlayAudio(_useSound);
    }

    /// <summary>
    /// calls the collider's IHurtable GetHurt(ref MyWeaponStats)
    /// </summary>
    protected virtual void DeliverDamage(Collider2D col)
    {
        col.GetComponent<IHurtable>().GetHurt(ref MyWeaponStats);
    }
}