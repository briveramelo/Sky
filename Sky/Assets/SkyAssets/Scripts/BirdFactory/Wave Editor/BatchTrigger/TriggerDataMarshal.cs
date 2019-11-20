using BRM.Sky.CustomWaveData;
using UnityEngine;

namespace BRM.Sky.WaveEditor
{
    public class TriggerDataMarshal : DataMarshal<BatchTriggerData>
    {
        [SerializeField] private TriggerDropdown _dropdown;
        [SerializeField] private TriggerInputField _inputField;

        public override bool IsDataReady => true;

        public override BatchTriggerData Data
        {
            get => new BatchTriggerData
            {
                TriggerType = _dropdown.TriggerType,
                Amount = _inputField.Amount ?? -1,
            };
            set
            {
                var settings = TriggerSettings.GetSettings(value.TriggerType);
                _inputField.gameObject.SetActive(settings.Display);
                _inputField.ContentType = settings.InputContentType;
                _dropdown.TriggerType = value.TriggerType;
                _inputField.Amount = settings.Display ? value.Amount : (float?) null;
            }
        }
    }
}