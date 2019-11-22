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

        public void Initialize()
        {
            _dropdown.options = EnumHelpers.GetAll<BatchTriggerType>().Select(prefabType => new TMP_Dropdown.OptionData(prefabType.ToString())).ToList();
            _dropdown.onValueChanged.AddListener(OnSpawnTypeSelected);
            _inputField.onValueChanged.AddListener(OnAmountChanged);
        }

        public BatchTriggerType TriggerType
        {
            get => (BatchTriggerType) _dropdown.value;
            set
            {
                var settings = TriggerSettings.GetSettings(value);
                _inputField.contentType = settings.InputContentType;
                _inputField.gameObject.SetActive(settings.Display);
                _dropdown.value = (int) value;
                UpdateDisplayText();
            }
        }

        public float Amount
        {
            get => float.Parse(_inputField.text);
            set
            {
                var stringFormat = Mathf.Approximately(value, (int) value) ? "0" : "0.00";
                _inputField.text = value.ToString(stringFormat);
                UpdateDisplayText();
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
            TriggerType = triggerType;
        }

        private void OnAmountChanged(string newAmount)
        {
            UpdateDisplayText();
        }
    }
}