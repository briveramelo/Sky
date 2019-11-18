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
                var spawnEventDataMarshals = _spawnEventParent.GetComponentsRecursively<SpawnEventDataMarshal>();
                return new BatchData
                {
                    SpawnEventData = spawnEventDataMarshals.Select(marshal => marshal.Data).ToList()
                };
            }
            set
            {
                var marshals = _spawnEventParent.GetComponentsRecursively<SpawnEventDataMarshal>().ToList();
                for (int i = 0; i < marshals.Count; i++)
                {
                    marshals[i].Data = value.SpawnEventData[i];
                }
            }
        }

        public override bool IsDataReady => true;
    }
}