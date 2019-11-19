using System;
using System.Collections;
using UnityEngine;

namespace BRM.Sky.WaveEditor
{
    public class SpawnEventViewController : Selector, ISelectable
    {
        public event Action<int> OnButtonClicked;

        [SerializeField] private SpawnEventUi _ui;
        [SerializeField] private SpawnTypeSelector _spawnTypeSelector;

        public bool IsSelected => _ui.IsSelected;
        public int Id { get; set; }

        public void Select(bool isSelected)
        {
            _ui.UpdateUi();
            _ui.Select(isSelected);
        }

        private void Start()
        {
            _spawnTypeSelector.OnSpawnSelected += OnSpawnTypeSelected;
        }

        private void OnDestroy()
        {
            if (Application.isPlaying)
            {
                Destroy(_spawnTypeSelector.gameObject);
            }
        }

        private void OnSpawnTypeSelected(SpawnPrefab selectedPrefab)
        {
            _ui.UpdateUi();
        }

        protected override IEnumerator OnClickRoutine()
        {
            bool isSelected = _ui.IsSelected;
            OnButtonClicked?.Invoke(Id);
            yield return null;
            Select(!isSelected);
        }
    }
}