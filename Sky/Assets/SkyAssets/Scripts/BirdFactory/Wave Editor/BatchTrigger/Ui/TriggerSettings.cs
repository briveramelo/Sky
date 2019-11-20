using System.Collections.Generic;
using BRM.Sky.CustomWaveData;
using TMPro;

namespace BRM.Sky.WaveEditor
{
    public class BatchTriggerSettings
    {
        public bool Display;
        public TMP_InputField.ContentType InputContentType;
    }

    public static class TriggerSettings
    {
        public static BatchTriggerSettings GetSettings(BatchTriggerType type)
        {
            return _triggerTypeSettings[type];
        }

        private static Dictionary<BatchTriggerType, BatchTriggerSettings> _triggerTypeSettings = new Dictionary<BatchTriggerType, BatchTriggerSettings>
        {
            {BatchTriggerType.AllDead, new BatchTriggerSettings {Display = false}},
            {BatchTriggerType.Dead, new BatchTriggerSettings {Display = true, InputContentType = TMP_InputField.ContentType.IntegerNumber}},
            {BatchTriggerType.Spears, new BatchTriggerSettings {Display = true, InputContentType = TMP_InputField.ContentType.IntegerNumber}},
            {BatchTriggerType.Time, new BatchTriggerSettings {Display = true, InputContentType = TMP_InputField.ContentType.DecimalNumber}},
        };
    }
}