﻿using UnityEngine;
using System.Collections;
using GenericFunctions;

public class BirdStats {
	private BirdType myBirdType;		public BirdType MyBirdType{get{return myBirdType;}}
	private int health;					public int Health{get{return health;} set{health = value;}}


	private int killPointValueBase;		public int KillPointValueBase{get{return killPointValueBase;} set{killPointValueBase = value;}}
	private int damagePointValueBase;	public int DamagePointValueBase{get{return damagePointValueBase;}}
	private int killPointValue;			public int KillPointValue{get{return killPointValue;} set{killPointValue = value;}}
	private int damagePointValue;		public int DamagePointValue{get{return damagePointValue;}}

	private float killPointMultiplier;	public float KillPointMultiplier{get{return 10*killPointMultiplier;}}
	private int damageGutValue;			public int DamageGutValue{get{return damageGutValue;}}
	private int killGutValue;			public int KillGutValue{get{return killGutValue;}}
	private Vector2 birdPosition;
	public Vector2 BirdPosition{get{return birdPosition;}
		set{
			birdPosition = value;
			if (Mathf.Abs(value.x)>(Constants.WorldDimensions.x)*.9f){
				birdPosition = new Vector2(Mathf.Sign(birdPosition.x) * Constants.WorldDimensions.x*.9f,birdPosition.y);
			}
			if (Mathf.Abs(value.y)>(Constants.WorldDimensions.y)*.9f){
				birdPosition = new Vector2(birdPosition.x,Mathf.Sign(birdPosition.y) *Constants.WorldDimensions.y * 0.9f);
			}
		}
	}

	public int TotalPointValue{
		get{return ((health-1) * damagePointValueBase + killPointValueBase);}
	}
	public int PointsToAdd{get{return health<=0 ? killPointValue : damagePointValue;}}

	public void ModifyForStreak(int birdStreak){
		killPointValue = killPointValueBase + birdStreak-1;
		damagePointValue = damagePointValueBase + birdStreak-1;
	}
	public void ModifyForCombo(int birdsHit){
		killPointValue *=10*birdsHit;
		damagePointValue *=10*birdsHit;
	}
	public void ModifyForMultiplier(){
		int totalFromMultiplier = 0;
		if (Health<=0){
			if (MyBirdType == BirdType.Seagull || MyBirdType == BirdType.Tentacles){ 
				foreach (Bird bird in MonoBehaviour.FindObjectsOfType<Bird>()){
					if (!(bird.MyBirdStats.MyBirdType==BirdType.Seagull || bird.MyBirdStats.MyBirdType==BirdType.Tentacles)){
						totalFromMultiplier += (bird.MyBirdStats.TotalPointValue * (int)KillPointMultiplier);
					}
				}
			}
		}
		killPointValue += totalFromMultiplier;
	}

	public BirdStats(BirdType birdType){
		myBirdType = birdType;
		health = 1;

		killPointValue =1;
		damagePointValue=1;
		killPointMultiplier=0;
		damageGutValue=1;
		killGutValue=1;

		birdPosition = Vector3.zero;

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
			break;
		case BirdType.BabyCrow:
			killGutValue = 2;
			killPointValue = 0;
			break;
		case BirdType.Crow:
			killGutValue = 5;
			killPointValue = 10;
			break;
		case BirdType.Seagull:
			killGutValue = 4;
			killPointValue = 2;
			killPointMultiplier = 2;
			break;
		case BirdType.Tentacles:
			killGutValue = 80;
			killPointValue = 10;
			health = 25;
			damageGutValue = 4;
			killPointMultiplier = 1.5f;
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
			killGutValue = 80;
			killPointValue = 50;
			health = 5;
			damageGutValue = 4;
			damagePointValue = 5;
			break;
		case BirdType.BirdOfParadise:
			killGutValue = 40;
			killPointValue = 10;
			break;
		}

		killPointValueBase = killPointValue;
		damagePointValueBase = damagePointValue;
	}
}