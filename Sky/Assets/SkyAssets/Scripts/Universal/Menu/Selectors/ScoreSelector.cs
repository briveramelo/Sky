using UnityEngine;
using System.Collections;

public class ScoreSelector : Selector{

    [SerializeField] private GameMode _myGameMode;
    [SerializeField] private ScoreDisplayer _theScoreDisplayer;

    protected override Vector2 TouchSpot => MenuInputHandler.TouchSpot;

    protected override IEnumerator PressButton() {
        AudioManager.PlayAudio(_buttonPress);
        _theScoreDisplayer.DisplayStats(_myGameMode);
        yield return null;
    }
}
