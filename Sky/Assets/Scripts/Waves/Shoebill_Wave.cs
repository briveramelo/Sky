using UnityEngine;
using System.Collections;
using GenericFunctions;
public class Shoebill_Wave : Wave {

	//SHOEBILLS
	protected override IEnumerator RunWave(){		
		
		yield return StartCoroutine (Produce1Wait3(SpawnShoebill));

		StartCoroutine(MassProduce(SpawnPelican,2));
		StartCoroutine(MassProduce(SpawnSeagull,3));
		yield return StartCoroutine(MassProduce(SpawnShoebill,5));
		yield return StartCoroutine(WaitFor(allDead,true));

		SpawnAlbatross();
		BirdWaiter WaitFor5Shoes = new BirdWaiter(CounterType.Spawned,false, 5,SpawnAlbatross,BirdType.Shoebill);
		StartCoroutine(WaitFor(WaitFor5Shoes));
		yield return StartCoroutine(MassProduce(SpawnShoebill,7));
		yield return StartCoroutine(WaitFor(allDead,true));

		SpawnDelegate SpawnPigeonAtCenter = ()=>SpawnBirds(BirdType.Pigeon,SpawnPoint(Bool.TossCoin(), medHeight));
		yield return StartCoroutine (MassProduce(SpawnPigeonAtCenter,2));
		SpawnDuckLeader();
		yield return StartCoroutine (MassProduce(SpawnShoebill,5));
		yield return StartCoroutine (WaitFor (allDead, true));

		SpawnBabyCrow();
		yield return StartCoroutine (MassProduce(SpawnShoebill,4));
		yield return StartCoroutine (WaitFor (allDead, true));

		StartCoroutine (MassProduce(SpawnPigeon,4));
		yield return StartCoroutine (MassProduce(SpawnShoebill,10));
		yield return StartCoroutine (WaitFor (allDead, true));

		yield return StartCoroutine (base.RunWave());
	}
}
