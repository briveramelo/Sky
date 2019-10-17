using UnityEngine;
using System.Collections;

public class ScoreSelector : Selector{

    [SerializeField] GameMode MyGameMode;
    [SerializeField] ScoreDisplayer theScoreDisplayer;

    protected override Vector2 TouchSpot {get { return MenuInputHandler.touchSpot; } }

    protected override IEnumerator PressButton() {
        AudioManager.PlayAudio(buttonPress);
        theScoreDisplayer.DisplayStats(MyGameMode);
        yield return null;
    }
}
