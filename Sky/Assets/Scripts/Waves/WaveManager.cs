using UnityEngine;
using System.Collections;

public enum WaveType {
    Story = 1,
    Endless = 2
}
public interface IWaveSet {
    void SetWaveType(WaveType MyWaveType);
}
public interface IRunWaves {
    void RunWaves(WaveType MyWaveType);
}

public class WaveManager : MonoBehaviour, IRunWaves {

	[SerializeField] private Wave[] storyWaves;
	private IWaveRunnable[] storyWaveCalls;
    [SerializeField] private Wave endlessWave;
	private IWaveRunnable endlessWaveCall;

    void OnLevelWasLoaded(int level) {
        if (level == (int)Scenes.Menu) {
            StopAllCoroutines();
        }
    }

	void Awake(){
        storyWaveCalls = (IWaveRunnable[])storyWaves;
        endlessWaveCall = (IWaveRunnable)endlessWave;
	}

    void IRunWaves.RunWaves(WaveType MyWaveType) {
        StartCoroutine(RunWaves(MyWaveType));
    }

	IEnumerator RunWaves(WaveType MyWaveType){
		yield return null;
        if (MyWaveType == WaveType.Story) {
		    foreach (IWaveRunnable wave in storyWaveCalls){
			    yield return StartCoroutine (wave.RunWave());
		    }
        }
        else {
            StartCoroutine(endlessWaveCall.RunWave());
        }
	}

    
}
