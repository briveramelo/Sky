using BRM.Sky.CustomWaveData;
using TMPro;
using UnityEngine;

namespace BRM.Sky.WaveEditor
{
    public class SpawnEventDataMarshal : DataMarshal<SpawnEventData, SpawnEventView>
    {
        [SerializeField] private SpawnEventView _view;
        [SerializeField] private SpawnEventViewController _controller;

        public void Initialize(TMP_InputField input)
        {
            _batchNameInput = input;
        }

        private TMP_InputField _batchNameInput;
        protected override SpawnEventView View => _view;
        
        public override SpawnEventData Data
        {
            get => new SpawnEventData
            {
                BatchName = View.BatchName,
                SpawnPrefab = View.SpawnPrefab,
                NormalizedPosition = View.NormalizedPosition,
                TimeAfterBatchStartSec = View.Time
            };
            set
            {
                _batchNameInput.text = value.BatchName;
                View.BatchName = value.BatchName;
                _controller.SpawnPrefab = value.SpawnPrefab;
                View.SpawnPrefab = value.SpawnPrefab;
                View.NormalizedPosition = value.NormalizedPosition;
                View.Time = value.TimeAfterBatchStartSec;
            }
        }

        public override bool IsDataReady => true;
    }
}