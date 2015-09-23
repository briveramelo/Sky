using UnityEngine;
using System.Collections; 

namespace GenericFunctions{

	public static class Spawn {

		public static IEnumerator NewBalloon(){
			GameObject newBal = MonoBehaviour.Instantiate (Resources.Load (Constants.balloonPrefabNames[Random.Range (0,Constants.balloonPrefabNames.Length)]),new Vector3 (Random.Range(-Constants.worldDimensions.x * 0.67f,Constants.worldDimensions.x * 0.67f),-Constants.worldDimensions.y*1.6f,0f),Quaternion.identity) as GameObject;
			yield return null;
		}

	}

}
