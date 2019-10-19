using System.Collections;

public class PelicanWave : Wave
{
    protected override IEnumerator GenerateBirds()
    {
        // 1 Wait 3 PELICAN
        if (ScoreSheet.Reporter.GetCount(CounterType.Alive, false, BirdType.Tentacles) == 0)
        {
            BirdSpawnDelegates[BirdType.Tentacles]();
        }

        yield return StartCoroutine(Produce1Wait3(BirdSpawnDelegates[BirdType.Pelican]));

        // 4 PIGEONS (Wait 2, + 2)
        // 2 PELICANS
        var waitForPigeons = new []
        {
            new BirdWaiter(CounterType.Alive, false, 2, MassProduce(BirdSpawnDelegates[BirdType.Pigeon], 2), BirdType.Pigeon),
            new BirdWaiter(CounterType.Spawned, false, 1, BirdSpawnDelegates[BirdType.Pelican], BirdType.Pigeon),
            new BirdWaiter(CounterType.Spawned, false, 3, BirdSpawnDelegates[BirdType.Pelican], BirdType.Pigeon)
        };
        yield return StartCoroutine(MassProduce(BirdSpawnDelegates[BirdType.Pigeon], 4));
        yield return StartCoroutine(WaitInParallel(waitForPigeons));
        yield return StartCoroutine(WaitFor(AllDeadExceptTentacles, true));

        // 3 DUCKS (Wait 1, +2), 2 PELICANS (Wait 1, +1)
        StartCoroutine(MassProduce(BirdSpawnDelegates[BirdType.Pelican], 2));
        yield return StartCoroutine(ProduceDucks(3));
        var waitForDucks = new BirdWaiter(CounterType.Alive, false, 1, ProduceDucks(2), BirdType.Duck);
        var waitForPelicans = new BirdWaiter(CounterType.Alive, false, 1, BirdSpawnDelegates[BirdType.Pelican], BirdType.Pelican);
        yield return StartCoroutine(WaitInParallel(waitForDucks, waitForPelicans));
        yield return StartCoroutine(WaitFor(AllDeadExceptTentacles, true));

        // 3 PELICANS, 3 SEAGULLS
        var spawnSeagullandPelican = BirdSpawnDelegates[BirdType.Seagull] + BirdSpawnDelegates[BirdType.Pelican];
        yield return StartCoroutine(MassProduce(spawnSeagullandPelican, 3));
        yield return StartCoroutine(WaitFor(AllDeadExceptTentacles, true));

        // >>PIGEONS + DUCKLEADER (Wait -4, +Albatross)
        var waitForKilled = new BirdWaiter(CounterType.Killed, false, 4, () => SpawnBirds(BirdType.Albatross, SpawnPoint(Right, LowHeight)), BirdType.All);
        StartCoroutine(FlyPigeonsAsDuckLeader());
        yield return StartCoroutine(WaitFor(waitForKilled, true));
        yield return StartCoroutine(WaitFor(AllDeadExceptTentacles, true));

        // 1 PELICAN, 1 SEAGULL, 2 PIGEONS, 1 DUCK
        StartCoroutine(MassProduce(BirdSpawnDelegates[BirdType.Pigeon], 2));
        StartCoroutine(ProduceDucks(1));
        spawnSeagullandPelican();
        yield return StartCoroutine(WaitFor(AllDeadExceptTentacles, true));
    }
}