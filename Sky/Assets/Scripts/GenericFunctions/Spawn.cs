using UnityEngine;
using System.Collections; 

namespace GenericFunctions{

	public static class Spawn {

		public static Vector3[] crowPositions  = new Vector3[]{
			new Vector3(0f   ,  7f , 0f),
			new Vector3(9.5f ,  5.3f , 0f),
			new Vector3(9.5f   , -5.3f , 0f),
			new Vector3(0f   , -7f , 0f),
			new Vector3(-9.5f, -5.3f , 0f),
			new Vector3(-9.5f,  5.3f , 0f),
		};

		public static string[] balloonPrefabNames = new string[]{
			"Prefabs/Gear/PinkBalloon",
			"Prefabs/Gear/TealBalloon",
			"Prefabs/Gear/GreyBalloon"
		};

		public static IEnumerator NewBalloon(){
			GameObject newBal = MonoBehaviour.Instantiate (Resources.Load (balloonPrefabNames[Random.Range (0,3)]),new Vector3 (Random.Range(-6f,6f),-8f,0f),Quaternion.identity) as GameObject;
			yield return null;
		}

	}

}
