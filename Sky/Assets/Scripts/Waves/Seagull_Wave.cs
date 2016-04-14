using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GenericFunctions;
public class Seagull_Wave : Wave {

	protected override IEnumerator RunWave(){

        // 1 WAIT 3 SEAGULL
        if (ScoreSheet.Reporter.GetCount(CounterType.Alive, false, BirdType.Tentacles) == 0) {
            BirdSpawnDelegates[BirdType.Tentacles]();
        }
		yield return StartCoroutine(Produce1Wait3(BirdSpawnDelegates[BirdType.Seagull]));

		// 5 PIGEONS
		// 2 SEAGULLS
		BirdWaiter WaitFor1Pigeons = new BirdWaiter(CounterType.Spawned, false,1, BirdSpawnDelegates[BirdType.Seagull], BirdType.Pigeon);
		BirdWaiter WaitFor4Pigeons = new BirdWaiter(CounterType.Spawned, false,4, BirdSpawnDelegates[BirdType.Seagull], BirdType.Pigeon);
		StartCoroutine(WaitInParallel(WaitFor1Pigeons,WaitFor4Pigeons));
		yield return StartCoroutine (MassProduce(BirdSpawnDelegates[BirdType.Pigeon],5));
		yield return StartCoroutine(WaitFor(allDeadExceptTentacles,true));

		// 3 DUCKS
		// 2 SEAGULLS
		BirdWaiter WaitFor1Duck = new BirdWaiter(CounterType.Spawned, false,1, BirdSpawnDelegates[BirdType.Seagull], BirdType.Duck);
		BirdWaiter WaitFor3Ducks = new BirdWaiter(CounterType.Spawned, false,3, BirdSpawnDelegates[BirdType.Seagull], BirdType.Duck);
		StartCoroutine(WaitInParallel(WaitFor1Duck,WaitFor3Ducks));
		yield return StartCoroutine (ProduceDucks(3));
		yield return StartCoroutine(WaitFor(allDeadExceptTentacles,true));

		// 1 DUCK LEADER
		// 1 SEAGULL
		bool leaderSide = Bool.TossCoin();
		SpawnBirds(BirdType.DuckLeader,SpawnPoint(leaderSide, 0));
		SpawnBirds(BirdType.Seagull,SpawnPoint(!leaderSide, 0.25f,.75f));
		yield return StartCoroutine(WaitFor(allDeadExceptTentacles,true));

		yield return StartCoroutine (base.RunWave());
	}
}