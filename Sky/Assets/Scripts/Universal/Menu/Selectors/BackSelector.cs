using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class BackSelector : Selector {

    protected override Vector2 TouchSpot { get { return MenuInputHandler.touchSpot; } }

    protected override IEnumerator PressButton() {
        AudioManager.PlayAudio(buttonPress);
        SceneManager.LoadScene((int)Scenes.Menu);
        yield return null;
    }
}
