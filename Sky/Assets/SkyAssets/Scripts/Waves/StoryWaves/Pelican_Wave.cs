using System.Collections;

public class Pelican_Wave : Wave {

	protected override IEnumerator GenerateBirds(){
        // 1 Wait 3 PELICAN
        if (ScoreSheet.Reporter.GetCount(CounterType.Alive, false, BirdType.Tentacles) == 0) {
            BirdSpawnDelegates[BirdType.Tentacles]();
        }
		yield return StartCoroutine(Produce1Wait3(BirdSpawnDelegates[BirdType.Pelican]));

		// 4 PIGEONS (Wait 2, + 2)
		// 2 PELICANS
		BirdWaiter[] waitForPigeons = new BirdWaiter[]{
			new BirdWaiter(CounterType.Alive, false, 2,MassProduce(BirdSpawnDelegates[BirdType.Pigeon],2), BirdType.Pigeon),
			new BirdWaiter(CounterType.Spawned, false, 1,BirdSpawnDelegates[BirdType.Pelican], BirdType.Pigeon),
			new BirdWaiter(CounterType.Spawned, false, 3,BirdSpawnDelegates[BirdType.Pelican], BirdType.Pigeon)
		};
		yield return StartCoroutine (MassProduce(BirdSpawnDelegates[BirdType.Pigeon],4));
		yield return StartCoroutine(WaitInParallel(waitForPigeons));
		yield return StartCoroutine(WaitFor(allDeadExceptTentacles,true));

		// 3 DUCKS (Wait 1, +2), 2 PELICANS (Wait 1, +1)
		StartCoroutine (MassProduce(BirdSpawnDelegates[BirdType.Pelican], 2));
		yield return StartCoroutine (ProduceDucks(3));
		BirdWaiter waitForDucks = new BirdWaiter(CounterType.Alive, false, 1,ProduceDucks(2), BirdType.Duck);
		BirdWaiter waitForPelicans = new BirdWaiter(CounterType.Alive, false, 1, BirdSpawnDelegates[BirdType.Pelican], BirdType.Pelican);
		yield return StartCoroutine(WaitInParallel(waitForDucks, waitForPelicans));
		yield return StartCoroutine(WaitFor(allDeadExceptTentacles,true));

		// 3 PELICANS, 3 SEAGULLS
		SpawnDelegate SpawnSeagullandPelican = BirdSpawnDelegates[BirdType.Seagull] + BirdSpawnDelegates[BirdType.Pelican];
		yield return StartCoroutine (MassProduce (SpawnSeagullandPelican,3));
		yield return StartCoroutine(WaitFor(allDeadExceptTentacles,true));

		// >>PIGEONS + DUCKLEADER (Wait -4, +Albatross)
		BirdWaiter waitForKilled = new BirdWaiter(CounterType.Killed, false, 4,()=>SpawnBirds(BirdType.Albatross, SpawnPoint(right,lowHeight)), BirdType.All);
		StartCoroutine(FlyPigeonsAsDuckLeader());
		yield return StartCoroutine(WaitFor(waitForKilled, true));
		yield return StartCoroutine(WaitFor(allDeadExceptTentacles,true));

		// 1 PELICAN, 1 SEAGULL, 2 PIGEONS, 1 DUCK
		StartCoroutine (MassProduce(BirdSpawnDelegates[BirdType.Pigeon],2));
		StartCoroutine (ProduceDucks(1));
		SpawnSeagullandPelican();
		yield return StartCoroutine(WaitFor(allDeadExceptTentacles,true));
	}
}