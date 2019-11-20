using BRM.Sky.CustomWaveData;
using UnityEngine;

namespace BRM.Sky.WaveEditor
{
    public class SpawnEventDataMarshal : DataMarshal<SpawnEventData>
    {
        [SerializeField] private SpawnTypeSelector _spawnTypeSelector;
        [SerializeField] private SpawnTimeInput _timeInput;

        public override SpawnEventData Data
        {
            get => new SpawnEventData
            {
                SpawnPrefab = _spawnTypeSelector.SpawnPrefab,
                NormalizedPosition = _spawnTypeSelector.NormalizedPosition,
                TimeAfterBatchStartSec = _timeInput.Time
            };
            set
            {
                _spawnTypeSelector.SpawnPrefab = value.SpawnPrefab;
                _spawnTypeSelector.NormalizedPosition = value.NormalizedPosition;
                _timeInput.Time = value.TimeAfterBatchStartSec;
            }
        }

        public override bool IsDataReady => true;
    }
}