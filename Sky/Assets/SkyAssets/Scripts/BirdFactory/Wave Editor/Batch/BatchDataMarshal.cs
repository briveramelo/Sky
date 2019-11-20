using System.Linq;
using BRM.Sky.CustomWaveData;
using GenericFunctions;
using TMPro;
using UnityEngine;

namespace BRM.Sky.WaveEditor
{
    public class BatchDataMarshal : DataMarshal<BatchData>
    {
        private TMP_InputField _batchNameText;
        private GameObject _spawnEventParent;
        private BatchData _cachedData = new BatchData();

        public void Initialize(GameObject spawnEventParent, TMP_InputField batchNameText)
        {
            _batchNameText = batchNameText;
            _spawnEventParent = spawnEventParent;
        }

        public string BatchName => _batchNameText == null ? "" : _batchNameText.text;
        public override bool IsDataReady => !string.IsNullOrWhiteSpace(BatchName);

        public void CacheData()
        {
            _cachedData = Data;
        }

        public BatchData GetCachedData() => _cachedData;

        public override BatchData Data
        {
            get
            {
                var spawnEventDataMarshals = _spawnEventParent.GetComponentsRecursively<SpawnEventDataMarshal>(true);
                var data = new BatchData
                {
                    Name = BatchName,
                    SpawnEventData = spawnEventDataMarshals.Select(marshal => marshal.Data).ToList()
                };
                return data;
            }
            set
            {
                if (value == null || value.SpawnEventData.Count == 0)
                {
                    return;
                }

                var marshals = _spawnEventParent.GetComponentsRecursively<SpawnEventDataMarshal>(true);
                for (int i = 0; i < marshals.Count; i++)
                {
                    marshals[i].Data = value.SpawnEventData[i];
                }
            }
        }
    }
}