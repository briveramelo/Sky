using UnityEngine;

public class Flail : Weapon
{
    protected override int WeaponNumber => TimesUsed;
    [SerializeField] private Rigidbody2D _rigbod;
    protected override Vector2 MyVelocity => _rigbod.velocity;

    protected override void DeliverDamage(Collider2D col)
    {
        base.DeliverDamage(col);
        //todo: add code here
    }

    protected override void UseMe(Vector2 startPosition, Vector2 swipeDir)
    {
        base.UseMe(startPosition, swipeDir);
        Debug.Log("FLAILED!");
    }
}