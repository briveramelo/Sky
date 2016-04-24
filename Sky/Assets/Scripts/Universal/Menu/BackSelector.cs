using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class BackSelector : Selector {

    protected override void Awake() {
        buttonRadius = .7f;
        base.Awake();
    }

    protected override IEnumerator PressButton() {
        buttonAnimator.SetInteger("AnimState", (int)ButtonState.Pressed);
        buttonNoise.PlayOneShot(buttonPress);
        inputManager.IsFrozen = true;
        yield return null;
        SceneManager.LoadScene((int)Scenes.Menu);
    }
}
