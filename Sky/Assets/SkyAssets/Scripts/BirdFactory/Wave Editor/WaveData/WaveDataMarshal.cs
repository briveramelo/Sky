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
                var batchTriggerDataMarshals = _dataMarshalsParent.GetComponentsInChildren<TriggerDataMarshal>();

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

        public override bool IsDataReady => !string.IsNullOrWhiteSpace(_waveNameText.text) && !string.IsNullOrWhiteSpace(_subtitleText.text);
    }
}