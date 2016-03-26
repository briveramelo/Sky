using UnityEngine;
using System.Collections;
using GenericFunctions;
using System.Collections.Generic;
using System;

public class Pigeon_Wave : Wave {

	protected override IEnumerator RunWave(){
		float[] bottomHeights= new float[]{lowHeight,lowHeight,medHeight};
		float[] topHeights= new float[]{medHeight,highHeight,highHeight};

		PigeonDelegate[] SpawnPigeons = new PigeonDelegate[]{
			AtHeight(heights),
			AtHeight(bottomHeights) + AtHeight(topHeights)
		};
		for (int j=0; j<SpawnPigeons.Length; j++){
			for (int i=0; i<heights.Length; i++){
				yield return StartCoroutine (Produce1Wait3(()=> SpawnPigeons[j](i)));
			}
		}
				
		yield return StartCoroutine (base.RunWave());
	}

	delegate void PigeonDelegate(int i);
	PigeonDelegate AtHeight(float[] myHeights){
		return (int i)=>{
			SpawnBirds(BirdType.Pigeon,SpawnPoint(right,myHeights[i]));
		};
	}
}