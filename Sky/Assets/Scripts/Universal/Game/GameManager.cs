﻿using UnityEngine;

public interface IScoreMenu {
    void SetScoreMenu(GameMode ScoreMenu);
}

public class GameManager : MonoBehaviour, IWaveSet, IScoreMenu {

    public static GameManager Instance;
    WaveType MyWaveType;
    static GameMode scoreMenu; public static GameMode ScoreMenu {get { return scoreMenu; } }

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

    void IScoreMenu.SetScoreMenu(GameMode ScoreMenu) {
        scoreMenu = ScoreMenu;
    }

    void OnLevelWasLoaded(int level) {
        if (previousLevel != null) {
            if (level == (int)Scenes.Endless || level == (int)Scenes.Story) {
                waveInterface.RunWaves(MyWaveType);
            }
        }
    }
}
