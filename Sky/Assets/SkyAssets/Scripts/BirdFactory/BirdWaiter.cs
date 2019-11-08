using System.Collections;
using System.Linq;
using System;

public class BirdWaiter
{
    protected CounterType CounterType;
    protected BirdType[] BirdTypes;
    public int NumberToWaitFor;
    protected int MovingNumber => ScoreSheet.Reporter.GetCounts(CounterType, true, BirdTypes);
    public bool Wait => _wait(MovingNumber);

    private delegate bool BoolDelegate(int mover);

    private BoolDelegate _wait;
    public IEnumerator Perform;
    public SpawnDelegate Spawn;

    public BirdWaiter(CounterType counterType, bool invertBirdTypes, int numberToWaitFor, IEnumerator perform, params BirdType[] birdTypes)
    {
        Initialize(counterType, invertBirdTypes, numberToWaitFor, birdTypes);
        Perform = perform;
    }

    public BirdWaiter(CounterType counterType, bool invertBirdTypes, int numberToWaitFor, SpawnDelegate spawn, params BirdType[] birdTypes)
    {
        Initialize(counterType, invertBirdTypes, numberToWaitFor, birdTypes);
        Spawn = spawn;
    }

    public BirdWaiter(CounterType counterType, bool invertBirdTypes, int numberToWaitFor, params BirdType[] birdTypes)
    {
        Initialize(counterType, invertBirdTypes, numberToWaitFor, birdTypes);
    }

    private void Initialize(CounterType counterType, bool invertBirdTypes, int numberToWaitFor, params BirdType[] birdTypes)
    {
        CounterType = counterType;
        BirdTypes = invertBirdTypes ? InvertBirdTypes(birdTypes) : birdTypes;
        NumberToWaitFor = numberToWaitFor;
        _wait = mover => mover > NumberToWaitFor;
        if (counterType == CounterType.Spawned || counterType == CounterType.Killed)
        {
            NumberToWaitFor += ScoreSheet.Reporter.GetCounts(counterType, true, birdTypes);
            _wait = mover => mover < NumberToWaitFor;
        }
    }

    private static BirdType[] InvertBirdTypes(params BirdType[] birdTypes)
    {
        var birdsToWaitFor = Enum.GetValues(typeof(BirdType)).Cast<BirdType>().ToList();
        birdsToWaitFor.Remove(BirdType.All);
        foreach (var birdType in birdTypes)
        {
            birdsToWaitFor.Remove(birdType);
        }

        return birdsToWaitFor.ToArray();
    }
}