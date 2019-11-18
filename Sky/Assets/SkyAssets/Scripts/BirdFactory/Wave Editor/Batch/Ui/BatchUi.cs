using UnityEngine;
using UnityEngine.UI;

namespace BRM.Sky.WaveEditor
{
    public class BatchUi : MonoBehaviour, ISelectable
    {
        [SerializeField] private Color _selected, _unselected;
        [SerializeField] private Image _backgroundImage;

        private bool _isSelected;
        public void Select(bool isSelected)
        {
            _backgroundImage.color = isSelected ? _selected : _unselected;
            _isSelected = isSelected;
        }

        public bool IsSelected => _isSelected;
    }
}