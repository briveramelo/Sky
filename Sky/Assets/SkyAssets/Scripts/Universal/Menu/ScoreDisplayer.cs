using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public enum GameMode
{
    Story = 0,
    Endless = 1
}

public class ScoreDisplayer : MonoBehaviour
{
    [SerializeField] private Text _title;
    [SerializeField] private Text _column1Title;
    [SerializeField] private Text _column2Title;
    [SerializeField] private Text[] _column1, _column2;

    private DataSave _currentDataSave;

    private void Awake()
    {
        _currentDataSave = FindObjectOfType<SaveLoadData>().CopyCurrentDataSave();
        DisplayStats(GameMode.Story);
    }

    public void DisplayStats(GameMode myGameMode)
    {
        _title.text = myGameMode.ToString().ToUpper() + " HIGH SCORES";
        _column1Title.text = myGameMode == GameMode.Story ? "Final Wave" : "Score";
        _column2Title.text = myGameMode == GameMode.Story ? "Score" : "Time";

        switch (myGameMode)
        {
            case GameMode.Story:
                DisplayScores(ref _currentDataSave.StoryScores);
                break;
            case GameMode.Endless:
                DisplayScores(ref _currentDataSave.EndlessScores);
                break;
        }
    }

    private void DisplayScores(ref List<StoryScore> myScores)
    {
        for (var i = 0; i < myScores.Count; i++)
        {
            _column1[i].text = myScores[i].FinalWave.ToString();
            _column2[i].text = myScores[i].Score.ToString();
        }

        EmptyRemaining(myScores.Count);
    }

    private void DisplayScores(ref List<EndlessScore> myScores)
    {
        for (var i = 0; i < myScores.Count; i++)
        {
            _column1[i].text = myScores[i].Score.ToString();
            _column2[i].text = ((int) myScores[i].Duration).ToString() + "s";
        }

        EmptyRemaining(myScores.Count);
    }

    private void EmptyRemaining(int scoresRecorded)
    {
        for (var i = scoresRecorded; i < 5; i++)
        {
            _column1[i].text = "-";
            _column2[i].text = "-";
        }
    }
}