using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BRM.EventBrokers;
using BRM.EventBrokers.Interfaces;
using UnityEngine.SceneManagement;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private List<Wave> _storyWaves;
    [SerializeField] private EndlessWave _endlessWave;
    [SerializeField] private DataWave _dataWave;
    [SerializeField] private WaveRunner _waveRunner;

    private WaveUi waveUi;

    private WaveUi _waveUi
    {
        get
        {
            if (waveUi)
            {
                return waveUi;
            }

            waveUi = FindObjectOfType<WaveUi>();
            return waveUi;
        }
    }
    public static WaveName CurrentWave { get; private set; }
    
    private IBrokerEvents _eventBroker = new StaticEventBroker();

    private void Awake()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
        _eventBroker.Subscribe<WaveEditorTestData>(OnWaveEditorStateChange);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
        _eventBroker.Unsubscribe<WaveEditorTestData>(OnWaveEditorStateChange);
    }
    
    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        ChooseMode(scene.name);
    }

    private void OnWaveEditorStateChange(WaveEditorTestData data)
    {
        KillRunningWaves();
        switch (data.State)
        {
            case WaveEditorState.Editing: 
                break;
            case WaveEditorState.Testing:
                _dataWave.SetWaveData(data.WaveData);
                RunWave(_dataWave);
                break;
        }
    }

    private void ChooseMode(string loadedScene)
    {
        switch (loadedScene)
        {
            case Scenes.Story:
                StartCoroutine(RunStoryWaves());
                break;
            case Scenes.Endless:
                RunWave(_endlessWave);
                break;
            default:
                Debug.LogFormat("coroutines stopped with scene: {0}", loadedScene);
                StopAllCoroutines();
                return;
        }
        Debug.LogFormat("mode chosen with scene: {0}", loadedScene);
    }

    #region StoryWaves

    private IEnumerator RunStoryWaves()
    {
        foreach (var wave in _storyWaves)
        {
            yield return RunWave(wave);
        }

        yield return StartCoroutine(FinishStoryMode());
    }

    private IEnumerator StartStoryMode()
    {
        CurrentWave = WaveName.Intro;
        yield return StartCoroutine(_waveUi.AnimateStoryStart());
    }

    private IEnumerator FinishStoryMode()
    {
        Debug.Log("Play Victory noises and stuff");
        CurrentWave = WaveName.Complete;
        ScoreSheet.Reporter.ReportScores();
        yield return StartCoroutine(_waveUi.AnimateStoryEnd());

        SceneManager.LoadScene(Scenes.Menu);
    }

    public void KillRunningWaves()
    {
        StopAllCoroutines();
    }

    public void RunStoryWave(WaveName waveName)
    {
        var wave = _storyWaves.Find(wav => wav.WaveNameType == waveName);
        if (wave != null)
        {
            RunWave(wave);
        }
        else
        {
            Debug.LogErrorFormat("No Story Wave of type: {0} was found", waveName);
        }
    }

    #endregion

    private Coroutine RunWave(Wave wave)
    {
        if (!_waveRunner.IsInitialized)
        {
            _waveRunner.Initialize(_waveUi);
        }

        _waveRunner.SetWave(wave);
        CurrentWave = _waveRunner.CurrentWave;
        return StartCoroutine(_waveRunner.RunWave());
    }
}