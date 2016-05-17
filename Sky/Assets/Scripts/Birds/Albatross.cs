using UnityEngine;
using GenericFunctions;

public class Albatross : Bird {

    bool shouldWaitToTurn;
	const float moveSpeed = 1.065f;

	void Update () {
        Vector2 moveDir = Constants.balloonCenter.position - transform.position;
        if (Vector2.Distance(moveDir, Vector2.zero)>0.1f) {
            rigbod.velocity = (moveDir).normalized * moveSpeed;
            transform.FaceForward(rigbod.velocity.x<0);
        }
        else {
            rigbod.velocity = Vector2.Lerp(rigbod.velocity, Vector2.zero, 0.1f);
            if (!shouldWaitToTurn) {
                StartCoroutine(Bool.Toggle(theBool => shouldWaitToTurn = !theBool, .75f));
                transform.FaceForward(Bool.TossCoin());
            }
        }
	}
		
	protected override int TakeDamage (ref WeaponStats weaponStats){
		float hitHeight = birdCollider.bounds.ClosestPoint(weaponStats.WeaponCollider.transform.position).y;
        int damageToTake = 3;
        int damageDealt;
        if (weaponStats.Velocity.y > 0 && hitHeight < transform.position.y){ //kill albatross with a tactical shot to the underbelly
            damageDealt = birdStats.Health >= damageToTake ? damageToTake : birdStats.Health;
        }
        else {
            damageDealt = weaponStats.Damage;
        }
        birdStats.Health -= damageDealt;
        (Instantiate(guts, transform.position, Quaternion.identity) as GameObject).GetComponent<IBleedable>().GenerateGuts(ref birdStats, weaponStats.Velocity);
        Debug.Log(damageDealt);
        return damageDealt;
	}
}