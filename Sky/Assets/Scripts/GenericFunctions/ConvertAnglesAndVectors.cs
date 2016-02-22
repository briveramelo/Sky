using UnityEngine;
using System.Collections;

namespace GenericFunctions
{
	public static class ConvertAnglesAndVectors{

		/// <summary>Takes an (float)angle and
		///  transforms it into a (Vector2)vector 
		/// </summary>
		public static Vector2 ConvertAngleToVector2(float inputAngle){
			return new Vector2(Mathf.Cos (Mathf.Deg2Rad * inputAngle),Mathf.Sin (Mathf.Deg2Rad * inputAngle));
		}

		/// <summary>Takes an (float)angle and
		///  transforms it into a (Vector3)vector 
		/// </summary>
		public static Vector3 ConvertAngleToVector3(int inputAngle){
			return new Vector3(Mathf.Cos (Mathf.Deg2Rad * inputAngle),Mathf.Sin (Mathf.Deg2Rad * inputAngle),0f);
		}

		public static float ConvertVector2FloatAngle(Vector2 inputVector){
			return Mathf.Atan2 (inputVector.y, inputVector.x) * Mathf.Rad2Deg;
		}

		public static int ConvertVector2IntAngle(Vector2 inputVector, bool forPixelAngle=true){
			int pixelAngle = Mathf.RoundToInt (Mathf.Atan2 (inputVector.y, inputVector.x) * Mathf.Rad2Deg);
			if (forPixelAngle && inputVector.x<0){
				pixelAngle = 180 - pixelAngle;
			}
			return pixelAngle;
		}

		public static int ConvertVector2SpearAngle(Vector2 inputVector){
			return Mathf.RoundToInt (Mathf.Atan2 (inputVector.y, inputVector.x) * Mathf.Rad2Deg)-90;
		}

		/// <summary>Uses x,y of a (Vector3)vector and
		///  transforms it into a (float)angle 
		/// </summary>
		public static int ConvertVector3Angle(Vector3 inputVector){
			return Mathf.RoundToInt (Mathf.Atan2 (inputVector.y, inputVector.x) * Mathf.Rad2Deg);
		}
	}
}
