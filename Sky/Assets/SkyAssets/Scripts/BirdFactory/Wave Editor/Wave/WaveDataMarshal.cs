using System.Linq;
using BRM.Sky.CustomWaveData;
using GenericFunctions;
using TMPro;
using UnityEngine;

namespace BRM.Sky.WaveEditor
{
    public class WaveDataMarshal : DataMarshal<WaveData, WaveView>
    {
        [SerializeField] private GameObject _dataMarshalsParent;
        [SerializeField] private WaveView _view;

        public string WaveName => View.WaveNameText;
        public override WaveData Data
        {
            get
            {
                _dataMarshalsParent.GetComponentsRecursively<BatchViewController>().ForEach(vc =>
                {
                    if (vc.IsSelected)
                    {
                        vc.Select(false);
                    }
                });//ensures data is cached for any uncached, currently selected view controllers
                
                var batchDataMarshals = _dataMarshalsParent.GetComponentsInChildren<BatchDataMarshal>();
                var batchTriggerDataMarshals = _dataMarshalsParent.GetComponentsInChildren<TriggerDataMarshal>();

                return new WaveData
                {
                    Name = WaveName,
                    Subtitle = View.SubtitleText,

                    WaveTimeline = new WaveTimeline
                    {
                        Batches = batchDataMarshals.Select(marshal => marshal.GetCachedData()).ToList(),
                        Triggers = batchTriggerDataMarshals.Select(marshal => marshal.Data).ToList(),
                    }
                };
            }
            set
            {
                var batchDataMarshals = _dataMarshalsParent.GetComponentsInChildren<BatchDataMarshal>().ToList();
                var batchTriggerDataMarshals = _dataMarshalsParent.GetComponentsInChildren<TriggerDataMarshal>().ToList();

                for (int i = 0; i < batchDataMarshals.Count; i++)
                {
                    batchDataMarshals[i].Data = value.WaveTimeline.Batches[i];
                    batchTriggerDataMarshals[i].Data = value.WaveTimeline.Triggers[i];
                }
            }
        }

        public override bool IsDataReady => !string.IsNullOrWhiteSpace(WaveName) && !string.IsNullOrWhiteSpace(View.SubtitleText);
        protected override WaveView View => _view;
    }
}