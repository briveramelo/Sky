﻿using System;

[Serializable]
public class EndlessScore : IComparable<EndlessScore> {
	private int score;          public int Score => score;
	private float duration; public float Duration => duration;

	public EndlessScore (int score, float duration){
		this.score = score;
		this.duration = duration;
	}

	//for sorting in an organized/prioritized fashion (sort by score currently)
	public int CompareTo(EndlessScore other){
		if (other==null){
			return 1;
		}
        int pointDif = other.Score - score;

        if (pointDif!=0) {
            return pointDif;
        }
        else {
            return (int)(other.Duration - duration);
        }
	}
}