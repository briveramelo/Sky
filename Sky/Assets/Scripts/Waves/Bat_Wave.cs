using UnityEngine;
using System.Collections;
using GenericFunctions;

public class Bat_Wave : Wave {

	//BATS
	protected override IEnumerator GenerateBirds(){		

		BirdSpawnDelegates[BirdType.Bat]();
		yield return StartCoroutine(WaitFor(allDead,true));
		yield return StartCoroutine (MassProduce(BirdSpawnDelegates[BirdType.Bat],10));
		yield return StartCoroutine(WaitFor(allDead,true));


		StartCoroutine (MassProduce(BirdSpawnDelegates[BirdType.Shoebill],7));
		yield return StartCoroutine (MassProduce(BirdSpawnDelegates[BirdType.Bat],10));
		yield return StartCoroutine(WaitFor(allDead,true));


		BirdWaiter waitFor5Bats = new BirdWaiter(CounterType.Spawned,false, 5, BirdSpawnDelegates[BirdType.DuckLeader], BirdType.Bat);
		StartCoroutine(WaitFor(waitFor5Bats,false));
		yield return StartCoroutine (MassProduce(BirdSpawnDelegates[BirdType.Bat],10));
		yield return StartCoroutine(WaitFor(allDead,true));


		waitFor5Bats.Perform = MassProduce(BirdSpawnDelegates[BirdType.Pigeon],5);
		BirdWaiter waitFor10DeadBats = new BirdWaiter(CounterType.Killed,false, 10, BirdSpawnDelegates[BirdType.BabyCrow], BirdType.Bat);
		StartCoroutine(WaitFor(waitFor5Bats,false));
		StartCoroutine(WaitFor(waitFor10DeadBats,false));
		yield return StartCoroutine (MassProduce(BirdSpawnDelegates[BirdType.Bat],10));
		yield return StartCoroutine(WaitFor(allDead,true));


		SpawnBirds(BirdType.Albatross,SpawnPoint(Bool.TossCoin(),lowHeight));
		yield return StartCoroutine (MassProduce(BirdSpawnDelegates[BirdType.Bat],5));
		yield return StartCoroutine(WaitFor(allDead,true));

		SpawnBirds(BirdType.DuckLeader,SpawnPoint(right,medHeight));
		SpawnBirds(BirdType.Eagle,Vector2.zero);
		//Many more birds will spawn (triggered from eagle)
		yield return StartCoroutine(WaitFor(allDead,true));

	}
}
