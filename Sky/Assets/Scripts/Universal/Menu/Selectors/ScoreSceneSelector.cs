using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ScoreSceneSelector : Selector {

    protected override Vector2 TouchSpot {get { return MenuInputHandler.touchSpot; } }

    protected override IEnumerator PressButton() {
        buttonNoise.PlayOneShot(buttonPress);
        yield return null;
        SceneManager.LoadScene((int)Scenes.Scores);
    }
}
