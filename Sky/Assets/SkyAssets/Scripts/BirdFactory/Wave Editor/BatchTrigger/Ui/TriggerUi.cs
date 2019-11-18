using TMPro;
using UnityEngine;

namespace BRM.Sky.WaveEditor
{
    public class TriggerUi : MonoBehaviour, IUpdateUi
    {
        [SerializeField] private TextMeshProUGUI _displayText;
        [SerializeField] private TriggerDropdown _dropdown;
        [SerializeField] private TriggerInputField _inputField;

        private void Awake()
        {
            UpdateUi();
        }

        private void Start()
        {
            _dropdown.OnBatchTriggerSelected += OnSpawnTypeSelected;
            _inputField.OnInputChanged += OnAmountChanged;
        }

        public void UpdateUi()
        {
            if (string.IsNullOrWhiteSpace(_inputField.Text))
            {
                _displayText.text = $"{_dropdown.TriggerType}";
            }
            else
            {
                _displayText.text = $"{_dropdown.TriggerType}, {_inputField.Text}";
            }
        }
        
        private void OnSpawnTypeSelected(BatchTriggerType triggerType)
        {
            UpdateUi();
        }

        private void OnAmountChanged(float amount)
        {
            UpdateUi();
        }
    }
}