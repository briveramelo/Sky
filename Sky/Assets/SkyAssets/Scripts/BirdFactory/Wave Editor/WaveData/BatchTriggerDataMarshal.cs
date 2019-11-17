using UnityEngine;

namespace BRM.Sky.WaveEditor
{
    public class BatchTriggerDataMarshal : DataMarshal<BatchTriggerData>
    {
        [SerializeField] private BatchTriggerDropdown _dropdown;
        [SerializeField] private BatchTriggerInputField _inputField;

        public override bool IsDataReady => _inputField.Amount.HasValue;

        public override BatchTriggerData Data => new BatchTriggerData
        {
            TriggerType = _dropdown.TriggerType,
            Amount = _inputField.Amount ?? -1,
        };
    }
}