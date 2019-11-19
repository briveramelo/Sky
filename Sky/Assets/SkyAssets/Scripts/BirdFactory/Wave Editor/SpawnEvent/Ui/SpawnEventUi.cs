using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BRM.Sky.WaveEditor
{
    public class SpawnEventUi : MonoBehaviour, IUpdateUi, ISelectable
    {
        [SerializeField] private Color _selected, _unselected;
        [SerializeField] private Image _buttonOutline;
        [SerializeField] private Image _iconPreview;
        [SerializeField] private TextMeshProUGUI _spawnTypeText;
        [SerializeField] private TextMeshProUGUI _positionText;
        [SerializeField] private TextMeshProUGUI _timeText;

        [SerializeField] private SpawnTypeSelector _spawnTypeSelector;
        [SerializeField] private SpawnTimeInput _spawnTime;

        public bool IsSelected => _spawnTypeSelector.gameObject.activeInHierarchy;

        private void Start()
        {
            UpdateUi();
        }
        private void Update()
        {
            _positionText.text = _spawnTypeSelector.PositionText;
        }

        public void UpdateUi()
        {
            _iconPreview.sprite = WaveEditorPrefabFactory.Instance.GetSprite(_spawnTypeSelector.SpawnPrefab);
            _spawnTypeText.text = _spawnTypeSelector.PrefabText;
            _positionText.text = _spawnTypeSelector.PositionText;
            _timeText.text = _spawnTime.Text;
        }

        public void Select(bool isSelected)
        {
            _buttonOutline.color = isSelected ? _selected : _unselected;
            _spawnTypeSelector.gameObject.SetActive(isSelected);
            _spawnTypeSelector.Select(isSelected);
        }
    }
}