using UnityEngine;
using System.Collections;
using GenericFunctions;

public class BirdOfParadise : LinearBird {

	[SerializeField] private GameObject balloon;

	protected override void Awake () {
		moveSpeed = 3f;
		birdStats = new BirdStats(BirdType.BirdOfParadise);
		Destroy (gameObject, 10f);
		base.Awake();
	}

	protected override void DieUniquely(){
		float xSpot = Random.Range(-Constants.WorldDimensions.x,Constants.WorldDimensions.x) * 0.67f;
		Vector3 spawnSpot = new Vector3 (xSpot,-Constants.WorldDimensions.y*1.6f,0f);
		Instantiate( balloon,spawnSpot,Quaternion.identity);
		base.DieUniquely();
	}
}
