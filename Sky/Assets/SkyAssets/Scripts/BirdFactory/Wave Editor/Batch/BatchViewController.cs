using System;
using System.Collections;
using BRM.Sky.CustomWaveData;
using GenericFunctions;
using TMPro;
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

        public void Initialize(GameObject spawnEventsView, GameObject spawnEventsParent, AddSpawnEventButton spawnEventButton, TMP_InputField batchNameText, int id)
        {
            _spawnEventButton = spawnEventButton;
            _spawnEventsView = spawnEventsView;
            _spawnEventsDirectParent = spawnEventsParent;
            _dataMarshal.Initialize(spawnEventsParent, batchNameText);
            Id = id;
        }

        public void Select(bool isSelected)
        {
            if (IsSelected && !isSelected)
            {
                _dataMarshal.CacheData();
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
                PopulateSpawnEventDataUi(_dataMarshal.GetCachedData());
            }
        }

        private void PopulateSpawnEventDataUi(BatchData data)
        {
            var numButtons = data.SpawnEventData.Count;
            _spawnEventButton.CreateButtons(numButtons, () => _dataMarshal.Data = data);
        }
    }
}