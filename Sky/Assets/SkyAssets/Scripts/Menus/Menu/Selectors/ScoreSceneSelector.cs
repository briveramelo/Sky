using System.Collections;
using UnityEngine.SceneManagement;

public class ScoreSceneSelector : Selector
{
    protected override IEnumerator OnClickRoutine()
    {
        yield return null;
        SceneManager.LoadScene(Scenes.Scores);
    }
}