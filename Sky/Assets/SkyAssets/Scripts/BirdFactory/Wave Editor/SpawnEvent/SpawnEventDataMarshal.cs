using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BRM.Sky.WaveEditor
{
    public class SpawnEventDataMarshal : DataMarshal<SpawnEventData>
    {
        [SerializeField] private SpawnTypeDropdown _spawnTypeDropdown;
        [SerializeField] private SpawnPositionMarshal _positionMarshal;
        [SerializeField] private TMP_InputField _timeInput;

        public override SpawnEventData Data => new SpawnEventData
        {
            SpawnPrefab = _spawnTypeDropdown.SpawnPrefab,
            Position = _positionMarshal.NormalizedPosition,
            TimeAfterBatchStartSec = float.Parse(_timeInput.text)
        };
        public override bool IsDataReady { get; }
    }
}