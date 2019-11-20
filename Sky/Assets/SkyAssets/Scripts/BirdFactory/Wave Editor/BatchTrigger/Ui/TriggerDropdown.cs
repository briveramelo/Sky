using System;
using System.Linq;
using BRM.Sky.CustomWaveData;
using TMPro;
using UnityEngine;

namespace BRM.Sky.WaveEditor
{
    public class TriggerDropdown : MonoBehaviour, IDisplayable
    {
        public event Action<BatchTriggerType> OnBatchTriggerSelected;
        
        [SerializeField] private TMP_Dropdown _dropdown;
        [SerializeField] private TMP_InputField _amountInput;

        private void Awake()
        {
            _dropdown.options = Enum.GetValues(typeof(BatchTriggerType)).Cast<BatchTriggerType>()
                .Select(prefabType => new TMP_Dropdown.OptionData(prefabType.ToString())).ToList();
            _dropdown.onValueChanged.AddListener(OnValueChanged);
            OnValueChanged(_dropdown.value);
        }

        public BatchTriggerType TriggerType
        {
            get => (BatchTriggerType) _dropdown.value;
            set => _dropdown.value = (int) value;
        }

        public void ToggleDisplay(bool show)
        {
            gameObject.SetActive(show);
        }

        private void OnValueChanged(int newValue)
        {
            var triggerType = (BatchTriggerType) newValue;
            var settings = TriggerSettings.GetSettings(triggerType);

            _amountInput.gameObject.SetActive(settings.Display);
            _amountInput.contentType = settings.InputContentType;
            
            OnBatchTriggerSelected?.Invoke(triggerType);
        }
    }
}