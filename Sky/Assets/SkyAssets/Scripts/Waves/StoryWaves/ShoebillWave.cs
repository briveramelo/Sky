using System.Collections;
using GenericFunctions;

public class ShoebillWave : Wave {

	//SHOEBILLS
	protected override IEnumerator GenerateBirds(){		
		
		yield return StartCoroutine (Produce1Wait3(BirdSpawnDelegates[BirdType.Shoebill]));

		StartCoroutine(MassProduce(BirdSpawnDelegates[BirdType.Pelican], 2));
		StartCoroutine(MassProduce(BirdSpawnDelegates[BirdType.Seagull], 3));
		yield return StartCoroutine(MassProduce(BirdSpawnDelegates[BirdType.Shoebill],5));
		yield return StartCoroutine(WaitFor(AllDead,true));

        BirdSpawnDelegates[BirdType.Shoebill]();
        BirdWaiter waitFor5Shoes = new BirdWaiter(CounterType.Spawned,false, 5, BirdSpawnDelegates[BirdType.Albatross], BirdType.Shoebill);
		StartCoroutine(WaitFor(waitFor5Shoes));
		yield return StartCoroutine(MassProduce(BirdSpawnDelegates[BirdType.Shoebill],7));
		yield return StartCoroutine(WaitFor(AllDead,true));

		SpawnDelegate pigeonAtCenter = ()=>SpawnBirds(BirdType.Pigeon,SpawnPoint(Bool.TossCoin(), MedHeight));
		yield return StartCoroutine (MassProduce(pigeonAtCenter, 2));
		BirdSpawnDelegates[BirdType.DuckLeader]();
		yield return StartCoroutine (MassProduce(BirdSpawnDelegates[BirdType.Shoebill],5));
		yield return StartCoroutine (WaitFor (AllDead, true));

        BirdSpawnDelegates[BirdType.BabyCrow]();
        yield return StartCoroutine (MassProduce(BirdSpawnDelegates[BirdType.Shoebill],4));
		yield return StartCoroutine (WaitFor (AllDead, true));

		StartCoroutine (MassProduce(BirdSpawnDelegates[BirdType.Pigeon],4));
		yield return StartCoroutine (MassProduce(BirdSpawnDelegates[BirdType.Shoebill],10));
		yield return StartCoroutine (WaitFor (AllDead, true));
	}
}
