using UnityEngine;
using System.Collections;
using GenericFunctions;
using System;

public interface ITriggerSpawnable{
	void TriggerSpawnEvent();
}

public class Bat_Wave : Wave, ITriggerSpawnable {

	//BATS
	protected override IEnumerator RunWave(){		

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

		yield return StartCoroutine (base.RunWave());
	}
		
	private enum SpawnEvent{
		Bats5 = 0,
		Pigeons5 =1,
		Shoebill6=2,
		Ducks3 =3,
		Seagull3=4,
		Albatross2=5
	}

	void ITriggerSpawnable.TriggerSpawnEvent(){
		SpawnEvent dice = (SpawnEvent)UnityEngine.Random.Range(0,Enum.GetNames(typeof(SpawnEvent)).Length);
		switch (dice){
		case SpawnEvent.Bats5:
			StartCoroutine(MassProduce(BirdSpawnDelegates[BirdType.Bat],5));
			break;
		case SpawnEvent.Pigeons5:
			StartCoroutine(MassProduce(BirdSpawnDelegates[BirdType.Pigeon], 3));
			break;
		case SpawnEvent.Shoebill6:
			StartCoroutine(MassProduce(BirdSpawnDelegates[BirdType.Shoebill], 6));
			break;
		case SpawnEvent.Ducks3:
			StartCoroutine(ProduceDucks(3));
			break;
		case SpawnEvent.Seagull3:
			StartCoroutine(MassProduce(BirdSpawnDelegates[BirdType.Seagull], 3));
			break;
		case SpawnEvent.Albatross2:
			SpawnBirds(BirdType.Albatross,SpawnPoint(right,lowHeight));
			SpawnBirds(BirdType.Albatross,SpawnPoint(left,lowHeight));
			break;
		}
	}
}
