using UnityEngine;
using System.Collections;

namespace GenericFunctions{

	public static class Bool{
		public static bool TossCoin(){
			return Random.value>0.5f;
		}
		public static IEnumerator Toggle(System.Action<bool> lambda, float time2Wait){
			lambda(false);
			yield return new WaitForSeconds(time2Wait);
			lambda(true);
		}
	}

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

		public static void FaceForward(this Transform trans, bool forward){
			trans.localScale = new Vector3 ((forward ? 1:-1) * Mathf.Abs(trans.localScale.x), trans.localScale.y, trans.localScale.z);
		}
			
		static Vector2 screenDimensions = new Vector2 (Screen.width, Screen.height);
		public static Vector2 ScreenDimensions{get{return screenDimensions;}}
		static Vector2 worldDimensions = ScreenDimensions /200f;
		public static Vector2 WorldDimensions{get{return worldDimensions;}}

		public static Vector2 correctionPixels = new Vector2 (Screen.width/2,(-3*Screen.height/2));
		public static float correctionPixelFactor = WorldDimensions.y * 2 / Screen.height;

		public const int basketLayer = 8;
		public const int spearLayer = 9;
		public const int birdLayer = 10;
		public const int balloonLayer = 11;
		public const int balloonFloatingLayer = 12;
		public const int jaiLayer = 13;
		public const int faceLayer = 14;
		public const int tentacleLayer = 15;
		public const int tentacleSensorLayer = 16;
		public const int teleporterLayer = 17;
		public const int worldBoundsLayer = 18;
		public const int pooNuggetLayer = 19;
		public const int balloonBoundsLayer = 20;

		public static Vector2 stockSpearPosition = new Vector3 (0.14f, 0.12f);
		public static Vector2 balloonOffset = new Vector2 (0f, 1.6f);
		public static Vector2 seagullOffset = balloonOffset + Vector2.up * 1.25f;
		public const float time2Destroy = 2f;
		public const float time2ThrowSpear = 0.333333f;
	}
}
