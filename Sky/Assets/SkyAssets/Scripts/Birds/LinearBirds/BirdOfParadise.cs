using UnityEngine;
using GenericFunctions;

public class BirdOfParadise : LinearBird {

	[SerializeField] private GameObject _balloon;

	protected override void Awake () {
		MoveSpeed = 3f;
		base.Awake();
		Destroy (gameObject, 10f);
	}

	protected override void DieUniquely(){
		float xSpot = Random.Range(-Constants.WorldDimensions.x,Constants.WorldDimensions.x) * 0.67f;
		Vector3 spawnSpot = new Vector3 (xSpot,-Constants.WorldDimensions.y*1.6f,0f);
		Instantiate( _balloon,spawnSpot,Quaternion.identity);
		base.DieUniquely();
	}
}
