using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public enum GameMode {
    Story =0,
    Endless =1
}

public class ScoreSelector : Selector{

    [SerializeField] GameMode MyScoreScene;

    protected override void Awake() {
        buttonRadius = .7f;
        base.Awake();
    }

    protected override IEnumerator PressButton() {
        buttonAnimator.SetInteger("AnimState", (int)ButtonState.Pressed);
        buttonNoise.PlayOneShot(buttonPress);
        inputManager.IsFrozen = true;
        yield return null;
        ((IScoreMenu)(GameManager.Instance)).SetScoreMenu(MyScoreScene);
        SceneManager.LoadScene((int)Scenes.Scores);
    }
}
