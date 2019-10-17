using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class QuitSelector : Selector {

    [SerializeField] Pauser pauser;
    protected override Vector2 TouchSpot { get { return InputManager.touchSpot; } }
    [SerializeField] bool shouldSaveScore;

    protected override IEnumerator PressButton() {
        AudioManager.PlayAudio(buttonPress);
        float startTime = Time.unscaledTime;
        while (Time.unscaledTime - startTime < 1f) {
            yield return null;
        }
        pauser.ResetPause();
        if (shouldSaveScore) {
            ScoreSheet.Reporter.ReportScores();
        }
        SceneManager.LoadScene(Scenes.Menu);
        yield return null;
    }
}
