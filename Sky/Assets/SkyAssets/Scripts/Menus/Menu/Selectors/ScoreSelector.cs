using UnityEngine;
using System.Collections;

public class ScoreSelector : Selector
{
    [SerializeField] private GameMode _myGameMode;
    [SerializeField] private ScoreDisplayer _theScoreDisplayer;

    protected override void OnClick()
    {
        AudioManager.PlayAudio(_buttonPress);
        _theScoreDisplayer.DisplayStats(_myGameMode);
    }
}