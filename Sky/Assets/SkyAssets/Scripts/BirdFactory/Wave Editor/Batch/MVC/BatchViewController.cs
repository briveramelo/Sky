using System;
using System.Collections;
using BRM.Sky.CustomWaveData;
using BRM.Sky.WaveEditor.Ui;
using GenericFunctions;
using TMPro;
using UnityEngine;

namespace BRM.Sky.WaveEditor
{
    public class BatchViewController : SelectorViewController<BatchView>, ISelectable
    {
        public event Action<int> OnButtonClicked;
        public bool IsSelected => _batchView.IsSelected;
        public int Id { get; private set; }

        [SerializeField] private BatchDataMarshal _dataMarshal;
        [SerializeField] private BatchView _batchView;

        protected override BatchView View => _batchView;
        
        private GameObject _spawnEventsView;
        private GameObject _spawnEventsDirectParent;
        private SpawnEventButtonFactory _spawnEventButtonFactory;

        public void Initialize(GameObject spawnEventsView, GameObject spawnEventsParent, SpawnEventButtonFactory spawnEventButtonFactory, TMP_InputField batchNameInput, int id)
        {
            _spawnEventButtonFactory = spawnEventButtonFactory;
            _spawnEventsView = spawnEventsView;
            _spawnEventsDirectParent = spawnEventsParent;
            
            View.Initialize(batchNameInput);
            _dataMarshal.Initialize(spawnEventsParent, View);
            Id = id;
        }

        public void Select(bool isSelected)
        {
            if (IsSelected && !isSelected)
            {
                _dataMarshal.CacheData();
            }
            View.Select(isSelected);
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
            _spawnEventButtonFactory.CreateButtons(numButtons, () => _dataMarshal.Data = data);
        }
    }
}