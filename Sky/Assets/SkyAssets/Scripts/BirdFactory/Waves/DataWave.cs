using System.Collections;
using System.Linq;
using BRM.Sky.CustomWaveData;
using UnityEngine;

public class DataWave : Wave
{
    public override string WaveName => _waveData.Name;
    public override string Subtitle => _waveData.Subtitle;
    
    private WaveData _waveData;
    private float _waveStartTime;
    private float _currentTimeAfterWaveStart => Time.time - _waveStartTime;

    public void SetWaveData(WaveData waveData)
    {
        _waveData = waveData;
    }

    public override IEnumerator GenerateBirds()
    {
        _waveStartTime = Time.time;
        
        var batches = _waveData.WaveTimeline.Batches;
        var triggers = _waveData.WaveTimeline.Triggers;
        var numWaves = batches.Count;
        for (int i = 0; i < numWaves; i++)
        {
            yield return StartCoroutine(RunBatch(batches[i]));
            
            var batchTrigger = TriggerFactory.Create(triggers[i]);
            while (batchTrigger.IsWaiting)
            {
                yield return null;
            }
        }
    }

    private IEnumerator RunBatch(BatchData batchData)
    {
        var spawnEventData = batchData.SpawnEventData;
        var spawnCount = spawnEventData.Count;
        spawnEventData = spawnEventData.OrderBy(spawnEvent => spawnEvent.TimeAfterBatchStartSec).ToList();
        for (int i = 0; i < spawnCount; i++)
        {
            var spawnEvent = spawnEventData[i];
            var waitTime = _currentTimeAfterWaveStart - spawnEvent.TimeAfterBatchStartSec;
            if (!Mathf.Approximately(0, waitTime))
            {
                yield return new WaitForSeconds(waitTime);
            }
            var instance = SpawnFactory.Instance.CreateInstance(spawnEvent.SpawnPrefab);
            instance.transform.position = spawnEvent.NormalizedPosition.ViewportToWorldPosition();
        }
    }
}