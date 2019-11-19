using System.Linq;
using GenericFunctions;
using UnityEngine;

namespace BRM.Sky.WaveEditor
{
    public class BatchDataMarshal : DataMarshal<BatchData>
    {
        private GameObject _spawnEventParent;

        public void SetSpawnEventParent(GameObject spawnEventParent)
        {
            _spawnEventParent = spawnEventParent;
        }

        public override BatchData Data
        {
            get
            {
                var spawnEventDataMarshals = _spawnEventParent.GetComponentsRecursively<SpawnEventDataMarshal>(true);
                var data = new BatchData
                {
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

        public override bool IsDataReady => true;
    }
}