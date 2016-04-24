using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

#region Interfaces
public interface ITallyable {
	void TallyBirth(ref BirdStats birdStats);
	void TallyDeath(ref BirdStats birdStats);
	void TallyKill(ref BirdStats birdStats);
	void TallyPoints(ref BirdStats birdStats);
    void TallyThreat(Threat MyThreat);
    void TallyBirdThreat(ref BirdStats birdStats, BirdThreat MyThreat);
}
public interface IResetable{
	void ResetWaveCounters();
}
public interface IReportable{
	int GetCount(CounterType counter, bool currentWave, BirdType birdType);
	int GetCounts(CounterType counter, bool currentWave, params BirdType[] birdTypes);
    int GetScore(ScoreType scoreType, bool currentWave, BirdType birdType);
    void ReportScores();
}
public interface IStreakable{
	void ReportHit(int spearNumber);
	int GetHitStreak();
}
#endregion

public enum CounterType{
	Spawned=0,
	Alive=	1,
	Scored =2,
	Killed =3,
}

public enum ScoreType {
    Total=0,
    Streak=1,
    Combo=2
}

public class ScoreSheet : MonoBehaviour, ITallyable, IResetable, IReportable, IStreakable {

	#region IStreakable
	static int hitStreak;
	static int tempStreak;
	static int lastSpearNumber;
	void IStreakable.ReportHit(int spearNumber){
		int spearNumberDif= spearNumber-lastSpearNumber;

		if (spearNumberDif==0 || spearNumberDif==1){
			//continue the streeeeeaaaakkkk!
			hitStreak++;
			if (spearNumberDif==1){
				tempStreak = 1;
			}
		}
		else if (spearNumberDif>1){
			//c-c-c-combo breaker
			tempStreak = hitStreak;
			hitStreak = 1;
		}
		else if (spearNumberDif<0){
			//Combo RESTORATION!
			hitStreak = tempStreak+1;
			//rectify points
		}
		lastSpearNumber = spearNumber;
	}
	int IStreakable.GetHitStreak(){
		return hitStreak;
	}
	#endregion

	public static ScoreSheet Instance;
	public static ITallyable Tallier;
	public static IResetable Resetter;
	public static IReportable Reporter;
	public static IStreakable Streaker;

	[SerializeField] GameObject points; 
	[SerializeField] ScoreBoard scoreBoard;

	#region BirdCounters
	class Counter{
		protected CounterType counterType;
		protected int[] currentCount = new int[Enum.GetNames(typeof(BirdType)).Length];
		protected int[] cummulativeCount = new int[Enum.GetNames(typeof(BirdType)).Length];

		public Counter(CounterType counterType){
			this.counterType = counterType;
		}

		public void SetCount(BirdType birdType, int change) {
			currentCount[(int)birdType] += change;
			currentCount[(int)BirdType.All] += change;
			cummulativeCount[(int)birdType] += change;
			cummulativeCount[(int)BirdType.All] += change;
		}
		public int GetCount(BirdType birdType, bool currentWave){
			return currentWave ? currentCount[(int)birdType] : cummulativeCount[(int)birdType];
		}
		public void ResetCurrentCount(){
			currentCount = new int[Enum.GetNames(typeof(BirdType)).Length];
		}
	}

	class BirdCounter : Counter{
		public BirdCounter(CounterType counterType) : base(counterType){}
		public void SetCount(BirdType birdType, bool increase) {
			int change = increase ? 1 : -1;
			base.SetCount(birdType, change);
		}
	}
	class PointCounter : Counter{
		public PointCounter(CounterType counterType) : base(counterType){}
	}

    static Dictionary<CounterType, Counter> allCounters;
    static Dictionary<ScoreType, PointCounter> scoreCounters;
	#endregion
	const bool increase = true;
	const bool decrease = false;
    decimal startTime;
	void Awake(){
        Instance = this;
        scoreBoard = FindObjectOfType<ScoreBoard>();

		Tallier = (ITallyable)this;
		Resetter = (IResetable)this;
		Reporter = (IReportable)this;
		Streaker = (IStreakable)this;

        allCounters = new Dictionary<CounterType, Counter>();
		for (int i=0; i<Enum.GetNames(typeof(CounterType)).Length; i++){
			if ((CounterType)i==CounterType.Scored){
				allCounters.Add((CounterType)i, new PointCounter(CounterType.Scored));
			}
			else{
				allCounters.Add((CounterType)i, new BirdCounter((CounterType)i));
			}
		}
        scoreCounters = new Dictionary<ScoreType, PointCounter>();
        for (int i=0; i<Enum.GetNames(typeof(ScoreType)).Length; i++) {
            scoreCounters.Add((ScoreType)i, new PointCounter(CounterType.Scored));
        }
        startTime = (decimal)Time.time;
	}

