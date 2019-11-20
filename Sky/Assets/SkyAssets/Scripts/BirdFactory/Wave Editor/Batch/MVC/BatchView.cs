using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BRM.Sky.WaveEditor
{
    public class BatchView : MonoBehaviour, ISelectable
    {
        [SerializeField] private Color _selected, _unselected;
        [SerializeField] private Image _backgroundImage;

        private TMP_InputField _batchNameInput;
        private bool _isSelected;

        public void Initialize(TMP_InputField batchNameInput)
        {
            _batchNameInput = batchNameInput;
        }

        public void Select(bool isSelected)
        {
            _backgroundImage.color = isSelected ? _selected : _unselected;
            _isSelected = isSelected;
        }

        public bool IsSelected => _isSelected;
        public string Text => _batchNameInput.text;
    }
}