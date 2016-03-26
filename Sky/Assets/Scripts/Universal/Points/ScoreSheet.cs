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
	int GetCounts(CounterType counter, params BirdType[] birdTypes);
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

	static Dictionary<CounterType, Counter> allCounters = new Dictionary<CounterType, Counter>();
	#endregion
	const bool increase = true;
	const bool decrease = false;
	void Awake(){
		Instance = this;
		Tallier = (ITallyable)this;
		Resetter = (IResetable)this;
		Reporter = (IReportable)this;
		Streaker = (IStreakable)this;

		for (int i=0; i<Enum.GetNames(typeof(CounterType)).Length; i++){
			if ((CounterType)i==CounterType.Scored){
				allCounters.Add((CounterType)i, new PointCounter((CounterType)i));
			}
			else{
				allCounters.Add((CounterType)i, new BirdCounter((CounterType)i));
			}
		}
	}

	#region IResetable
	void IResetable.ResetWaveCounters(){
		for (int i=0; i<allCounters.Count; i++){
			allCounters[(CounterType)i].ResetCurrentCount();
		}
	}
	#endregion

	#region IReportable
	int IReportable.GetCount(CounterType counter, BirdType birdType){
		return allCounters[counter].GetCount(birdType, true);
	}
	int IReportable.GetCounts(CounterType counter, params BirdType[] birdTypes){
		int total=0;
		for (int i=0; i<birdTypes.Length; i++){
			total+= allCounters[counter].GetCount(birdTypes[i], true);
		} 
		return total;
	}
	#endregion

	#region ITallyable
	void ITallyable.TallyBirth(BirdStats birdStats){
		((BirdCounter)(allCounters[CounterType.Spawned])).SetCount(birdStats.MyBirdType, increase);
		((BirdCounter)(allCounters[CounterType.Alive])).SetCount(birdStats.MyBirdType, increase);
	}

	void ITallyable.TallyDeath(BirdStats birdStats){
		((BirdCounter)(allCounters[CounterType.Alive])).SetCount(birdStats.MyBirdType, decrease);
	}

	void ITallyable.TallyKill(BirdStats birdStats){
		((BirdCounter)(allCounters[CounterType.Killed])).SetCount(birdStats.MyBirdType, increase);
	}

	void ITallyable.TallyPoints(BirdStats birdStats){
		int pointsToAdd = birdStats.Health<=0 ? birdStats.KillPointValue : birdStats.DamagePointValue;
		((PointCounter)(allCounters[CounterType.Scored])).SetCount (birdStats.MyBirdType, pointsToAdd);
		(Instantiate(points, birdStats.birdPosition, Quaternion.identity) as GameObject).GetComponent<IDisplayable>().DisplayPoints(pointsToAdd);
		((IDisplayable)scoreBoard).DisplayPoints(((PointCounter)(allCounters[CounterType.Scored])).GetCount(BirdType.All, false));
	}
	#endregion
}