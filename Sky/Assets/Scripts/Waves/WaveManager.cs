using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class WaveManager : MonoBehaviour {

    [SerializeField] WaveUI waveUI;         IWaveUI myWaveUI;
    [SerializeField] Wave[] storyWaves;	    IWaveRunnable[] storyWaveCalls;
    [SerializeField] Wave endlessWave;      IWaveRunnable endlessWaveCall;
    static WaveName currentWave;            public static WaveName CurrentWave {get { return currentWave; } }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }
    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode) {
        ChooseMode(scene.name);
    }

	void Awake(){
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
        storyWaveCalls = storyWaves;
        endlessWaveCall = endlessWave;
        myWaveUI = waveUI;
        StopAllCoroutines();
        ChooseMode(SceneManager.GetActiveScene().name);
	}

    void ChooseMode(string loadedScene) {
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
    IEnumerator RunStoryWaves() {
        //yield return StartCoroutine(StartStoryMode());
        foreach (IWaveRunnable wave in storyWaveCalls){
            if (wave.MyWave == WaveName.Pigeon) {
                currentWave = wave.MyWave;
                yield return StartCoroutine (wave.RunWave());
            }
		}
        yield return StartCoroutine(FinishStoryMode());
    }

    IEnumerator StartStoryMode() {
        currentWave = WaveName.Intro;
        yield return StartCoroutine(myWaveUI.AnimateStoryStart());
    }
    IEnumerator FinishStoryMode() {
        Debug.Log("Play Victory noises and stuff");
        currentWave = WaveName.Complete;
        ScoreSheet.Reporter.ReportScores();
        yield return StartCoroutine(myWaveUI.AnimateStoryEnd());

        SceneManager.LoadScene(Scenes.Menu);
    }
    #endregion

    void RunEndlessWaves() {
        currentWave = WaveName.Endless;
        StartCoroutine(endlessWaveCall.RunWave());
    }
}
