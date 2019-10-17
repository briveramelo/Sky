using UnityEngine.SceneManagement;

public class ScoreBoard : PointDisplay {

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode) {
        if (scene.name == Scenes.Menu) {
            myText.text = "";
        }
    }

	protected override void DisplayPoints(int points){
		myText.text = points.ToString();
	}
}
