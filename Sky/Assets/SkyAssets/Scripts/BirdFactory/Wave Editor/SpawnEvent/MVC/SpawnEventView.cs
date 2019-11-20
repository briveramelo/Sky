using System;
using System.Linq;
using BRM.Sky.CustomWaveData;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace BRM.Sky.WaveEditor
{
    public class SpawnEventView : MonoBehaviour, IUpdateUi, ISelectable
    {
        #region Variables
        public event Action<int> OnDropdownSelected;
        
        [SerializeField] private TMP_Dropdown _spawnTypeDropdown;
        [SerializeField] private TMP_InputField _timeInput;
        
        [SerializeField] private Image _iconPreview;
        [SerializeField] private TextMeshProUGUI _spawnTypeDisplay;
        [SerializeField] private TextMeshProUGUI _positionDisplay;
        [SerializeField] private TextMeshProUGUI _timeDisplay;
        
        [SerializeField] private Image _buttonOutline;
        [SerializeField] private Color _selected, _unselected;

        private GameObject _prefabInstance;

        public void SetPrefabInstance(GameObject prefabInstance)
        {
            _prefabInstance = prefabInstance;
        }

        public float Time
        {
            get => float.Parse(_timeInput.text);
            set
            {
                var stringFormat = Mathf.Approximately((int) value, value) ? "0" : "0.00";
                var formattedValue = value.ToString(stringFormat);
                _timeInput.text = formattedValue;
                _timeDisplay.text = formattedValue;
            }
        }

        public bool IsSelected => _spawnTypeDropdown.gameObject.activeInHierarchy;
        
        public SpawnPrefab SpawnPrefab
        {
            get => (SpawnPrefab) _spawnTypeDropdown.value;
            set => _spawnTypeDropdown.value = (int) value;
        }

        public Vector2 NormalizedPosition
        {
            get => _prefabInstance == null ? default : _prefabInstance.transform.position.WorldToViewportPosition();
            set
            {
                _prefabInstance.transform.position = value.ViewportToWorldPosition();
                _positionDisplay.text = value.ToString("0.00");
            }
        }

        private string _prefabText => SpawnPrefab.ToString();
        private string _timeText => GetFormattedTimeString(_timeInput.text);
        private string _positionText => NormalizedPosition.ToString("0.00");
        #endregion
        
        public void UpdateDisplayText()
        {
            _iconPreview.sprite = WaveEditorPrefabFactory.Instance.GetSprite(SpawnPrefab);
            _spawnTypeDisplay.text = _prefabText;
            _positionDisplay.text = _positionText;
            _timeInput.text = _timeText;
            _timeDisplay.text = _timeText;
        }

        public void Select(bool isSelected)
        {
            _buttonOutline.color = isSelected ? _selected : _unselected;
            _spawnTypeDropdown.gameObject.SetActive(isSelected);
            if (_prefabInstance != null && isSelected)
            {
                Selection.activeGameObject = _prefabInstance;
            }
        }

        private void Awake()
        {
            _spawnTypeDropdown.options = Enum.GetValues(typeof(SpawnPrefab)).Cast<SpawnPrefab>()
                .Select(prefabType => new TMP_Dropdown.OptionData(prefabType.ToString(), WaveEditorPrefabFactory.Instance.GetSprite(prefabType))).ToList();
            _spawnTypeDropdown.onValueChanged.AddListener(OnDropSelected);
            _timeInput.onValueChanged.AddListener(OnTimeValueChanged);
        }

        private void Start()
        {
            UpdateDisplayText();
        }
        private void Update()
        {
            NormalizedPosition = _prefabInstance == null ? default : _prefabInstance.transform.position.WorldToViewportPosition();
        }

        private void OnDestroy()
        {
            if (Application.isPlaying)
            {
                Destroy(_spawnTypeDropdown.gameObject);
            }
        }

        private void OnTimeValueChanged(string timeValue)
        {
            _timeDisplay.text = _timeText;
        }

        private string GetFormattedTimeString(string timeInput)
        {
            if(float.TryParse(timeInput, out var value))
            {
                var stringFormat = Mathf.Approximately((int)value, value) ? "0" : "0.00";
                var formattedValue = value.ToString(stringFormat);
                return formattedValue;
            }
            return timeInput;
        }

        private void OnDropSelected(int newValue)
        {
            OnDropdownSelected?.Invoke(newValue);
            if (_prefabInstance != null)
            {
                Selection.activeGameObject = _prefabInstance;
            }
        }
    }
}