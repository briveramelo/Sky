using BRM.Sky.CustomWaveData;
using UnityEngine;

namespace BRM.Sky.WaveEditor
{
    //data interface to the view
    public class TriggerDataMarshal : DataMarshal<BatchTriggerData, TriggerView>
    {
        [SerializeField] private TriggerView _view;

        public override bool IsDataReady => true;
        protected override TriggerView View => _view;

        public override BatchTriggerData Data
        {
            get => new BatchTriggerData
            {
                TriggerType = View.TriggerType,
                Amount = View.Amount,
            };
            set
            {
                View.TriggerType = value.TriggerType;
                View.Amount = value.Amount;
            }
        }
    }
}