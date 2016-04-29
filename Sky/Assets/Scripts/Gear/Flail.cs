using UnityEngine;
using System.Collections;

public class Flail : Weapon {

    protected override int weaponNumber {get {return timesUsed; } }
    [SerializeField] Rigidbody2D rigbod;
    protected override Vector2 MyVelocity { get { return rigbod.velocity; } }

    protected override void DeliverDamage(Collider2D col) {
        base.DeliverDamage(col);
        //code here

    }

    protected override void UseMe(PointVector2 spotSwipe) {
        base.UseMe(spotSwipe);
        Debug.Log("FLAILED!");
    }

}
