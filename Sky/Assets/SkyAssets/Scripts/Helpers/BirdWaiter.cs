using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System;

public class BirdWaiter{
	protected CounterType counterType;
	protected BirdType[] birdTypes;
	public int numberToWaitFor;
	protected int movingNumber => ScoreSheet.Reporter.GetCounts(counterType,true,birdTypes);
	public bool wait => Wait(movingNumber);

	private delegate bool BoolDelegate(int mover);

	private BoolDelegate Wait;
	public IEnumerator Perform;
	public SpawnDelegate Spawn;

	public BirdWaiter(CounterType counterType, bool invertBirdTypes, int numberToWaitFor, IEnumerator Perform, params BirdType[] birdTypes){
		Initialize(counterType, invertBirdTypes, numberToWaitFor, birdTypes);
		this.Perform = Perform;
	}
	public BirdWaiter(CounterType counterType, bool invertBirdTypes, int numberToWaitFor, SpawnDelegate Spawn, params BirdType[] birdTypes){
		Initialize(counterType, invertBirdTypes, numberToWaitFor, birdTypes);
		this.Spawn = Spawn;
	}
	public BirdWaiter(CounterType counterType, bool invertBirdTypes, int numberToWaitFor, params BirdType[] birdTypes){
		Initialize(counterType, invertBirdTypes, numberToWaitFor, birdTypes);
	}

	private void Initialize(CounterType counterType, bool invertBirdTypes, int numberToWaitFor, params BirdType[] birdTypes){
		this.counterType = counterType;
		this.birdTypes = invertBirdTypes ? InvertBirdTypes(birdTypes) : birdTypes;
		this.numberToWaitFor = numberToWaitFor;
		Wait = mover => mover>this.numberToWaitFor;
		if (counterType == CounterType.Spawned || counterType == CounterType.Killed){
			this.numberToWaitFor +=ScoreSheet.Reporter.GetCounts(counterType,true, birdTypes);
			Wait = mover => mover < this.numberToWaitFor;
		} 
	}

	private static BirdType[] InvertBirdTypes(params BirdType[] birdTypes){
		List<BirdType> birdsToWaitFor = Enum.GetValues(typeof(BirdType)).Cast<BirdType>().ToList();
		birdsToWaitFor.Remove(BirdType.All);
		foreach (BirdType birdType in birdTypes){
			birdsToWaitFor.Remove(birdType);
		}
		return birdsToWaitFor.ToArray();
	}
}