using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEditor;

namespace BRM.Sky.WaveEditor
{
    public class SpawnTypeSelector : MonoBehaviour
    {
        public event Action<SpawnPrefab> OnSpawnSelected;

        public SpawnPrefab SpawnPrefab
        {
            get => (SpawnPrefab) _dropdown.value;
            set
            {
                _dropdown.value = (int) value;
                if (_prefabInstance == null)
                {
                    Debug.Log(value);
                    CreateEditorInstance(value);
                }
            }
        }

        public Vector2 NormalizedPosition
        {
            get => _prefabInstance == null ? Vector2.zero : _prefabInstance.transform.position.WorldToViewportPosition();
            set
            {
                if (_prefabInstance == null)
                {
                    return;
                }

                _prefabInstance.transform.position = value.ViewportToWorldPosition();
            }
        }

        public string PositionText => NormalizedPosition.ToString("0.00");
        public string PrefabText => SpawnPrefab.ToString();

        [SerializeField] private TMP_Dropdown _dropdown;

        private GameObject _prefabInstance;

        private void Awake()
        {
            _dropdown.options = Enum.GetValues(typeof(SpawnPrefab)).Cast<SpawnPrefab>()
                .Select(prefabType => new TMP_Dropdown.OptionData(prefabType.ToString(), WaveEditorPrefabFactory.Instance.GetSprite(prefabType))).ToList();
            _dropdown.onValueChanged.AddListener(OnDropdownSelected);
            if (_prefabInstance == null)
            {
                CreateEditorInstance(0);
            }
        }

        private void OnDestroy()
        {
            if (_prefabInstance != null && Application.isPlaying)
            {
                Destroy(_prefabInstance);
            }
        }

        private void OnDropdownSelected(int value)
        {
            CreateEditorInstance((SpawnPrefab) value);
        }

        private void CreateEditorInstance(SpawnPrefab prefabType)
        {
            var lastPosition = new Vector2(-ScreenSpace.SpawnEdge.x, 0);
            if (_prefabInstance != null)
            {
                lastPosition = _prefabInstance.transform.position;
                Destroy(_prefabInstance);
            }

            _prefabInstance = WaveEditorPrefabFactory.Instance.CreateInstance(prefabType);
            _prefabInstance.transform.position = lastPosition;

            OnSpawnSelected?.Invoke(prefabType);

            Select(true);
        }

        public void Select(bool isSelected)
        {
            if (_prefabInstance != null && isSelected)
            {
                Selection.activeGameObject = _prefabInstance;
            }
        }
    }
}