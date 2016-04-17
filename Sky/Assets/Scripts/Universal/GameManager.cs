using UnityEngine;

public class GameManager : MonoBehaviour, IWaveSet {

    public static GameManager Instance;
    WaveType MyWaveType;
    [SerializeField] WaveManager waveManager;
    IRunWaves waveInterface;
    int? previousLevel;

    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            waveInterface = (IRunWaves)waveManager;
            previousLevel = (int)Scenes.Menu;
        }
        else {
            Destroy(gameObject);
        }
    }

    void IWaveSet.SetWaveType(WaveType MyWaveType) {
        this.MyWaveType = MyWaveType;
    }

    void OnLevelWasLoaded(int level) {
        if (previousLevel != null) {
            if (level == (int)Scenes.Endless || level == (int)Scenes.Story) {
                Debug.Log(waveInterface);
                waveInterface.RunWaves(MyWaveType);
            }
        }
    }
}
