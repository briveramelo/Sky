using UnityEngine;
using System.Collections;
using System;

public interface IBasketToBalloon {

	void DetachFromBasket();
	void AttachToBasket(Vector2 newPosition);
	IEnumerator BecomeInvincible();
	int BalloonNumber{get;set;}
}
