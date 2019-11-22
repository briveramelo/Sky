using System.Collections;
using System.Linq;
using System;
using System.Collections.Generic;
using BRM.DebugAdapter;
using BRM.DebugAdapter.Interfaces;
using UnityEngine;
using Random = System.Random;

public class BirdWaiter
{
    public bool Wait => _wait(_movingNumber);
    public IEnumerator Perform;
    public SpawnDelegate Spawn;

    private BirdCounterType _birdCounterType;
    private BirdType[] _birdTypes;
    private int _numberToWaitFor;
    private int _movingNumber => ScoreSheet.Reporter.GetCounts(_birdCounterType, WavePhase.CurrentWave, _birdTypes);

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
        if (invertBirdTypes)
        {
            var inverted = birdTypes.Invert();
            inverted.Remove(BirdType.All);
            _birdTypes = inverted.ToArray();
        }
        else
        {
            _birdTypes = birdTypes;
        }

        _numberToWaitFor = numberToWaitFor;
        _wait = mover => mover > _numberToWaitFor;
        if (birdCounterType == BirdCounterType.BirdsSpawned || birdCounterType == BirdCounterType.BirdsKilled)
        {
            _numberToWaitFor += ScoreSheet.Reporter.GetCounts(birdCounterType, WavePhase.CurrentWave, birdTypes);
            _wait = mover => mover < _numberToWaitFor;
        }
    }
}

public static class EnumHelpers
{
    private static IDebug _debugger = new UnityDebugger();
    public static List<TEnum> Invert<TEnum>(this TEnum[] birdTypes) where TEnum : Enum
    {
        var birdsToWaitFor = Enum.GetValues(typeof(TEnum)).Cast<TEnum>().ToList();
        foreach (var birdType in birdTypes)
        {
            birdsToWaitFor.Remove(birdType);
        }

        return birdsToWaitFor;
    }

    public static List<TEnum> GetAll<TEnum>() where TEnum : Enum
    {
        return Enum.GetValues(typeof(TEnum)).Cast<TEnum>().ToList();
    }

    public static TVal SafeGet<TKey, TVal>(this Dictionary<TKey, TVal> dic, TKey key) where TKey : Enum
    {
        if(dic.TryGetValue(key, out var val))
        {
            return val;
        }

        _debugger.LogErrorFormat("No value found in dictionary for keyed enum {0}", key);
        return default;
    }

    public static TEnum GetRandom<TEnum>() where TEnum : Enum
    {
        var all = GetAll<TEnum>();
        var index = UnityEngine.Random.Range(0, all.Count);
        return all[index];
    }
}