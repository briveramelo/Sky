using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public enum WaveType {
    Story = 1,
    Endless = 2
}

public class WaveManager : MonoBehaviour {

    [SerializeField] WaveUI waveUI; IWaveUI myWaveUI;
	[SerializeField] private Wave[] storyWaves;
	private IWaveRunnable[] storyWaveCalls;
    [SerializeField] private Wave endlessWave;
	private IWaveRunnable endlessWaveCall;
    static WaveName currentWave; public static WaveName CurrentWave {get { return currentWave; } }

    void OnLevelWasLoaded(int level) {
        ChooseMode((Scenes)level);
    }

	void Awake(){
        storyWaveCalls = (IWaveRunnable[])storyWaves;
        endlessWaveCall = (IWaveRunnable)endlessWave;
        myWaveUI = (IWaveUI)waveUI;
        StopAllCoroutines();
        ChooseMode((Scenes)SceneManager.GetActiveScene().buildIndex);
	}

    void ChooseMode(Scenes loadedScene) {
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

    IEnumerator RunStoryWaves() {
		foreach (IWaveRunnable wave in storyWaveCalls){
            currentWave = wave.MyWave;
            yield return StartCoroutine (wave.RunWave());
		}
        yield return StartCoroutine(FinishStoryMode());
    }

    void RunEndlessWaves() {
        currentWave = WaveName.Endless;
        StartCoroutine(endlessWaveCall.RunWave());
    }

    IEnumerator FinishStoryMode() {
        //play victory noise and other celebratory things
        currentWave = WaveName.Complete;
        ScoreSheet.Reporter.ReportScores();
        yield return StartCoroutine(myWaveUI.AnimateStoryEnd());

        SceneManager.LoadScene((int)Scenes.Menu);
    }
}
