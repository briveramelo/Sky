using System.Collections;
using GenericFunctions;

public class Shoebill_Wave : Wave {

	//SHOEBILLS
	protected override IEnumerator GenerateBirds(){		
		
		yield return StartCoroutine (Produce1Wait3(BirdSpawnDelegates[BirdType.Shoebill]));

		StartCoroutine(MassProduce(BirdSpawnDelegates[BirdType.Pelican], 2));
		StartCoroutine(MassProduce(BirdSpawnDelegates[BirdType.Seagull], 3));
		yield return StartCoroutine(MassProduce(BirdSpawnDelegates[BirdType.Shoebill],5));
		yield return StartCoroutine(WaitFor(allDead,true));

        BirdSpawnDelegates[BirdType.Shoebill]();
        BirdWaiter WaitFor5Shoes = new BirdWaiter(CounterType.Spawned,false, 5, BirdSpawnDelegates[BirdType.Albatross], BirdType.Shoebill);
		StartCoroutine(WaitFor(WaitFor5Shoes));
		yield return StartCoroutine(MassProduce(BirdSpawnDelegates[BirdType.Shoebill],7));
		yield return StartCoroutine(WaitFor(allDead,true));

		SpawnDelegate PigeonAtCenter = ()=>SpawnBirds(BirdType.Pigeon,SpawnPoint(Bool.TossCoin(), medHeight));
		yield return StartCoroutine (MassProduce(PigeonAtCenter, 2));
		BirdSpawnDelegates[BirdType.DuckLeader]();
		yield return StartCoroutine (MassProduce(BirdSpawnDelegates[BirdType.Shoebill],5));
		yield return StartCoroutine (WaitFor (allDead, true));

        BirdSpawnDelegates[BirdType.BabyCrow]();
        yield return StartCoroutine (MassProduce(BirdSpawnDelegates[BirdType.Shoebill],4));
		yield return StartCoroutine (WaitFor (allDead, true));

		StartCoroutine (MassProduce(BirdSpawnDelegates[BirdType.Pigeon],4));
		yield return StartCoroutine (MassProduce(BirdSpawnDelegates[BirdType.Shoebill],10));
		yield return StartCoroutine (WaitFor (allDead, true));
	}
}
