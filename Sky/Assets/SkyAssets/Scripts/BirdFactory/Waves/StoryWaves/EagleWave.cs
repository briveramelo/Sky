using UnityEngine;
using System.Collections;

public class EagleWave : Wave
{
    public override IEnumerator GenerateBirds()
    {
        SpawnBirds(BirdType.DuckLeader, SpawnPoint(Right, MedHeight));
        SpawnBirds(BirdType.Eagle, Vector2.zero);
        //Many more birds will spawn (triggered from eagle)
        yield return StartCoroutine(WaitFor(AllDead, true));
    }
}