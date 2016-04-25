using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public enum GameMode {
    Story =0,
    Endless =1
}

public class ScoreSelector : Selector{

    [SerializeField] GameMode MyScoreScene;

    protected override Vector2 TouchSpot {get { return MenuInputHandler.touchSpot; } }

    protected override IEnumerator PressButton() {
        buttonNoise.PlayOneShot(buttonPress);
        yield return null;
        ((IScoreMenu)(GameManager.Instance)).SetScoreMenu(MyScoreScene);
        SceneManager.LoadScene((int)Scenes.Scores);
    }
}
