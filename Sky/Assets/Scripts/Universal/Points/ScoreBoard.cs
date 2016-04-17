public class ScoreBoard : PointDisplay {

	protected override void DisplayPoints(int points){
		myText.text = points.ToString();
	}
}
