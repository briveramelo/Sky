using UnityEngine;
using System.Collections;
using GenericFunctions;

public class Albatross : Bird {

	private float moveSpeed = .71f;

	protected override void Awake(){
		birdStats = new BirdStats(BirdType.Albatross);
		base.Awake();
	}

	void Update () {
		rigbod.velocity = (Constants.balloonCenter.position - transform.position).normalized * moveSpeed;
		transform.FaceForward(rigbod.velocity.x<0);
	}
		
	protected override int TakeDamage (ref SpearItems spearItems){
		float hitHeight = birdCollider.bounds.ClosestPoint(spearItems.SpearCollider.transform.position).y;
        int damageDealt;
        if (spearItems.SpearVelocity.y > 0 && hitHeight < transform.position.y){ //kill albatross with a tactical shot to the underbelly
            damageDealt = birdStats.Health;
        }
        else {
            damageDealt = spearItems.Damage;
        }
        birdStats.Health -= damageDealt;
        (Instantiate(guts, transform.position, Quaternion.identity) as GameObject).GetComponent<IBleedable>().GenerateGuts(ref birdStats, spearItems.SpearVelocity);
        return damageDealt;
	}
}