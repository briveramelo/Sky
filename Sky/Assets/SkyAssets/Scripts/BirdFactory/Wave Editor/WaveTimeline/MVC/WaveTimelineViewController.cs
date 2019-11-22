using BRM.EventBrokers;
using BRM.EventBrokers.Interfaces;
using UnityEngine;

namespace BRM.Sky.WaveEditor
{
    public class WaveTimelineViewController : ViewController<WaveTimelineView>
    {
        [SerializeField] private WaveTimelineView _view;
        protected override WaveTimelineView View => _view;
        private IPublishEvents _eventPublisher = new StaticEventBroker();
        private IBrokerEvents _eventBroker = new StaticEventBroker();

        private void Awake()
        {
            View.OnWaveSelected += OnWaveSelected;
            _eventBroker.Subscribe<BatchStartData>(OnBatchStarted);
            _eventBroker.Subscribe<WaveEditorTestData>(OnWaveEditorStateChanged);
        }

        private void Start()
        {
            View.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            _eventBroker.Unsubscribe<BatchStartData>(OnBatchStarted);
            _eventBroker.Unsubscribe<WaveEditorTestData>(OnWaveEditorStateChanged);
        }

        private void OnBatchStarted(BatchStartData data)
        {
            View.SetCurrentBatch(data.StartedBatchIndex);
        }

        private void OnWaveEditorStateChanged(WaveEditorTestData data)
        {
            View.gameObject.SetActive(data.State == WaveEditorState.Testing);
        }

        private void OnWaveSelected(int waveNum)
        {
            _eventPublisher.Publish(new WaveEditorSliderData {TargetWave = waveNum});
        }
    }
}