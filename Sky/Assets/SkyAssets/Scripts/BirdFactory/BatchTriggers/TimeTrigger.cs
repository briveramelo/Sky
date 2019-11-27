using BRM.Sky.CustomWaveData;
using UnityEngine;

namespace BRM.Sky.WaveEditor
{
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
}