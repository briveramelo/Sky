using UnityEngine;
using System.Collections;

namespace GenericFunctions{

	public static class Constants{

		public static Transform jaiTransform;
		public static Transform balloonCenter;
		public static Transform basketTransform;
		public static Collider2D worldBoundsCollider;
		public static Collider2D bottomOfTheWorldCollider;

		public static int poosOnJaisFace;
		static int targetPooInt;
		public static int TargetPooInt {
			get{return targetPooInt;}
			set{ targetPooInt = value;
				if (targetPooInt>4)
					targetPooInt =0;
			}
		}

		public static void FaceFoward(this Transform trans, bool forward){
			trans.localScale = forward ? Vector3.one : new Vector3 (-1f, 1f, 1f);
		}

		public static void Face4ward(this Transform trans, bool forward){
			trans.localScale = forward ? Vector3.one * 4f : new Vector3 (-4f, 4f, 1f);
		}

		public static Vector2 screenDimensions = new Vector2 (Screen.width, Screen.height);
		public static Vector2 worldDimensions = screenDimensions /200f;

		public static Vector2 correctionPixels = new Vector2 (Screen.width/2,(-3*Screen.height/2));
		public static float correctionPixelFactor = worldDimensions.y * 2 / Screen.height;

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

		/// <summary> side is +/- 1 and y position is between -1 <-> +1
		/// Multiplies these input numbers by worldDimensions
		/// </summary>
		public static Vector2 RandomSpawnHeight(int side, float minY, float maxY){
			return new Vector2 (side * worldDimensions.x, Random.Range (minY, maxY) * worldDimensions.y);
		}

		/// <summary> x and y position ranges are between -1 <-> +1
		/// Multiplies these input numbers by worldDimensions
		/// </summary>
		public static Vector2 RandomSpawnPoint(float minX, float maxX, float minY, float maxY){
			return new Vector2 (Random.Range (minX, maxX) * worldDimensions.x, Random.Range (minY, maxY) * worldDimensions.y);
		}

		/// <summary> Choose a side (+/- 1) and y position (-1 <-> +1)
		/// Multiplies these input numbers by worldDimensions
		/// </summary>
		public static Vector2 FixedSpawnHeight (int side, float y){
			return new Vector2 (side * worldDimensions.x, y * worldDimensions.y);
		}

		/// <summary> Choose an x position (-1 <-> +1) and y position (-1 <-> +1)
		/// Multiplies these input numbers by worldDimensions
		/// </summary>
		public static Vector2 FixedSpawnPoint(float x, float y){
			return new Vector2 (x * worldDimensions.x, y * worldDimensions.y);
		}

		public static Vector2 stockSpearPosition = new Vector3 (0.14f, 0.12f);
		public static Vector2 balloonOffset = new Vector2 (0f, 1.6f);
		public static Vector2 seagullOffset = balloonOffset + Vector2.up * 1.25f;
		public static float time2Destroy = 2f;
		public static float time2ThrowSpear = 0.66667f;

		public static int[] NegativeOnes(int numberOfNegativeOnes){
			int[] negs = new int[numberOfNegativeOnes];
			for (int i = 0; i<numberOfNegativeOnes; i++){
				negs[i] = -1;
			}
			return negs;
		}

	}
}
