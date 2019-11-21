using System;
using System.Collections;
using BRM.Sky.CustomWaveData;
using UnityEngine;

namespace BRM.Sky.WaveEditor
{
    public class SpawnEventViewController : SelectorViewController<SpawnEventView>, ISelectable
    {
        #region Variables
        public event Action<int> OnButtonClicked;
        public bool IsSelected => _ui.IsSelected;
        public int Id { get; set; }

        [SerializeField] private SpawnEventView _ui;
        
        protected override SpawnEventView View => _ui;
        #endregion

        private GameObject _prefabInstance;
        
        public void Select(bool isSelected)
        {
            _ui.UpdateDisplayText();
            _ui.Select(isSelected);
        }

        public SpawnPrefab SpawnPrefab
        {
            get => View.SpawnPrefab;
            set
            {
                CreateEditorInstance(value);
                View.SpawnPrefab = value;
            }
        }

        protected override IEnumerator OnClickRoutine()
        {
            bool isSelected = _ui.IsSelected;
            OnButtonClicked?.Invoke(Id);
            yield return null;
            Select(!isSelected);
        }

        private void Start()
        {
            View.OnDropdownSelected += OnDropdownSelected;
            if (_prefabInstance == null)
            {
                CreateEditorInstance(0);
                Select(true);
            }
        }

        private void OnDestroy()
        {
            if (_prefabInstance != null && Application.isPlaying)
            {
                //note: OnDestroy is not called if the gameobject never becomes active.
                //i assume awakening/starting also triggers registering of other unity callbacks, like OnDestroy
                Destroy(_prefabInstance);
            }
        }

        private void OnDropdownSelected(int value)
        {
            CreateEditorInstance((SpawnPrefab) value);
            _ui.UpdateDisplayText();
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
            View.SetPrefabInstance(_prefabInstance);
        }
    }
}