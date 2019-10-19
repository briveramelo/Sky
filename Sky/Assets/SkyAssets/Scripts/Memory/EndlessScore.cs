using System;

[Serializable]
public class EndlessScore : IComparable<EndlessScore> {
	public int Score;
	public float Duration;

	public EndlessScore (int score, float duration){
		Score = score;
		Duration = duration;
	}

	//for sorting in an organized/prioritized fashion (sort by score currently)
	public int CompareTo(EndlessScore other){
		if (other==null){
			return 1;
		}
        int pointDif = other.Score - Score;

        if (pointDif!=0) {
            return pointDif;
        }
        else {
            return (int)(other.Duration - Duration);
        }
	}
}