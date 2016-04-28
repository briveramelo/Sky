﻿using UnityEngine;
using GenericFunctions;

public class BirdStats {

	private BirdType myBirdType;		public BirdType MyBirdType{get{return myBirdType;}}
	private int health;					public int Health{get{return health;} set{health = value;}}
    public int DamageTaken;

    int killPointValueBase;
    public int KillPointValueBase{get{return killPointValueBase;}
        set {
            killPointValueBase = killPointValue = value;
        }
    }
    int streakPoints; public int StreakPoints { get { return streakPoints * basePointMultiplier; } }
    int comboPoints; public int ComboPoints { get { return comboPoints * basePointMultiplier; } }

	readonly int damagePointValueBase;	public int DamagePointValueBase{get{return damagePointValueBase;}}
	private int killPointValue;			public int KillPointValue{get{return killPointValue;}}
	private int damagePointValue;		public int DamagePointValue{get{return damagePointValue;}}

	private int damageGutValue;			public int DamageGutValue{get{return damageGutValue;}}
	private int killGutValue;			public int KillGutValue{get{return killGutValue;}}
	private Vector2 birdPosition;   	public Vector2 BirdPosition{get{return birdPosition;} set { birdPosition = value; } }

	const int basePointMultiplier =10;
	public int TotalThreatValue{
		get{return ( Mathf.Clamp((health-1),0,100) * damagePointValueBase + killPointValueBase);}
	}
    public int ThreatRemoved{
        get {
            return health > 0 ? DamageTaken * damagePointValueBase : ((DamageTaken-1) * damagePointValueBase + killPointValueBase);
        }
    }

    public int TotalPointValue{
		get{return ( basePointMultiplier * ((health-1) * damagePointValueBase + killPointValueBase));}
	}
	public int PointsToAdd{
		get{return basePointMultiplier * (health<=0 ? killPointValue : damagePointValue);}
	}
	public int GutsToSpill{
		get{return health>0 ? damageGutValue : killGutValue;}
	}

	public void ModifyForStreak(int birdStreak){
        streakPoints = birdStreak-1;
        killPointValue = killPointValueBase + streakPoints;
		damagePointValue = damagePointValueBase + streakPoints;
	}
	public void ModifyForCombo(int birdsHit){
        comboPoints = health<=0 ? killPointValue : damagePointValue;
        killPointValue *=birdsHit;
		damagePointValue *=birdsHit;
        comboPoints = (health<=0 ? killPointValue : damagePointValue) - comboPoints;
	}

	public BirdStats(BirdType birdType){
		myBirdType = birdType;
		health = 1;
		killPointValue =1;
		damagePointValue=1;
		damageGutValue=1;
		killGutValue=1;

		switch(birdType){
		case BirdType.Pigeon:
			killGutValue = 3;
			break;
		case BirdType.Duck:
			killGutValue = 4;
			//killPointValue set to 3 if going solo or upon breaking formation
			break;
		case BirdType.DuckLeader:
			killGutValue = 7;
			killPointValue = 2;
			break;
		case BirdType.Albatross:
			killGutValue = 25;
			killPointValue = 20;
			health = 7;
			damageGutValue = 4;
			damagePointValue =2;
			break;
		case BirdType.BabyCrow:
			killGutValue = 1;
			killPointValue = 0;
			break;
		case BirdType.Crow:
			killGutValue = 5;
			killPointValue = 10;
			break;
		case BirdType.Seagull:
			killGutValue = 4;
			killPointValue = 2;
			break;
		case BirdType.Tentacles:
			killGutValue = 80;
			killPointValue = 10;
			health = 25;
			damageGutValue = 4;
			break;
		case BirdType.Pelican:
			killGutValue = 10;
			killPointValue = 5;
			health = 2;
			damageGutValue = 4;
			damagePointValue = 2;
			break;
		case BirdType.Shoebill:
			killGutValue = 6;
			killPointValue = 3;
			break;
		case BirdType.Bat:
			killGutValue = 3;
			killPointValue = 2;
			break;
		case BirdType.Eagle:
			killGutValue = 160;
			killPointValue = 100;
			health = 30;
			damageGutValue = 4;
			damagePointValue = 5;
			break;
		case BirdType.BirdOfParadise:
			killGutValue = 40;
			killPointValue = 10;
			break;
		}

		damagePointValueBase = damagePointValue;
		killPointValueBase = killPointValue;
	}
}