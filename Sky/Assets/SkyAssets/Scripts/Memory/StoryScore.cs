using System;

[Serializable]
public class StoryScore : IComparable<StoryScore> {
	public int Score;
	public WaveName FinalWave;

	public StoryScore (int score, WaveName finalWave){
		Score = score;
		FinalWave = finalWave;
	}

	//for sorting in an organized/prioritized fashion (sort by score currently)
	public int CompareTo(StoryScore other){
		if (other==null){
			return 1;
		}
        int waveDiff = (int)other.FinalWave - (int)FinalWave;

        if (waveDiff!=0) {
            return waveDiff;
        }
        else {
            return other.Score - Score;
        }
	}
}
