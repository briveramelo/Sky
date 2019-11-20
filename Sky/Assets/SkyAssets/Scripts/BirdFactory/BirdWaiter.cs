using System.Collections;
using System.Linq;
using System;

public class BirdWaiter
{
    public bool Wait => _wait(_movingNumber);
    public IEnumerator Perform;
    public SpawnDelegate Spawn;
    
    private BirdCounterType _birdCounterType;
    private BirdType[] _birdTypes;
    private int _numberToWaitFor;
    private int _movingNumber => ScoreSheet.Reporter.GetCounts(_birdCounterType, true, _birdTypes);

    private delegate bool BoolDelegate(int mover);
    private BoolDelegate _wait;

    public BirdWaiter(BirdCounterType birdCounterType, bool invertBirdTypes, int numberToWaitFor, IEnumerator perform, params BirdType[] birdTypes)
    {
        Initialize(birdCounterType, invertBirdTypes, numberToWaitFor, birdTypes);
        Perform = perform;
    }

    public BirdWaiter(BirdCounterType birdCounterType, bool invertBirdTypes, int numberToWaitFor, SpawnDelegate spawn, params BirdType[] birdTypes)
    {
        Initialize(birdCounterType, invertBirdTypes, numberToWaitFor, birdTypes);
        Spawn = spawn;
    }

    public BirdWaiter(BirdCounterType birdCounterType, bool invertBirdTypes, int numberToWaitFor, params BirdType[] birdTypes)
    {
        Initialize(birdCounterType, invertBirdTypes, numberToWaitFor, birdTypes);
    }

    private void Initialize(BirdCounterType birdCounterType, bool invertBirdTypes, int numberToWaitFor, params BirdType[] birdTypes)
    {
        _birdCounterType = birdCounterType;
        _birdTypes = invertBirdTypes ? InvertBirdTypes(birdTypes) : birdTypes;
        _numberToWaitFor = numberToWaitFor;
        _wait = mover => mover > _numberToWaitFor;
        if (birdCounterType == BirdCounterType.BirdsSpawned || birdCounterType == BirdCounterType.BirdsKilled)
        {
            _numberToWaitFor += ScoreSheet.Reporter.GetCounts(birdCounterType, true, birdTypes);
            _wait = mover => mover < _numberToWaitFor;
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