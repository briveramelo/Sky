using System;
using BRM.Sky.CustomWaveData;
using UnityEngine;
using UnityEngine.Assertions;

public static class TriggerFactory
{
    public static BatchTrigger Create(BatchTriggerData data)
    {
        Func<float, bool> isWaiting = null;
        switch (data.TriggerType)
        {
            case BatchTriggerType.AllDead: isWaiting = Triggers.IsWaitingForAllDead; break;
            case BatchTriggerType.Dead: isWaiting = Triggers.IsWaitingForNumDead; break;
            case BatchTriggerType.Spears: isWaiting = Triggers.IsWaitingForNumSpears; break;
            case BatchTriggerType.Time:
                float currentTime = Time.time;
                isWaiting = timeToWait => Time.time - currentTime > timeToWait; 
                break;
        }

        return new BatchTrigger(data, isWaiting);
    }
}

public class BatchTrigger
{
    public BatchTrigger(BatchTriggerData data, Func<float, bool> isWaiting)
    {
        Assert.IsNotNull(isWaiting);
        Assert.IsNotNull(data);
        
        _data = data;
        _isWaiting = isWaiting;
    }

    public bool IsWaiting => _isWaiting(_data.Amount);
    private Func<float, bool> _isWaiting;
    private BatchTriggerData _data;
}

public static class Triggers
{
    public static bool IsWaitingForAllDead(float dummy = 0)
    {
        var currentAlive = ScoreSheet.Reporter.GetCount(BirdCounterType.BirdsAlive, WavePhase.CurrentWave, BirdType.All);
        return currentAlive == 0;
    }
    
    public static bool IsWaitingForNumDead(float numDeadToWaitFor)
    {
        var currentKilled = ScoreSheet.Reporter.GetCount(BirdCounterType.BirdsKilled, WavePhase.CurrentBatch, BirdType.All);
        return currentKilled < numDeadToWaitFor;
    }
    
    public static bool IsWaitingForNumSpears(float numSpearsToWaitFor)
    {
        var currentThrown = ScoreSheet.Reporter.GetScore(ScoreCounterType.SpearsThrown, WavePhase.CurrentBatch);
        return currentThrown < numSpearsToWaitFor;
    }
}