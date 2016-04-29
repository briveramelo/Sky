public class ScoreBoard : PointDisplay {

    void OnLevelWasLoaded(int level) {
        if (level == (int)Scenes.Menu) {
            myText.text = "";
        }
    }

	protected override void DisplayPoints(int points){
		myText.text = points.ToString();
	}
}
