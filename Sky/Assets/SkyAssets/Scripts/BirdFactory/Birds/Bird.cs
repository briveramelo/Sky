﻿using UnityEngine;

public interface IHurtable
{
    void GetHurt(ref WeaponStats weaponStats);
}

public interface IDeathDebug
{
    void KillDebug();
}

public abstract class Bird : MonoBehaviour, IHurtable, IDeathDebug
{
    protected BirdStats BirdStats;
    protected abstract BirdType MyBirdType { get; }

    [SerializeField] protected Rigidbody2D _rigbod;
    [SerializeField] protected Collider2D _birdCollider;
    [SerializeField] protected GameObject _guts;

    protected virtual void Awake()
    {
        BirdStats = new BirdStats(MyBirdType);
        ScoreSheet.Tallier.TallyBirth(ref BirdStats);
        ScoreSheet.Tallier.TallyBirdThreat(ref BirdStats, BirdThreat.Spawn);
    }

    void IHurtable.GetHurt(ref WeaponStats weaponStats)
    {
        BirdStats.DamageTaken = TakeDamage(ref weaponStats);
        BirdStats.BirdPosition = transform.position;
        BirdStats.ModifyForStreak(ScoreSheet.Streaker.GetHitStreak());
        BirdStats.ModifyForCombo(weaponStats.BirdsHit);
        ScoreSheet.Tallier.TallyPoints(ref BirdStats);
        ScoreSheet.Tallier.TallyBirdThreat(ref BirdStats, BirdThreat.Damage);
        if (BirdStats.Health <= 0)
        {
            GameClock.Instance.SlowTime(.1f, .8f);
            ScoreSheet.Tallier.TallyKill(ref BirdStats);
            OnDeath();
        }
    }

    protected virtual int TakeDamage(ref WeaponStats weaponStats)
    {
        var damageDealt = Mathf.Clamp(weaponStats.Damage, 0, BirdStats.Health);
        BirdStats.Health -= damageDealt;
        Instantiate(_guts, transform.position, Quaternion.identity).GetComponent<IBleedable>().GenerateGuts(ref BirdStats, weaponStats.Velocity);
        return damageDealt;
    }

    protected virtual void OnDeath()
    {
        Destroy(gameObject);
    }

    protected virtual void OnDestroy()
    {
        if (ScoreSheet.Instance)
        {
            ScoreSheet.Tallier.TallyDeath(ref BirdStats);
            if (BirdStats.Health > 0)
            {
                ScoreSheet.Tallier.TallyBirdThreat(ref BirdStats, BirdThreat.Leave);
            }
        }

        StopAllCoroutines();
    }

    void IDeathDebug.KillDebug()
    {
        OnDeath();
    }
}