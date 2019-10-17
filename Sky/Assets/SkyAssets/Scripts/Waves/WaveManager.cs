using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class WaveManager : MonoBehaviour {

    [SerializeField] private WaveUI waveUI;
    private IWaveUI myWaveUI;
    [SerializeField] private Wave[] storyWaves;
    private IWaveRunnable[] storyWaveCalls;
    [SerializeField] private Wave endlessWave;
    private IWaveRunnable endlessWaveCall;
    private static WaveName currentWave;            public static WaveName CurrentWave => currentWave;

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode) {
        ChooseMode(scene.name);
    }

    private void Awake(){
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
        storyWaveCalls = storyWaves;
        endlessWaveCall = endlessWave;
        myWaveUI = waveUI;
        StopAllCoroutines();
        ChooseMode(SceneManager.GetActiveScene().name);
	}

    private void ChooseMode(string loadedScene) {
        switch (loadedScene) {
            case Scenes.Menu:
                StopAllCoroutines();
                break;
            case Scenes.Story:
                StartCoroutine(RunStoryWaves());
                break;
            case Scenes.Endless:
                RunEndlessWaves();
                break;
            case Scenes.Scores:
                break;
        }
    }

    #region StoryWaves

    private IEnumerator RunStoryWaves() {
        //yield return StartCoroutine(StartStoryMode());
        foreach (IWaveRunnable wave in storyWaveCalls){
            if (wave.MyWave == WaveName.Pigeon) {
                currentWave = wave.MyWave;
                yield return StartCoroutine (wave.RunWave());
            }
		}
        yield return StartCoroutine(FinishStoryMode());
    }

    private IEnumerator StartStoryMode() {
        currentWave = WaveName.Intro;
        yield return StartCoroutine(myWaveUI.AnimateStoryStart());
    }

    private IEnumerator FinishStoryMode() {
        Debug.Log("Play Victory noises and stuff");
        currentWave = WaveName.Complete;
        ScoreSheet.Reporter.ReportScores();
        yield return StartCoroutine(myWaveUI.AnimateStoryEnd());

        SceneManager.LoadScene(Scenes.Menu);
    }
    #endregion

    private void RunEndlessWaves() {
        currentWave = WaveName.Endless;
        StartCoroutine(endlessWaveCall.RunWave());
    }
}
