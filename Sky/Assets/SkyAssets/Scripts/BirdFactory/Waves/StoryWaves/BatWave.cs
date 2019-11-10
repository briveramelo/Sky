using System.Collections;
using GenericFunctions;

public class BatWave : Wave
{
    //BATS
    protected override IEnumerator GenerateBirds()
    {
        BirdSpawnDelegates[BirdType.Bat]();
        yield return StartCoroutine(WaitFor(AllDead, true));
        yield return StartCoroutine(MassProduce(BirdSpawnDelegates[BirdType.Bat], 10));
        yield return StartCoroutine(WaitFor(AllDead, true));


        StartCoroutine(MassProduce(BirdSpawnDelegates[BirdType.Shoebill], 7));
        yield return StartCoroutine(MassProduce(BirdSpawnDelegates[BirdType.Bat], 10));
        yield return StartCoroutine(WaitFor(AllDead, true));


        var waitFor5Bats = new BirdWaiter(CounterType.Spawned, false, 5, BirdSpawnDelegates[BirdType.DuckLeader], BirdType.Bat);
        StartCoroutine(WaitFor(waitFor5Bats));
        yield return StartCoroutine(MassProduce(BirdSpawnDelegates[BirdType.Bat], 10));
        yield return StartCoroutine(WaitFor(AllDead, true));


        waitFor5Bats.Perform = MassProduce(BirdSpawnDelegates[BirdType.Pigeon], 5);
        var waitFor10DeadBats = new BirdWaiter(CounterType.Killed, false, 10, BirdSpawnDelegates[BirdType.BabyCrow], BirdType.Bat);
        StartCoroutine(WaitFor(waitFor5Bats));
        StartCoroutine(WaitFor(waitFor10DeadBats));
        yield return StartCoroutine(MassProduce(BirdSpawnDelegates[BirdType.Bat], 10));
        yield return StartCoroutine(WaitFor(AllDead, true));


        SpawnBirds(BirdType.Albatross, SpawnPoint(Bool.TossCoin(), LowHeight));
        yield return StartCoroutine(MassProduce(BirdSpawnDelegates[BirdType.Bat], 5));
        yield return StartCoroutine(WaitFor(AllDead, true));
    }
}