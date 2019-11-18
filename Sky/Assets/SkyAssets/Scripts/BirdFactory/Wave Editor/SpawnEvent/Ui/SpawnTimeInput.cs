using TMPro;
using UnityEngine;

namespace BRM.Sky.WaveEditor
{
    public class SpawnTimeInput : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _inputField;

        public float Time
        {
            get => float.Parse(_inputField.text);
            set => _inputField.text = value.ToString("0.00");
        }

        public string Text => string.IsNullOrWhiteSpace(_inputField.text) ? "-" : _inputField.text;
    }
}