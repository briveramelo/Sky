using UnityEngine;
using System.Collections;

public enum Threat{
	BalloonPop = 0,
	Poop = 1,
	BasketThud =2,
	BasketGrab =3,
	BalloonSurround =4,
	BirdSpawn =5
}

public class EmotionalIntensity : MonoBehaviour {

	void Awake(){
		Decay();
	}
	static float intensity; public static float Intensity{get{return intensity;}}

	void Increase(Threat newThreat, int threatValue = 0){
		switch (newThreat){
		case Threat.BalloonPop:
			intensity+= 25;
			break;
		case Threat.Poop:
			intensity+=10;
			break;
		case Threat.BasketThud:
			intensity+=5;
			break;
		case Threat.BasketGrab:
			intensity+=15;
			break;
		case Threat.BalloonSurround:
			intensity+=2;
			break;
		case Threat.BirdSpawn:
			intensity+= threatValue;
			break;
		}
	}

	BirdType[] checkForDecay = new BirdType[]{
		BirdType.Pigeon,
		BirdType.Duck,
		BirdType.DuckLeader,
		BirdType.Albatross,
		//BirdType.BabyCrow,
		BirdType.Crow,
		//BirdType.Seagull,
		BirdType.Tentacles,
		BirdType.Pelican,
		//BirdType.Shoebill,
		BirdType.Bat,
		BirdType.Eagle,
		BirdType.BirdOfParadise
	};

	float repeatTime;
	void Decay(){
		bool decay = intensity>0 && ScoreSheet.Reporter.GetCounts(CounterType.Threat, true, checkForDecay)<=2;
		if (decay){
			intensity -= 1;
			repeatTime=1f;
		}
		else{
			repeatTime =3f;
		}
		Invoke ("Decay",repeatTime);
	}
}
