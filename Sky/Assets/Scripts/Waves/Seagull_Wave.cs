using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GenericFunctions;
public class Seagull_Wave : Wave {

	protected override IEnumerator RunWave(){

		// 1 WAIT 3 SEAGULL
		if (ScoreSheet.Reporter.GetCount(CounterType.Alive,BirdType.Tentacles)==0) SpawnTentacles();
		yield return StartCoroutine(Produce1Wait3(SpawnSeagull));

		// 5 PIGEONS
		// 2 SEAGULLS
		BirdWaiter WaitFor1Pigeons = new BirdWaiter(CounterType.Spawned, false,1,SpawnSeagull,BirdType.Pigeon);
		BirdWaiter WaitFor4Pigeons = new BirdWaiter(CounterType.Spawned, false,4,SpawnSeagull,BirdType.Pigeon);
		StartCoroutine(WaitInParallel(WaitFor1Pigeons,WaitFor4Pigeons));
		yield return StartCoroutine (MassProduce(SpawnPigeon,5));
		yield return StartCoroutine(WaitFor(allDeadExceptTentacles,true));

		// 3 DUCKS
		// 2 SEAGULLS
		BirdWaiter WaitFor1Duck = new BirdWaiter(CounterType.Spawned, false,1,SpawnSeagull,BirdType.Duck);
		BirdWaiter WaitFor3Ducks = new BirdWaiter(CounterType.Spawned, false,3,SpawnSeagull,BirdType.Duck);
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