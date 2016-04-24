using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class EndlessScore : IComparable<EndlessScore> {

	int score;          public int Score {get { return score; } }
	decimal duration; public decimal Duration {get { return duration; } }

	public EndlessScore (int score, decimal duration){
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