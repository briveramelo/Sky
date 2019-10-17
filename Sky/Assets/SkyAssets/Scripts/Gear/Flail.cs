using UnityEngine;

public class Flail : Weapon {

    protected override int weaponNumber => timesUsed;
    [SerializeField] Rigidbody2D rigbod;
    protected override Vector2 MyVelocity => rigbod.velocity;

    protected override void DeliverDamage(Collider2D col) {
        base.DeliverDamage(col);
        //todo: add code here

    }

    protected override void UseMe(Vector2 swipeDir) {
        base.UseMe(swipeDir);
        Debug.Log("FLAILED!");
    }

}
