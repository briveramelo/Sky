using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class QuitSelector : Selector
{
    [SerializeField] private Pauser _pauser;
    protected override Vector2 TouchSpot => InputManager.TouchSpot;
    [SerializeField] private bool _shouldSaveScore;

    protected override IEnumerator PressButton()
    {
        AudioManager.PlayAudio(_buttonPress);
        var startTime = Time.unscaledTime;
        while (Time.unscaledTime - startTime < 1f)
        {
            yield return null;
        }

        _pauser.ResetPause();
        if (_shouldSaveScore)
        {
            ScoreSheet.Reporter.ReportScores();
        }

        SceneManager.LoadScene(Scenes.Menu);
        yield return null;
    }
}