using System;
using System.Linq;
using BRM.Sky.CustomWaveData;
using TMPro;
using UnityEngine;

namespace BRM.Sky.WaveEditor
{
    public class TriggerView : MonoBehaviour, IUpdateUi
    {
        [SerializeField] private TextMeshProUGUI _displayText;
        [SerializeField] private TMP_Dropdown _dropdown;
        [SerializeField] private TMP_InputField _inputField;

        private void Start()
        {
            _dropdown.options = Enum.GetValues(typeof(BatchTriggerType)).Cast<BatchTriggerType>()
                .Select(prefabType => new TMP_Dropdown.OptionData(prefabType.ToString())).ToList();
            _dropdown.onValueChanged.AddListener(OnSpawnTypeSelected);
            _inputField.onValueChanged.AddListener(OnAmountChanged);
            
            UpdateDisplayText();
        }

        public string DisplayText => _displayText.text;

        public BatchTriggerType TriggerType
        {
            get => (BatchTriggerType) _dropdown.value;
            set => _dropdown.value = (int) value;
        }
        
        public TMP_InputField.ContentType ContentType
        {
            get => _inputField.contentType;
            set => _inputField.contentType = value;
        }

        public float Amount
        {
            get => float.Parse(_inputField.text);
            set
            {
                var stringFormat = Mathf.Approximately(value, (int) value) ? "0" : "0.00";
                _inputField.text = value.ToString(stringFormat);
            }
        }

        public void UpdateDisplayText()
        {
            if (TriggerSettings.GetSettings(TriggerType).Display)
            {
                _displayText.text = $"{TriggerType}, {_inputField.text}";
            }
            else
            {
                _displayText.text = $"{TriggerType}";
            }
        }
        
        private void OnSpawnTypeSelected(int newValue)
        {
            var triggerType = (BatchTriggerType) newValue;
            var settings = TriggerSettings.GetSettings(triggerType);

            _inputField.gameObject.SetActive(settings.Display);
            _inputField.contentType = settings.InputContentType;
            UpdateDisplayText();
        }

        private void OnAmountChanged(string newAmount)
        {
            UpdateDisplayText();
        }
    }
}