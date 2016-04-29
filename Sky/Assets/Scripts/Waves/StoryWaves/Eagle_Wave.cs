using UnityEngine;
using System.Collections;

public class Eagle_Wave : Wave {

    protected override IEnumerator GenerateBirds() {
        SpawnBirds(BirdType.DuckLeader,SpawnPoint(right,medHeight));
		SpawnBirds(BirdType.Eagle,Vector2.zero);
		//Many more birds will spawn (triggered from eagle)
		yield return StartCoroutine(WaitFor(allDead,true));
    }
}
