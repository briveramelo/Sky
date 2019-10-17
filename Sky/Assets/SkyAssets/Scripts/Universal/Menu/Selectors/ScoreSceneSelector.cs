using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ScoreSceneSelector : Selector {

    protected override Vector2 TouchSpot => MenuInputHandler.touchSpot;

    protected override IEnumerator PressButton() {
        AudioManager.PlayAudio(buttonPress);
        yield return null;
        SceneManager.LoadScene(Scenes.Scores);
    }
}
