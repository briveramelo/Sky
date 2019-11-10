using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private Wave[] _storyWaves;
    [SerializeField] private Wave _endlessWave;

    private IWaveUi _myWaveUi;
    private List<IWaveRunnable> _storyWaveCalls;
    private IWaveRunnable _endlessWaveCall;
    public static WaveName CurrentWave { get; private set; }

    private void Awake()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
        _storyWaveCalls = _storyWaves.Cast<IWaveRunnable>().ToList();
        _endlessWaveCall = _endlessWave;
    }

    private void Start()
    {
        _myWaveUi = FindObjectOfType<WaveUi>();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }
    
    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        ChooseMode(scene.name);
    }

    private void ChooseMode(string loadedScene)
    {
        Debug.LogFormat("mode chosen with scene: {0}", loadedScene);
        switch (loadedScene)
        {
            case Scenes.Story:
                StartCoroutine(RunStoryWaves());
                break;
            case Scenes.Endless:
                RunEndlessWaves();
                break;
            default:
                Debug.LogFormat("coroutines stopped with scene: {0}", loadedScene);
                StopAllCoroutines();
                break;
        }
    }

    #region StoryWaves

    private IEnumerator RunStoryWaves()
    {
        foreach (var wave in _storyWaveCalls)
        {
            if (wave.WaveName == WaveName.Pigeon)
            {
                CurrentWave = wave.WaveName;
                yield return StartCoroutine(wave.RunWave());
            }
        }

        yield return StartCoroutine(FinishStoryMode());
    }

    private IEnumerator StartStoryMode()
    {
        CurrentWave = WaveName.Intro;
        yield return StartCoroutine(_myWaveUi.AnimateStoryStart());
    }

    private IEnumerator FinishStoryMode()
    {
        Debug.Log("Play Victory noises and stuff");
        CurrentWave = WaveName.Complete;
        ScoreSheet.Reporter.ReportScores();
        yield return StartCoroutine(_myWaveUi.AnimateStoryEnd());

        SceneManager.LoadScene(Scenes.Menu);
    }

    public void KillRunningWaves()
    {
        StopAllCoroutines();
    }

    public void RunWave(WaveName waveName)
    {
        var waveCall = _storyWaveCalls.Find(wave => wave.WaveName == waveName);
        if (waveCall != null)
        {
            StartCoroutine(waveCall.RunWave());
        }
        else
        {
            Debug.LogErrorFormat("No Wave of type: {0} was found", waveName);
        }
    }

    #endregion

    private void RunEndlessWaves()
    {
        CurrentWave = WaveName.Endless;
        StartCoroutine(_endlessWaveCall.RunWave());
    }
}