using UnityEngine;

namespace BRM.Sky.WaveEditor
{
    public class SpawnEventDataMarshal : DataMarshal<SpawnEventData>
    {
        [SerializeField] private SpawnTypeDropdown _spawnTypeDropdown;
        [SerializeField] private SpawnPositionMarshal _positionMarshal;
        [SerializeField] private SpawnTimeInput _timeInput;

        public override SpawnEventData Data
        {
            get => new SpawnEventData
            {
                SpawnPrefab = _spawnTypeDropdown.SpawnPrefab,
                NormalizedPosition = _positionMarshal.NormalizedPosition,
                TimeAfterBatchStartSec = float.Parse(_timeInput.Text)
            };
            set
            {
                _spawnTypeDropdown.SpawnPrefab = value.SpawnPrefab;
                _positionMarshal.NormalizedPosition = value.NormalizedPosition;
                _timeInput.Text = value.TimeAfterBatchStartSec.ToString("0.00");
            }
        }

        public override bool IsDataReady => true;
    }
}