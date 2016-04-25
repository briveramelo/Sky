using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class ScoreDisplayer : MonoBehaviour {

    [SerializeField] Text Title, Column1_Title, Column2_Title;
    [SerializeField] Text[] column1, column2;

    DataSave currentDataSave;

	void Awake() {
        currentDataSave = FindObjectOfType<SaveLoadData>().CopyCurrentDataSave();
        DisplayScene(GameManager.ScoreMenu);
    }

    void DisplayScene(GameMode MyGameMode) {
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

    void DisplayScores(ref List<StoryScore> MyScores) {
        for (int i=0; i<MyScores.Count; i++) {
            column1[i].text = MyScores[i].FinalWave.ToString();
            column2[i].text = MyScores[i].Score.ToString();
        }
    }
    void DisplayScores(ref List<EndlessScore> MyScores) {
        for (int i=0; i<MyScores.Count; i++) {
            column1[i].text = MyScores[i].Score.ToString();
            column2[i].text = ((int)MyScores[i].Duration).ToString() + "s";
        }
    }
    
}
