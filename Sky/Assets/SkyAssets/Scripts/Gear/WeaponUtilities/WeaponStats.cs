using UnityEngine;

public class WeaponStats
{
    public WeaponStats(Collider2D myCollider, Vector2 velocity, int birdsHit, WeaponType myWeaponType)
    {
        WeaponCollider = myCollider;
        Velocity = velocity;
        BirdsHit = birdsHit;
        MyWeaponType = myWeaponType;
    }

    public void ReDefineWeapon(Collider2D myCollider, Vector2 velocity, int birdsHit, WeaponType myWeaponType)
    {
        WeaponCollider = myCollider;
        Velocity = velocity;
        BirdsHit = birdsHit;
        MyWeaponType = myWeaponType;
    }

    public WeaponStats()
    {
    }

    public WeaponType MyWeaponType { get; private set; }
    public Collider2D WeaponCollider { get; private set; }
    public Vector2 Velocity { get; private set; }
    public int BirdsHit { get; private set; }

    public int Damage
    {
        get
        {
            switch (MyWeaponType)
            {
                case WeaponType.Spear:
                    return 1;
                case WeaponType.Lightning:
                    return 2;
                case WeaponType.Flail:
                    return 1;
                case WeaponType.None:
                    return 0;
            }

            return 0;
        }
    }
}