using System;
using BRM.Sky.CustomWaveData;
using UnityEngine;
using UnityEngine.Assertions;

public static class TriggerFactory
{
    public static BatchTrigger Create(BatchTriggerData data)
    {
        Func<float, bool> canAdvance = null;
        switch (data.TriggerType)
        {
            case BatchTriggerType.AllDead: canAdvance = Triggers.CanAdvancePastAllDead; break;
            case BatchTriggerType.Dead: canAdvance = Triggers.CanAdvancePastNumDead; break;
            case BatchTriggerType.Spears: canAdvance = Triggers.CanAdvancePastNumSpears; break;
            case BatchTriggerType.Time:
                var creationTime = Time.time;
                canAdvance = timeToWait => Time.time - creationTime > timeToWait; 
                break;
        }

        return new BatchTrigger(data, canAdvance);
    }
}

public class BatchTrigger
{
    public BatchTrigger(BatchTriggerData data, Func<float, bool> canAdvance)
    {
        Assert.IsNotNull(canAdvance);
        Assert.IsNotNull(data);
        
        _data = data;
        _canAdvance = canAdvance;
    }

    public bool CanAdvance => _canAdvance(_data.Amount);
    private Func<float, bool> _canAdvance;
    private BatchTriggerData _data;
}

public static class Triggers
{
    public static bool CanAdvancePastAllDead(float dummy = 0)
    {
        var currentAlive = ScoreSheet.Reporter.GetCount(BirdCounterType.BirdsAlive, WavePhase.CurrentWave, BirdType.All);
        return currentAlive <= 0;
    }
    
    public static bool CanAdvancePastNumDead(float numDeadToWaitFor)
    {
        var currentKilled = ScoreSheet.Reporter.GetCount(BirdCounterType.BirdsKilled, WavePhase.CurrentBatch, BirdType.All);
        return currentKilled >= numDeadToWaitFor;
    }
    
    public static bool CanAdvancePastNumSpears(float numSpearsToWaitFor)
    {
        var currentThrown = ScoreSheet.Reporter.GetScore(ScoreCounterType.SpearsThrown, WavePhase.CurrentBatch);
        return currentThrown >= numSpearsToWaitFor;
    }
}