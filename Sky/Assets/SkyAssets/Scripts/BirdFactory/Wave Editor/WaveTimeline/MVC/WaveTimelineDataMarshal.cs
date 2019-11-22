using System.Collections.Generic;
using BRM.EventBrokers;
using BRM.EventBrokers.Interfaces;
using BRM.Sky.CustomWaveData;
using UnityEngine;

namespace BRM.Sky.WaveEditor
{
    public class WaveTimelineDataMarshal : DataMarshal<WaveTimeline, WaveTimelineView>
    {
        [SerializeField] private WaveTimelineView _view;

        public override bool IsDataReady => true;
        protected override WaveTimelineView View => _view;

        private IBrokerEvents _eventBroker = new StaticEventBroker();

        private void Awake()
        {
            _eventBroker.Subscribe<WaveEditorTestData>(OnWaveEditorTest);
        }

        private void OnDestroy()
        {
            _eventBroker.Unsubscribe<WaveEditorTestData>(OnWaveEditorTest);
        }

        private void OnWaveEditorTest(WaveEditorTestData data)
        {
            if (data.State == WaveEditorState.Testing)
            {
                Data = data.WaveData.WaveTimeline;
            }
        }

        public override WaveTimeline Data
        {
            get => GetCachedData();
            set
            {
                List<string> labels = new List<string>();
                for (int i = 0; i < value.Batches.Count; i++)
                {
                    var batchName = value.Batches[i].Name;
                    var label = string.IsNullOrWhiteSpace(batchName) ? i.ToString() : batchName;
                    labels.Add(label);
                }

                View.SetLabels(labels);
            }
        }
    }
}