	#region IResetable
	void IResetable.ResetWaveCounters(){
		for (int i=0; i<allCounters.Count; i++){
			allCounters[(CounterType)i].ResetCurrentCount();
		}
	}
	#endregion

	#region IReportable
	int IReportable.GetCount(CounterType counter, bool currentWave, BirdType birdType){
		return allCounters[counter].GetCount(birdType, currentWave);
	}
	int IReportable.GetCounts(CounterType counter, bool currentWave, params BirdType[] birdTypes){
		int total=0;
		for (int i=0; i<birdTypes.Length; i++){
			total+= allCounters[counter].GetCount(birdTypes[i], currentWave);
		} 
		return total;
	}
    int IReportable.GetScore(ScoreType scoreType, bool currentWave, BirdType birdType){
		return scoreCounters[scoreType].GetCount(birdType, currentWave);
    }
    void IReportable.ReportScores() {
        if (WaveManager.CurrentWave == WaveName.Endless) {
            decimal duration = (decimal)Time.time - startTime;
            EndlessScore MyEndlessScore = new EndlessScore(scoreCounters[ScoreType.Total].GetCount(BirdType.All, false), duration);
            FindObjectOfType<SaveLoadData>().PromptSave(MyEndlessScore);
        }
        else {
            StoryScore MyStoryScore = new StoryScore(scoreCounters[ScoreType.Total].GetCount(BirdType.All,false), WaveManager.CurrentWave);
            FindObjectOfType<SaveLoadData>().PromptSave(MyStoryScore);
        }
    }
	#endregion

	#region ITallyable
	void ITallyable.TallyBirth(ref BirdStats birdStats){
		((BirdCounter)(allCounters[CounterType.Spawned])).SetCount(birdStats.MyBirdType, increase);
		((BirdCounter)(allCounters[CounterType.Alive])).SetCount(birdStats.MyBirdType, increase);
	}

	void ITallyable.TallyDeath(ref BirdStats birdStats){
		((BirdCounter)(allCounters[CounterType.Alive])).SetCount(birdStats.MyBirdType, decrease);
	}

	void ITallyable.TallyKill(ref BirdStats birdStats){
		((BirdCounter)(allCounters[CounterType.Killed])).SetCount(birdStats.MyBirdType, increase);
	}

	void ITallyable.TallyPoints(ref BirdStats birdStats){
		((PointCounter)(allCounters[CounterType.Scored])).SetCount (birdStats.MyBirdType, birdStats.PointsToAdd);
        scoreCounters[ScoreType.Total].SetCount(birdStats.MyBirdType, birdStats.PointsToAdd);
        scoreCounters[ScoreType.Streak].SetCount(birdStats.MyBirdType, birdStats.StreakPoints);
        scoreCounters[ScoreType.Combo].SetCount(birdStats.MyBirdType, birdStats.ComboPoints);

		(Instantiate(points, birdStats.BirdPosition, Quaternion.identity) as GameObject).GetComponent<IDisplayable>().DisplayPoints(birdStats.PointsToAdd);
		((IDisplayable)scoreBoard).DisplayPoints(((PointCounter)(allCounters[CounterType.Scored])).GetCount(BirdType.All, false));
	}

    void ITallyable.TallyThreat(Threat MyThreat) {
        EmotionalIntensity.ThreatTracker.RaiseThreat(MyThreat);
    }
    void ITallyable.TallyBirdThreat(ref BirdStats birdStats, BirdThreat MyThreat)
    {
        if (EmotionalIntensity.ThreateningBirds.Contains(birdStats.MyBirdType))
        {
            EmotionalIntensity.ThreatTracker.BirdThreat(ref birdStats, MyThreat);
        }
    }
    #endregion
}