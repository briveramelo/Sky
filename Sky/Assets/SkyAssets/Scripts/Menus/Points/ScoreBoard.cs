using UnityEngine.SceneManagement;

public class ScoreBoard : PointDisplay
{
    private void Awake()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
        DisplayPoints(0);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        var showScore = Scenes.IsGameplay(scene.name);
        if (!showScore)
        {
            _myText.text = "";
        }
        ToggleDisplay(showScore);
    }

    protected override void DisplayPoints(int points)
    {
        _myText.text = points.ToString();
    }
}