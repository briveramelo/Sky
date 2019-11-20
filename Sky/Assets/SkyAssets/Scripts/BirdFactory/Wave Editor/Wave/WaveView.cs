using TMPro;
using UnityEngine;

namespace BRM.Sky.WaveEditor
{
    public class WaveView : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _waveNameText;
        [SerializeField] private TMP_InputField _subtitleText;

        public string WaveNameText
        {
            get => _waveNameText.text;
            set => _waveNameText.text = value;
        }
        public string SubtitleText
        {
            get => _subtitleText.text;
            set => _subtitleText.text = value;
        }
    }
}