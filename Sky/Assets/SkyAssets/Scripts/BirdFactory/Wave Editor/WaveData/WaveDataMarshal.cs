using System.Linq;
using TMPro;
using UnityEngine;

namespace BRM.Sky.WaveEditor
{
    public class WaveDataMarshal : DataMarshal<WaveData>
    {
        [SerializeField] private TMP_InputField _waveNameText;
        [SerializeField] private TMP_InputField _subtitleText;
        [SerializeField] private GameObject _dataMarshalsParent;

        public override WaveData Data
        {
            get
            {
                var batchDataMarshals = _dataMarshalsParent.GetComponentsInChildren<BatchDataMarshal>();
                var batchTriggerDataMarshals = _dataMarshalsParent.GetComponentsInChildren<BatchTriggerDataMarshal>();

                return new WaveData
                {
                    Name = _waveNameText.text,
                    Subtitle = _subtitleText.text,

                    WaveTimeline = new WaveTimeline
                    {
                        Batches = batchDataMarshals.Select(marshal => marshal.Data).ToList(),
                        Triggers = batchTriggerDataMarshals.Select(marshal => marshal.Data).ToList(),
                    }
                };
            }
        }

        public override bool IsDataReady { get; }
    }
}