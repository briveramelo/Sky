using UnityEngine;
using System.Collections;

public interface IHurtable {
	void TakeDamage(Vector2 gutDirection, Collider2D spearCollider);
	BirdStats MyBirdStats{get;}
}
