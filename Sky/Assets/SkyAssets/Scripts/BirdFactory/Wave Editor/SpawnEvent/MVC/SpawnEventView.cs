using System;
using System.Linq;
using BRM.Sky.CustomWaveData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BRM.Sky.WaveEditor
{
    public class SpawnEventView : MonoBehaviour, IUpdateUi, ISelectable
    {
        #region Variables
        public event Action<int> OnDropdownSelected;
        
        [SerializeField] private TMP_Dropdown _dropdown;
        [SerializeField] private TMP_InputField _timeInputField;
        [SerializeField] private TextMeshProUGUI _positionDisplay;
        [SerializeField] private TextMeshProUGUI _spawnTypeText;
        [SerializeField] private Image _buttonOutline;
        [SerializeField] private Image _iconPreview;
        [SerializeField] private Color _selected, _unselected;

        private Func<Vector2?> _getInstancePosition;

        public void Initialize(Func<Vector2?> getPrefabInstancePosition)
        {
            _getInstancePosition = getPrefabInstancePosition;
        }

        public float Time
        {
            get => float.Parse(_timeInputField.text);
            set => _timeInputField.text = value.ToString("0.00");
        }

        public bool IsSelected => _dropdown.gameObject.activeInHierarchy;
        
        public SpawnPrefab SpawnPrefab
        {
            get => (SpawnPrefab) _dropdown.value;
            set => _dropdown.value = (int) value;
        }

        public Vector2 NormalizedPosition
        {
            get => _getInstancePosition().GetValueOrDefault();
            set => _positionDisplay.text = value.ToString("0.00");
        }

        private string _prefabText => SpawnPrefab.ToString();
        private string _timeText => string.IsNullOrWhiteSpace(_timeInputField.text) ? "-" : _timeInputField.text;
        private string _positionText => NormalizedPosition.ToString("0.00");
        #endregion
        
        public void UpdateDisplayText()
        {
            _iconPreview.sprite = WaveEditorPrefabFactory.Instance.GetSprite(SpawnPrefab);
            _spawnTypeText.text = _prefabText;
            _positionDisplay.text = _positionText;
            _timeInputField.text = _timeText;
        }

        public void Select(bool isSelected)
        {
            _buttonOutline.color = isSelected ? _selected : _unselected;
            _dropdown.gameObject.SetActive(isSelected);
        }

        private void Awake()
        {
            _dropdown.options = Enum.GetValues(typeof(SpawnPrefab)).Cast<SpawnPrefab>()
                .Select(prefabType => new TMP_Dropdown.OptionData(prefabType.ToString(), WaveEditorPrefabFactory.Instance.GetSprite(prefabType))).ToList();
            _dropdown.onValueChanged.AddListener(OnDropSelected);
        }

        private void Start()
        {
            UpdateDisplayText();
        }

        private void OnDestroy()
        {
            if (Application.isPlaying)
            {
                Destroy(_dropdown.gameObject);
            }
        }

        private void OnDropSelected(int newValue)
        {
            OnDropdownSelected?.Invoke(newValue);
        }
    }
}