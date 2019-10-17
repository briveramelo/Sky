using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public enum GameMode {
    Story =0,
    Endless =1
}

public class ScoreDisplayer : MonoBehaviour {

    [SerializeField] private Text Title, Column1_Title, Column2_Title;
    [SerializeField] private Text[] column1, column2;

    private DataSave currentDataSave;

    private void Awake() {
        currentDataSave = FindObjectOfType<SaveLoadData>().CopyCurrentDataSave();
        DisplayStats(GameMode.Story);
    }

    public void DisplayStats(GameMode MyGameMode) {
        Title.text = MyGameMode.ToString().ToUpper() + " HIGH SCORES";
        Column1_Title.text = MyGameMode == GameMode.Story ? "Final Wave" : "Score";
        Column2_Title.text = MyGameMode == GameMode.Story ? "Score" : "Time";

        switch (MyGameMode) {
            case GameMode.Story:
                DisplayScores(ref currentDataSave.storyScores);
                break;
            case GameMode.Endless:
                DisplayScores(ref currentDataSave.endlessScores);
                break;
        }
    }

    private void DisplayScores(ref List<StoryScore> MyScores) {
        for (int i=0; i<MyScores.Count; i++) {
            column1[i].text = MyScores[i].FinalWave.ToString();
            column2[i].text = MyScores[i].Score.ToString();
        }
        EmptyRemaining(MyScores.Count);
    }

    private void DisplayScores(ref List<EndlessScore> MyScores) {
        for (int i=0; i<MyScores.Count; i++) {
            column1[i].text = MyScores[i].Score.ToString();
            column2[i].text = ((int)MyScores[i].Duration).ToString() + "s";
        }
        EmptyRemaining(MyScores.Count);
    }

    private void EmptyRemaining(int scoresRecorded) {
        for (int i=scoresRecorded; i<5; i++) {
            column1[i].text = "-";
            column2[i].text = "-";
        }
    }
    
}
