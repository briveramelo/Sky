using UnityEngine;
using System.Collections;
using GenericFunctions;

public class BirdOfParadise : LinearBird {

	[SerializeField] private GameObject[] balloons;

	protected override void Awake () {
		moveSpeed = 3f;
		birdStats = new BirdStats(BirdType.BirdOfParadise);
		Destroy (gameObject, 10f);
		base.Awake();
	}

	protected override void PayTheIronPrice(){
		float xSpot = Random.Range(-Constants.worldDimensions.x,Constants.worldDimensions.x) * 0.67f;
		Vector3 spawnSpot = new Vector3 (xSpot,-Constants.worldDimensions.y*1.6f,0f);
		Instantiate( balloons[Random.Range (0,balloons.Length)],spawnSpot,Quaternion.identity);
	}
}
