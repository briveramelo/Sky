using UnityEngine;
using System.Collections;
using GenericFunctions;

public class Albatross : Bird {

	const float moveSpeed = .71f;

	void Update () {
		rigbod.velocity = (Constants.balloonCenter.position - transform.position).normalized * moveSpeed;
		transform.FaceForward(rigbod.velocity.x<0);
	}
		
	protected override int TakeDamage (ref WeaponStats weaponStats){
		float hitHeight = birdCollider.bounds.ClosestPoint(weaponStats.WeaponCollider.transform.position).y;
        int damageDealt;
        if (weaponStats.Velocity.y > 0 && hitHeight < transform.position.y){ //kill albatross with a tactical shot to the underbelly
            damageDealt = birdStats.Health;
        }
        else {
            damageDealt = weaponStats.Damage;
        }
        birdStats.Health -= damageDealt;
        (Instantiate(guts, transform.position, Quaternion.identity) as GameObject).GetComponent<IBleedable>().GenerateGuts(ref birdStats, weaponStats.Velocity);
        return damageDealt;
	}
}