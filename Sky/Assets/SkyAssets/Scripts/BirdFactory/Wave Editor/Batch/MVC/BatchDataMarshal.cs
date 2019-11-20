using System.Linq;
using BRM.Sky.CustomWaveData;
using GenericFunctions;
using UnityEngine;

namespace BRM.Sky.WaveEditor
{
    public class BatchDataMarshal : DataMarshal<BatchData, BatchView>
    {
        private GameObject _spawnEventParent;
        private BatchView _view;
        protected override BatchView View => _view;

        public void Initialize(GameObject spawnEventParent, BatchView view)
        {
            _view = view;
            _spawnEventParent = spawnEventParent;
        }

        public string BatchName => View == null ? "" : View.BatchName;
        public override bool IsDataReady => !string.IsNullOrWhiteSpace(BatchName);

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