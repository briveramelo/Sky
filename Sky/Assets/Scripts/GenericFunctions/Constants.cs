using UnityEngine;
using System.Collections;

namespace GenericFunctions{
	public static class Constants{

		public static string tentaclePrefab = "Prefabs/Birds/Tentacles";

		public static string pooNuggetPrefab = "Prefabs/Effects/PooNugget";
		public static string spearPrefab = "Prefabs/Gear/Spear";
		public static string murderPrefab = "Prefabs/Birds/Murder";
		public static string gutSplosionParentPrefab = "Prefabs/Effects/GutSplosionParent";

		public static string[] balloonPrefabNames = new string[]{
			"Prefabs/Gear/PinkBalloon",
			"Prefabs/Gear/TealBalloon",
			"Prefabs/Gear/GreyBalloon"
		};

		public static string[] birdNamePrefabs = new string[]{
			"Prefabs/Birds/Pigeon",			//0
			"Prefabs/Birds/Duck",			//1
			"Prefabs/Birds/DuckLeader",		//2
			"Prefabs/Birds/Albatross",		//3
			"Prefabs/Birds/BabyCrow",		//4
			"Prefabs/Birds/Murder",			//5
			"Prefabs/Birds/Seagull",		//6
			"Prefabs/Birds/TentacleSensor",	//7
			"Prefabs/Birds/Pelican",		//8
			"Prefabs/Birds/Bat_white",		//9
			"Prefabs/Birds/Bat_dark",		//10
			"Prefabs/Birds/Eagle",			//11
			"Prefabs/Birds/BirdOfParadise"	//12
		};

		public static string[] gutSplosionPrefabs = new string[]{
			"Prefabs/Effects/GutSplosion1a", //small birds  //0
			"Prefabs/Effects/GutSplosion2a", //medium birds //1
			"Prefabs/Effects/GutSplosion2b",				 //1
			"Prefabs/Effects/GutSplosion2c",				 //2
			"Prefabs/Effects/GutSplosion2d",				 //3
			"Prefabs/Effects/GutSplosion2e",				 //4
			"Prefabs/Effects/GutSplosion3a", //big birds	 //5
			"Prefabs/Effects/GutSplosion3b",				 //6
		};
		public static string[] pooSplatPrefabs = new string[]{
			"Prefabs/Effects/PooSplat1",
			"Prefabs/Effects/PooSplat7"
		};

		public static void FaceFoward(this Transform trans,bool forward){
			trans.localScale = forward ? Vector3.one : new Vector3 (-1f, 1f, 1f);
		}

		public static void Face4ward(this Transform trans,bool forward){
			trans.localScale = forward ? Vector3.one * 4f : new Vector3 (-4f, 4f, 1f);
		}

		public static float timeToThrowSpear = .66667f;
		public static float timeToStabSpear = .1f;
		public static float timeToDestroySpear = 3f;

		public static Vector2 screenDimensions = new Vector2 (Screen.width, Screen.height);
		//public static Vector2 worldDimensions = new Vector2 (1.8815f, 1.06f);
		//public static Vector2 worldDimensions = new Vector2 (1.8815f, 1.06f);
		//public static Vector2 worldDimensions = new Vector2 (5.68f, 3.2f);
		public static Vector2 worldDimensions = screenDimensions /200f;
		public static Vector2[] worldEdgePoints = new Vector2[]{
			new Vector2 (-worldDimensions.x, worldDimensions.y),
			new Vector2 (worldDimensions.x, worldDimensions.y),
			new Vector2 (worldDimensions.x, -worldDimensions.y),
			new Vector2 (-worldDimensions.x, -worldDimensions.y),
			new Vector2 (-worldDimensions.x, worldDimensions.y)
		};

		public static Vector2 correctionPixels = new Vector2 (Screen.width/2,(-3*Screen.height/2));
		public static float correctionPixelFactor = Constants.worldDimensions.y * 2 / Screen.height;

		public static int defaultLater = 0;
		public static int basketLayer = 13;
		public static int spearLayer = 14;
		public static int joystickLayer = 15;
		public static int birdLayer = 16;
		public static int balloonLayer = 17;
		public static int balloonFloatingLayer = 18;
		public static int tentacleLayer = 19;
		public static int jaiLayer = 20;
		public static int pooFingerLayer = 21;
		public static int pooSmearLayer = 22;
		public static int faceLayer = 23;

		public static int totalPooSpots = 250;

		public static int pigeon = 0;
		public static int duck = 1;
		public static int duckLeader = 2;
		public static int albatross = 3;
		public static int babyCrow = 4;
		public static int crow = 5;
		public static int seagull = 6;
		public static int tentacles = 7;
		public static int pelican = 8;
		public static int bat = 9;
		public static int eagle = 10;
		public static int birdOfParadise = 11;

		public static Vector3[] crowPositions  = new Vector3[]{
			new Vector3(0f  								,  worldDimensions.y * 1.4f , 0f),
			new Vector3(worldDimensions.x * 1.08f ,  worldDimensions.y * 1.2f , 0f),
			new Vector3(worldDimensions.x * 1.08f , -worldDimensions.y * 1.2f , 0f),
			new Vector3(0f  								, -worldDimensions.y * 1.4f , 0f),
			new Vector3(-worldDimensions.x * 1.08f, -worldDimensions.y * 1.2f , 0f),
			new Vector3(-worldDimensions.x * 1.08f,  worldDimensions.y * 1.2f , 0f),
		};

		public static Vector3 stockSpearPosition = new Vector3 (0.14f, 0.12f, 0f);
		public static Vector3 balloonOffset = new Vector3 (0f, 1.6f, 0f);
		public static Vector3 seagullOffset = balloonOffset + Vector3.up * .75f;
		public static float time2Destroy = 2f;

		public static int[] NegativeOnes(int numberOfNegativeOnes){
			int[] negs = new int[numberOfNegativeOnes];
			for (int i = 0; i<numberOfNegativeOnes; i++){
				negs[i] = -1;
			}
			return negs;
		}

	}
}
