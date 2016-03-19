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
		
	protected override void TakeDamage (SpearItems spearItems){
		base.TakeDamage(spearItems);
		float hitHeight = birdCollider.bounds.ClosestPoint(spearItems.SpearCollider.transform.position).y;
		if (spearItems.SpearVelocity.y>0 && hitHeight<transform.position.y){ //kill albatross with a tactical shot to the underbelly
			birdStats.Health = 0;
			//super kill!
			//take a bite out of that soft, vulnerable tummy
		}
	}
}