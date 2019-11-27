using System;
using System.Collections.Generic;
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

        [SerializeField] private TMP_Dropdown _spawnTypeDropdown;
        [SerializeField] private TMP_InputField _timeInput;

        [SerializeField] private Image _iconPreview;
        [SerializeField] private TextMeshProUGUI _spawnTypeDisplay;
        [SerializeField] private TextMeshProUGUI _positionDisplay;
        [SerializeField] private TextMeshProUGUI _timeDisplay;

        [SerializeField] private Image _buttonOutline;
        [SerializeField] private Color _selected, _unselected;

        private GameObject _prefabInstance;
        private readonly GameObjectSelector _selector = new EditorSelector(); //todo: abstract for builds (now it's editor only)

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

        public string BatchName
        {
            get => _spawnTypeDisplay.text;
            set => _spawnTypeDisplay.text = value;
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
                _selector.Select(_prefabInstance);
            }
        }

        private void Awake()
        {
            if (_spawnTypeDropdown.options.Count == 0)
            {
                SetBatchDropdowns(null);
            }

            _spawnTypeDropdown.onValueChanged.AddListener(OnDropSelected);
            _timeInput.onValueChanged.AddListener(OnTimeValueChanged);
        }

        public void SetBatchDropdowns(List<BatchData> batchDropdowns)
        {
            var baseSet = EnumHelpers.GetAll<SpawnPrefab>();
            baseSet.Remove(SpawnPrefab.Batch);
            _spawnTypeDropdown.options = baseSet.Select(prefabType => new TMP_Dropdown.OptionData(prefabType.ToString(), WaveEditorPrefabFactory.Instance.GetSprite(prefabType))).ToList();

            if (batchDropdowns == null)
            {
                return;
            }

            for (int i = 0; i < batchDropdowns.Count; i++)
            {
                var batch = batchDropdowns[i];
                _spawnTypeDropdown.options.Add(new TMP_Dropdown.OptionData(batch.Name, WaveEditorPrefabFactory.Instance.GetSprite(SpawnPrefab.Batch)));
            }
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
            if (float.TryParse(timeInput, out var value))
            {
                var stringFormat = Mathf.Approximately((int) value, value) ? "0" : "0.00";
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
                _selector.Select(_prefabInstance);
            }
        }
    }
}