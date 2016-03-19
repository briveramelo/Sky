using UnityEngine;
using System.Collections;

public struct BirdStats {
	private BirdType myBirdType;		public BirdType MyBirdType{get{return myBirdType;}}
	private int health;					public int Health{get{return health;} set{health = value;}}

	private int killPointValue;			public int KillPointValue{get{return killPointValue;} set{killPointValue = value;}}
	private int damagePointValue;		public int DamagePointValue{get{return damagePointValue;}}
	private float killPointMultiplier;	public float KillPointMultiplier{get{return killPointMultiplier;}}
	private int damageGutValue;			public int DamageGutValue{get{return damageGutValue;}}
	private int killGutValue;			public int KillGutValue{get{return killGutValue;}}
	public Vector3 birdPosition;

	public int TotalPointValue{
		get{return (health * damagePointValue + killPointValue);}
	}

	public void ModifyForStreak(int birdStreak){
		killPointValue += birdStreak-1;
		damagePointValue += birdStreak-1;
	}
	public void ModifyForCombo(int birdsHit){
		killPointValue *= birdsHit;
		damagePointValue *= birdsHit;
	}

	public BirdStats(BirdType birdType){
		myBirdType = birdType;
		health = 1;

		killPointValue =0;
		damagePointValue=0;
		killPointMultiplier=0;
		damageGutValue=1;
		killGutValue=1;

		birdPosition = Vector3.zero;

		switch(birdType){
		case BirdType.Pigeon:
			killGutValue = 3;
			killPointValue = 1;
			break;
		case BirdType.Duck:
			killGutValue = 4;
			killPointValue = 1; // set to 3 if going solo or upon breaking formation
			break;
		case BirdType.DuckLeader:
			killGutValue = 7;
			killPointValue = 2;
			break;
		case BirdType.Albatross:
			killGutValue = 20;
			killPointValue = 4;

			health = 7;
			damageGutValue = 4;
			damagePointValue = 1;
			break;
		case BirdType.BabyCrow:
			killGutValue = 2;
			killPointValue = 0;
			break;
		case BirdType.Crow:
			killGutValue = 5;
			killPointValue = 2;
			break;
		case BirdType.Seagull:
			killGutValue = 4;
			killPointMultiplier = 2;
			break;
		case BirdType.Tentacles:
			killGutValue = 80;
			killPointValue = 10;

			health = 25;
			damageGutValue = 4;
			damagePointValue= 1;
			killPointMultiplier = 1.5f;
			break;
		case BirdType.Pelican:
			killGutValue = 15;
			killPointValue = 3;

			health = 3;
			damageGutValue = 4;
			damagePointValue = 2;
			break;
		case BirdType.Bat:
			killGutValue = 3;
			killPointValue = 1;
			break;
		case BirdType.Eagle:
			killGutValue = 80;
			killPointValue = 20;

			health = 5;
			damageGutValue = 4;
			damagePointValue = 3;
			break;
		case BirdType.BirdOfParadise:
			killGutValue = 40;
			killPointValue = 10;
			break;
		}
	}
}