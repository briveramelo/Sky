using UnityEngine;
using System.Collections;

public class ScoreSelector : Selector{

    [SerializeField] private GameMode MyGameMode;
    [SerializeField] private ScoreDisplayer theScoreDisplayer;

    protected override Vector2 TouchSpot => MenuInputHandler.touchSpot;

    protected override IEnumerator PressButton() {
        AudioManager.PlayAudio(buttonPress);
        theScoreDisplayer.DisplayStats(MyGameMode);
        yield return null;
    }
}
