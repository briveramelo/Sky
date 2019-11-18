using System;
using TMPro;
using UnityEngine;

namespace BRM.Sky.WaveEditor
{
    public class TriggerInputField : MonoBehaviour, IDisplayable
    {
        public event Action<float> OnInputChanged;
        
        [SerializeField] private TMP_InputField _inputField;

        private void Start()
        {
            _inputField.onValueChanged.AddListener(OnValueChanged);
        }

        public TMP_InputField.ContentType ContentType
        {
            get => _inputField.contentType;
            set => _inputField.contentType = value;
        }

        public float? Amount
        {
            get
            {
                if (float.TryParse(_inputField.text, out var amount))
                {
                    return amount;
                }
                return null;
            }
            set => _inputField.text = value == null ? "" : value.Value.ToString("0.00");
        }

        public string Text => _inputField.text;

        public void ToggleDisplay(bool show)
        {
            gameObject.SetActive(show);
        }

        private void OnValueChanged(string newValue)
        {
            OnInputChanged?.Invoke(Amount.Value);
        }
    }
}