using System;
using System.Collections.Generic;
using BRM.Sky.CustomWaveData;
using UnityEngine;

namespace BRM.Sky.WaveEditor
{
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

            Debug.LogError($"No instructions found for BatchTriggerType:{data.TriggerType}");
            return null;
        }
    }
}