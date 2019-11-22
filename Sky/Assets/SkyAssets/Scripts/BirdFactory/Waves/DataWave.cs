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
    private float _batchStartTime;
    private float _currentTimeAfterWaveStart => Time.time - _waveStartTime;
    private float _currentTimeAfterBatchStart => Time.time - _batchStartTime;

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
            ScoreSheet.Resetter.ResetBatch();
            yield return StartCoroutine(RunBatch(batches[i]));

            yield return null;//wait for unity lifecycle events in the awake/start functions to count spawned/alive tallys
            var batchTrigger = TriggerFactory.Create(triggers[i]);
            while (!batchTrigger.CanAdvance)
            {
                yield return null;
            }
        }
    }

    private IEnumerator RunBatch(BatchData batchData)
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