using System;
using System.Collections;
using GenericFunctions;
using UnityEngine;

namespace BRM.Sky.WaveEditor
{
    public class BatchViewController : Selector, ISelectable
    {
        public event Action<int> OnButtonClicked;
        public bool IsSelected => _batchUi.IsSelected;
        public int Id { get; private set; }

        [SerializeField] private BatchDataMarshal _dataMarshal;
        [SerializeField] private BatchUi _batchUi;

        private GameObject _spawnEventsView;
        private GameObject _spawnEventsDirectParent;
        private AddSpawnEventButton _spawnEventButton;
        private BatchData _cachedData = new BatchData();

        public void Initialize(GameObject spawnEventsView, GameObject spawnEventsParent, AddSpawnEventButton spawnEventButton, int id)
        {
            _spawnEventButton = spawnEventButton;
            _spawnEventsView = spawnEventsView;
            _spawnEventsDirectParent = spawnEventsParent;
            _dataMarshal.SetSpawnEventParent(spawnEventsParent);
            Id = id;
        }
        
        public void Select(bool isSelected)
        {
            if (IsSelected && !isSelected)
            {
                _cachedData = _dataMarshal.Data;
            }
            _batchUi.Select(isSelected);
        }

        protected override IEnumerator OnClickRoutine()
        {
            OnButtonClicked?.Invoke(Id);
            if (!IsSelected)
            {
                _spawnEventsDirectParent.DestroyChildren("DontDestroyChild");
            }
            yield return null;

            Select(!IsSelected);
            
            if(IsSelected)
            {
                _spawnEventsView.SetActive(true);
                PopulateSpawnEventDataUi(_cachedData);
            }
        }

        private void PopulateSpawnEventDataUi(BatchData data)
        {
            var numButtons = data.SpawnEventData.Count;
            _spawnEventButton.CreateButtons(numButtons, () => _dataMarshal.Data = data);
        }
    }
}