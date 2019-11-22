using System.Collections;
using BRM.Sky.CustomWaveData;
using UnityEngine;

public class WaveRunner : MonoBehaviour
{
    private Wave _wave;
    private WaveUi _waveUi;
    public WaveName CurrentWave => _wave.WaveNameType;
    public bool IsInitialized => _waveUi;
    public void Initialize(WaveUi waveUi)
    {
        _waveUi = waveUi;
    }

    public void SetWave(Wave wave)
    {
        _wave = wave;
    }
    
    public IEnumerator RunWave()
    {
        yield return StartCoroutine(StartWave());
        
        yield return StartCoroutine(_wave.GenerateBirds());
        
        yield return StartCoroutine(FinishWave());
    }

    private IEnumerator StartWave()
    {
        yield return StartCoroutine(_waveUi.AnimateWaveStart(_wave.WaveName, _wave.Subtitle));
    }

    private IEnumerator FinishWave()
    {
        ScoreSheet.Resetter.ResetWave();
        
        yield return new WaitForSeconds(2f);
        var instance = SpawnFactory.Instance.CreateInstance(SpawnPrefab.BirdOfParadise);
        instance.transform.position = new Vector2(1.05f, 0.3f).ViewportToWorldPosition();
        
        var trigger = TriggerFactory.Create(new BatchTriggerData {TriggerType = BatchTriggerType.AllDead});
        while (!trigger.CanAdvance)
        {
            yield return null;
        }
        yield return StartCoroutine(_waveUi.AnimateWaveEnd());
    }
}