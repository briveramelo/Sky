using System;
using System.Collections;
using UnityEngine;

namespace BRM.Sky.WaveEditor
{
    public class SpawnEventViewController : Selector
    {
        public event Action OnButtonClicked;

        [SerializeField] private SpawnEventUi _ui;
        [SerializeField] private SpawnTypeSelector _spawnTypeSelector;

        private void Start()
        {
            _spawnTypeSelector.CreateEditorInstance(0);
            _spawnTypeSelector.OnSpawnSelected += OnSpawnTypeSelected;
        }

        private void OnSpawnTypeSelected(SpawnPrefab selectedPrefab)
        {
            _ui.UpdateUi();
        }

        protected override IEnumerator OnClickRoutine()
        {
            bool isSelected = _ui.IsSelected;
            OnButtonClicked?.Invoke();
            yield return null;
            Select(!isSelected);
        }

        public void Select(bool isSelected)
        {
            _ui.UpdateUi();
            _ui.Select(isSelected);
        }
    }
}