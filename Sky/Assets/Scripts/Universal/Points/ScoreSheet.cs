using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

#region Interfaces
public interface ITallyable {
	void TallyBirth(BirdStats birdStats);
	void TallyDeath(BirdStats birdStats);
	void TallyKill(BirdStats birdStats);
	void TallyPoints(BirdStats birdStats);
}
public interface IResetable{
	void ResetWaveCounters();
}
public interface IReportable{
	int GetCount(CounterType counter, BirdType birdType);
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

	[SerializeField] private GameObject points; 
	[SerializeField] private ScoreBoard scoreBoard;

	#region BirdCounters
	class Counter: IComparable<Counter>{
		public Counter(CounterType counterType, List<Counter> allCounters){
			this.counterType = counterType;
			allCounters.Add(this);
		}
		public int CompareTo(Counter other){
			return this.counterType.CompareTo(other.counterType);
		}

		protected CounterType counterType;
		protected int[] currentCount = new int[Enum.GetNames(typeof(BirdType)).Length];
		protected int[] cummulativeCount = new int[Enum.GetNames(typeof(BirdType)).Length];

		public void SetCount(BirdType birdType, int change) {
			currentCount[(int)birdType] += change;
			currentCount[(int)BirdType.All] += change;
			cummulativeCount[(int)birdType] += change;
			cummulativeCount[(int)BirdType.All] += change;
		}
		public int GetCount(BirdType birdType, bool currentWave){
			int countToReturn = currentWave ? currentCount[(int)birdType] : cummulativeCount[(int)birdType];
			return countToReturn;
		}
		public void ResetCurrentCount(){
			currentCount = new int[Enum.GetNames(typeof(BirdType)).Length];
		}
	}

	class BirdCounter : Counter{
		public BirdCounter(CounterType counterType, List<Counter> allCounters) : base(counterType, allCounters){}
		public void SetCount(BirdType birdType, bool increment) {
			int change = increment ? 1 : -1;
			base.SetCount(birdType, change);
		}
	}

	static List<Counter> allCounters = new List<Counter>();
	private BirdCounter birdsSpawned, birdsAlive, birdsKilled;
	private Counter birdsScored;
	#endregion

	void Awake(){
		Instance = this;
		Tallier = (ITallyable)this;
		Resetter = (IResetable)this;
		Reporter = (IReportable)this;
		Streaker = (IStreakable)this;
		birdsSpawned = new BirdCounter(CounterType.Spawned,allCounters);
		birdsAlive = new BirdCounter(CounterType.Alive,allCounters);
		birdsScored = new BirdCounter(CounterType.Scored,allCounters);
		birdsKilled = new BirdCounter(CounterType.Killed,allCounters);
		allCounters.Sort();
	}

	#region IResetable
	void IResetable.ResetWaveCounters(){
		for (int i=0; i<allCounters.Count; i++){
			allCounters[i].ResetCurrentCount();
		}
	}
	#endregion

	#region IReportable
	int IReportable.GetCount(CounterType counter, BirdType birdType){
		return allCounters[(int)counter].GetCount(birdType, true);
	}
	#endregion

	#region ITallyable
	void ITallyable.TallyBirth(BirdStats birdStats){
		birdsSpawned.SetCount(birdStats.MyBirdType, true);
		birdsAlive.SetCount(birdStats.MyBirdType, true);
	}

	void ITallyable.TallyDeath(BirdStats birdStats){
		birdsAlive.SetCount(birdStats.MyBirdType, false);
	}

	void ITallyable.TallyKill(BirdStats birdStats){
		birdsKilled.SetCount(birdStats.MyBirdType, true);
	}

	void ITallyable.TallyPoints(BirdStats birdStats){
		//special case for killing birds of point multipliers
		int totalFromMultiplier = 0;
		if (birdStats.Health<=0){
			if (birdStats.MyBirdType == BirdType.Seagull || birdStats.MyBirdType == BirdType.Tentacles){ 
				foreach (Bird bird in FindObjectsOfType<Bird>()){
					totalFromMultiplier += Mathf.CeilToInt(bird.MyBirdStats.TotalPointValue * birdStats.KillPointMultiplier);
				}
			}
		}
		int pointsToAdd = totalFromMultiplier + birdStats.Health<=0 ? birdStats.KillPointValue : birdStats.DamagePointValue;

		birdsScored.SetCount (birdStats.MyBirdType, pointsToAdd);
		(Instantiate(points, birdStats.birdPosition, Quaternion.identity) as GameObject).GetComponent<IDisplayable>().DisplayPoints(pointsToAdd);
		((IDisplayable)scoreBoard).DisplayPoints(birdsScored.GetCount(BirdType.All, false));
	}
	#endregion
}