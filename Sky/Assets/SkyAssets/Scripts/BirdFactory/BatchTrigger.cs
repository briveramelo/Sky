using System;
using System.Collections.Generic;
using BRM.Sky.CustomWaveData;
using UnityEngine;
using UnityEngine.Assertions;

public static class TriggerFactory
{
    private static Dictionary<BatchTriggerType, Func<BatchTriggerData, BatchTrigger>> _factoryInstructions = new Dictionary<BatchTriggerType, Func<BatchTriggerData, BatchTrigger>>
    {
        {BatchTriggerType.AllDead, data => new AllDeadTrigger(data)},
        {BatchTriggerType.Dead, data => new NumDeadTrigger(data)},
        {BatchTriggerType.Spears, data => new SpearsTrigger(data)},
        {BatchTriggerType.Time, data => new TimeTrigger(data)},
    };

    public static BatchTrigger Create(BatchTriggerData data)
    {
        if (_factoryInstructions.TryGetValue(data.TriggerType, out var getTrigger))
        {
            return getTrigger(data);
        }
        
        Debug.LogError("No instructions found for BatchTriggerType{}");
        return null;
    }
}

public abstract class BatchTrigger
{
    protected BatchTrigger(BatchTriggerData data)
    {
        Assert.IsNotNull(data);
        _data = data;
    }

    protected BatchTriggerData _data;
    public abstract bool CanAdvance { get; }
}

public class AllDeadTrigger : BatchTrigger
{
    public AllDeadTrigger(BatchTriggerData data) : base(data)
    {
    }

    public override bool CanAdvance
    {
        get
        {
            var numAlive = ScoreSheet.Reporter.GetCount(BirdCounterType.BirdsAlive, WavePhase.CurrentWave, BirdType.All);
            const int numAliveNeed = 0;
            return numAlive <= numAliveNeed;
        }
    }
}

public class NumDeadTrigger : BatchTrigger
{
    public NumDeadTrigger(BatchTriggerData data) : base(data)
    {
    }

    public override bool CanAdvance
    {
        get
        {
            var killedInBatch = ScoreSheet.Reporter.GetCount(BirdCounterType.BirdsKilled, WavePhase.CurrentBatch, BirdType.All);
            var numDeadNeeded = _data.Amount;
            return killedInBatch >= numDeadNeeded;
        }
    }
}

public class SpearsTrigger : BatchTrigger
{
    public SpearsTrigger(BatchTriggerData data) : base(data)
    {
    }

    public override bool CanAdvance
    {
        get
        {
            var currentThrown = ScoreSheet.Reporter.GetScore(ScoreCounterType.SpearsThrown, WavePhase.CurrentBatch);
            var numSpearsToThrow = _data.Amount;
            return currentThrown >= numSpearsToThrow;
        }
    }
}

public class TimeTrigger : BatchTrigger
{
    private float _startTime;

    public TimeTrigger(BatchTriggerData data) : base(data)
    {
        _startTime = Time.time;
    }

    public override bool CanAdvance
    {
        get
        {
            var currentTimePastStart = Time.time - _startTime;
            var timeToWait = _data.Amount;
            return currentTimePastStart >= timeToWait;
        }
    }
}