using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class BackSelector : Selector {

    protected override Vector2 TouchSpot => MenuInputHandler.touchSpot;

    protected override IEnumerator PressButton() {
        AudioManager.PlayAudio(buttonPress);
        SceneManager.LoadScene(Scenes.Menu);
        yield return null;
    }
}
