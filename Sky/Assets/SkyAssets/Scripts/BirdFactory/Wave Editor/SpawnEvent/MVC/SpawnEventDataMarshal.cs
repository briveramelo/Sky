using BRM.Sky.CustomWaveData;
using UnityEngine;

namespace BRM.Sky.WaveEditor
{
    public class SpawnEventDataMarshal : DataMarshal<SpawnEventData, SpawnEventView>
    {
        [SerializeField] private SpawnEventView _view;
        protected override SpawnEventView View => _view;
        
        public override SpawnEventData Data
        {
            get => new SpawnEventData
            {
                SpawnPrefab = View.SpawnPrefab,
                NormalizedPosition = View.NormalizedPosition,
                TimeAfterBatchStartSec = View.Time
            };
            set
            {
                View.SpawnPrefab = value.SpawnPrefab;
                View.NormalizedPosition = value.NormalizedPosition;
                View.Time = value.TimeAfterBatchStartSec;
            }
        }

        public override bool IsDataReady => true;
    }
}