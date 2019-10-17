using System;

[Serializable]
public class StoryScore : IComparable<StoryScore> {
	private int score;          public int Score => score;
	private WaveName finalWave; public WaveName FinalWave => finalWave;

	public StoryScore (int score, WaveName finalWave){
		this.score = score;
		this.finalWave = finalWave;
	}

	//for sorting in an organized/prioritized fashion (sort by score currently)
	public int CompareTo(StoryScore other){
		if (other==null){
			return 1;
		}
        int waveDiff = (int)other.finalWave - (int)finalWave;

        if (waveDiff!=0) {
            return waveDiff;
        }
        else {
            return other.score - score;
        }
	}
}
