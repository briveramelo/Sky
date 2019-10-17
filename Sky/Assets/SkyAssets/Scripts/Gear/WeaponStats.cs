using UnityEngine;

public class WeaponStats {

	public WeaponStats(Collider2D myCollider, Vector2 velocity, int birdsHit, WeaponType MyWeaponType) {
        this.myCollider = myCollider;
        this.velocity = velocity;
        this.birdsHit = birdsHit;
        this.myWeaponType = MyWeaponType;
    }
    public void ReDefineWeapon(Collider2D myCollider, Vector2 velocity, int birdsHit, WeaponType MyWeaponType) {
        this.myCollider = myCollider;
        this.velocity = velocity;
        this.birdsHit = birdsHit;
        this.myWeaponType = MyWeaponType;
    }
    public WeaponStats() { }

    WeaponType myWeaponType; public WeaponType MyWeaponType => myWeaponType;
    Collider2D myCollider; public Collider2D WeaponCollider => myCollider;
    Vector2 velocity; public Vector2 Velocity => velocity;
    int birdsHit; public int BirdsHit => birdsHit;

    public int Damage {
        get {
            switch (MyWeaponType) {
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
