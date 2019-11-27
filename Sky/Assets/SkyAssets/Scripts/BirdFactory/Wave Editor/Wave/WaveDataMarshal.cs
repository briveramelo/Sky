using System.Linq;
using BRM.Sky.CustomWaveData;
using GenericFunctions;
using UnityEngine;

namespace BRM.Sky.WaveEditor
{
    public class WaveDataMarshal : DataMarshal<WaveData, WaveView>
    {
        [SerializeField] private GameObject _dataMarshalsParent;
        [SerializeField] private WaveView _view;
        protected override WaveView View => _view;
        public override bool IsDataReady => !string.IsNullOrWhiteSpace(WaveName) && !string.IsNullOrWhiteSpace(View.SubtitleText);
        
        public string WaveName => View.WaveNameText;
        public override WaveData Data
        {
            get
            {
                _dataMarshalsParent.GetComponentsRecursively<BatchViewController>(true).ForEach(vc =>
                {
                    if (vc.IsSelected)
                    {
                        vc.GetComponent<BatchDataMarshal>().CacheData();
                    }
                });//ensures data is cached for any uncached, currently selected view controllers
                
                var batchDataMarshals = _dataMarshalsParent.GetComponentsRecursively<BatchDataMarshal>(true);
                var batchTriggerDataMarshals = _dataMarshalsParent.GetComponentsRecursively<TriggerDataMarshal>(true);

                return new WaveData
                {
                    Name = WaveName,
                    Subtitle = View.SubtitleText,

                    WaveTimeline = new WaveTimeline
                    {
                        Batches = batchDataMarshals.Select(marshal => marshal.GetCachedData()).ToList(),//batch ui repopulates with currently selected batch, so cached data is needed
                        Triggers = batchTriggerDataMarshals.Select(marshal => marshal.Data).ToList(),//trigger ui always represents the data, so regular data coming from ui is fine
                    }
                };
            }
            set
            {
                var batchDataMarshals = _dataMarshalsParent.GetComponentsRecursively<BatchDataMarshal>(true).ToList();
                var batchTriggerDataMarshals = _dataMarshalsParent.GetComponentsRecursively<TriggerDataMarshal>(true).ToList();

                for (int i = 0; i < batchDataMarshals.Count; i++)
                {
                    batchDataMarshals[i].Data = value.WaveTimeline.Batches[i];
                    batchDataMarshals[i].CacheData(value.WaveTimeline.Batches[i]);
                    batchTriggerDataMarshals[i].Data = value.WaveTimeline.Triggers[i];
                }
            }
        }
    }
}