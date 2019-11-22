using System.Collections;
using System.Linq;
using BRM.EventBrokers;
using BRM.EventBrokers.Interfaces;
using BRM.Sky.CustomWaveData;
using UnityEngine;

public class DataWave : Wave
{
    public override string WaveName => _waveData.Name;
    public override string Subtitle => _waveData.Subtitle;

    private WaveData _waveData;
    private float _batchStartTime;
    private float _currentTimeAfterBatchStart => Time.time - _batchStartTime;

    private Coroutine _currentBatch;
    private int _currentBatchIndex;
    private IBrokerEvents _eventBroker = new StaticEventBroker();
    private IPublishEvents _eventPublisher = new StaticEventBroker();

    private void Awake()
    {
        _eventBroker.Subscribe<WaveEditorSliderData>(OnWaveSetFromWaveEditor);
    }

    private void OnDestroy()
    {
        _eventBroker.Unsubscribe<WaveEditorSliderData>(OnWaveSetFromWaveEditor);
    }

    public void SetWaveData(WaveData waveData)
    {
        _waveData = waveData;
    }
    
    private void OnWaveSetFromWaveEditor(WaveEditorSliderData data)
    {
        var targetWave = data.TargetWave;
        if (_waveData != null && _currentBatch != null)
        {
            StopCoroutine(_currentBatch);
            _currentBatch = StartCoroutine(RunBatch(_waveData.WaveTimeline.Batches[targetWave], _waveData.WaveTimeline.Triggers[targetWave], targetWave));
        }
    }

    public override IEnumerator GenerateBirds()
    {
        var batches = _waveData.WaveTimeline.Batches;
        var triggers = _waveData.WaveTimeline.Triggers;
        var numWaves = batches.Count;
        for (_currentBatchIndex = 0; _currentBatchIndex < numWaves; _currentBatchIndex++)
        {
            _currentBatch = StartCoroutine(RunBatch(batches[_currentBatchIndex], triggers[_currentBatchIndex], _currentBatchIndex));
            yield return _currentBatch;
        }
    }

    private IEnumerator RunBatch(BatchData batch, BatchTriggerData trigger, int batchIndex)
    {
        yield return null;//to prevent collisions with the spawn factory destroying all these...
        _eventPublisher.Publish(new BatchStartData{StartedBatchIndex = batchIndex});
        ScoreSheet.Resetter.ResetBatch();
        yield return StartCoroutine(GenerateBatch(batch));

        yield return null; //wait for unity lifecycle events in the awake/start functions to count spawned/alive tallys
        var batchTrigger = TriggerFactory.Create(trigger);
        while (!batchTrigger.CanAdvance)
        {
            yield return null;
        }
    }

    private IEnumerator GenerateBatch(BatchData batchData)
    {
        _batchStartTime = Time.time;

        var spawnEventData = batchData.SpawnEventData;
        var spawnCount = spawnEventData.Count;
        spawnEventData = spawnEventData.OrderBy(spawnEvent => spawnEvent.TimeAfterBatchStartSec).ToList();
        for (int i = 0; i < spawnCount; i++)
        {
            var spawnEvent = spawnEventData[i];
            var waitTime = _currentTimeAfterBatchStart - spawnEvent.TimeAfterBatchStartSec;
            if (!Mathf.Approximately(0, waitTime))
            {
                yield return new WaitForSeconds(waitTime);
            }

            var instance = SpawnFactory.Instance.CreateInstance(spawnEvent.SpawnPrefab);
            instance.transform.position = spawnEvent.NormalizedPosition.ViewportToWorldPosition();
        }
    }
}