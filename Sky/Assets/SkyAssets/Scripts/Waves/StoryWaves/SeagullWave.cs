using System.Collections;
using GenericFunctions;

public class SeagullWave : Wave
{
    protected override IEnumerator GenerateBirds()
    {
        // 1 WAIT 3 SEAGULL
        if (ScoreSheet.Reporter.GetCount(CounterType.Alive, false, BirdType.Tentacles) == 0)
        {
            BirdSpawnDelegates[BirdType.Tentacles]();
        }

        yield return StartCoroutine(Produce1Wait3(BirdSpawnDelegates[BirdType.Seagull]));

        // 5 PIGEONS
        // 2 SEAGULLS
        var waitFor1Pigeons = new BirdWaiter(CounterType.Spawned, false, 1, BirdSpawnDelegates[BirdType.Seagull], BirdType.Pigeon);
        var waitFor4Pigeons = new BirdWaiter(CounterType.Spawned, false, 4, BirdSpawnDelegates[BirdType.Seagull], BirdType.Pigeon);
        StartCoroutine(WaitInParallel(waitFor1Pigeons, waitFor4Pigeons));
        yield return StartCoroutine(MassProduce(BirdSpawnDelegates[BirdType.Pigeon], 5));
        yield return StartCoroutine(WaitFor(AllDeadExceptTentacles, true));

        // 3 DUCKS
        // 2 SEAGULLS
        var waitFor1Duck = new BirdWaiter(CounterType.Spawned, false, 1, BirdSpawnDelegates[BirdType.Seagull], BirdType.Duck);
        var waitFor3Ducks = new BirdWaiter(CounterType.Spawned, false, 3, BirdSpawnDelegates[BirdType.Seagull], BirdType.Duck);
        StartCoroutine(WaitInParallel(waitFor1Duck, waitFor3Ducks));
        yield return StartCoroutine(ProduceDucks(3));
        yield return StartCoroutine(WaitFor(AllDeadExceptTentacles, true));

        // 1 DUCK LEADER
        // 1 SEAGULL
        var leaderSide = Bool.TossCoin();
        SpawnBirds(BirdType.DuckLeader, SpawnPoint(leaderSide, 0));
        SpawnBirds(BirdType.Seagull, SpawnPoint(!leaderSide, 0.25f, .75f));
        yield return StartCoroutine(WaitFor(AllDeadExceptTentacles, true));
    }
}