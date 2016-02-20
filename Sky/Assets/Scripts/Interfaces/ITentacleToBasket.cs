using UnityEngine;

public interface ITentacleToBasket {
	void KnockDown(float downForce);
	void LoseAllBalloons();
	void AttachToTentacles(Transform tentaclesTransform);
	void DetachFromTentacles();
	Collider2D[] BasketColliders{get;}
}
