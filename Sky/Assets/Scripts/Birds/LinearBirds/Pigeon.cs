﻿using UnityEngine;
using System.Collections;
using GenericFunctions;

public class Pigeon : LinearBird {

	protected override void Awake () {
		moveSpeed =2f;
		birdStats = new BirdStats(BirdType.Pigeon);
		base.Awake();
	}
}