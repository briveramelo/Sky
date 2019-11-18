using TMPro;
using UnityEngine;

namespace BRM.Sky.WaveEditor
{
    public class TriggerInputField : MonoBehaviour, IDisplayable
    {
        [SerializeField] private TMP_InputField _inputField;

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
    }
}