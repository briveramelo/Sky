using BRM.Sky.CustomWaveData;
using UnityEngine.Assertions;

namespace BRM.Sky.WaveEditor
{
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
}