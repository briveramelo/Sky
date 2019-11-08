using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private WaveUi _waveUi;
    [SerializeField] private Wave[] _storyWaves;
    [SerializeField] private Wave _endlessWave;

    private IWaveUi _myWaveUi;
    private IWaveRunnable[] _storyWaveCalls;
    private IWaveRunnable _endlessWaveCall;
    public static WaveName CurrentWave { get; private set; }

    private void Awake()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
        _storyWaveCalls = _storyWaves;
        _endlessWaveCall = _endlessWave;
        _myWaveUi = _waveUi;
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
            if (wave.MyWave == WaveName.Pigeon)
            {
                CurrentWave = wave.MyWave;
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

    #endregion

    private void RunEndlessWaves()
    {
        CurrentWave = WaveName.Endless;
        StartCoroutine(_endlessWaveCall.RunWave());
    }
}