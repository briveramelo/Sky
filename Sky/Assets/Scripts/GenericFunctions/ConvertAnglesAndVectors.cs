using UnityEngine;
using System.Collections;

namespace GenericFunctions
{
	public static class ConvertAnglesAndVectors{

		/// <summary>Takes an (float)angle and
		/// <para> transforms it into a (Vector2)vector </para>
		/// </summary>
		public static Vector2 ConvertAngleToVector2(float inputAngle)
		{
			return new Vector2(Mathf.Cos (Mathf.Deg2Rad * inputAngle),Mathf.Sin (Mathf.Deg2Rad * inputAngle));
		}

		/// <summary>Takes an (float)angle and
		/// <para> transforms it into a (Vector3)vector </para>
		/// </summary>
		public static Vector3 ConvertAngleToVector3(int inputAngle)
		{
			return new Vector3(Mathf.Cos (Mathf.Deg2Rad * inputAngle),Mathf.Sin (Mathf.Deg2Rad * inputAngle),0f);
		}

		/// <summary>Takes a (Vector2)vector and
		/// <para> transforms it into a (float)angle </para>
		/// </summary>
		public static int ConvertVector2IntAngle(Vector2 inputVector)
		{
			return Mathf.RoundToInt (Mathf.Atan2 (inputVector.y, inputVector.x) * Mathf.Rad2Deg);
		}

		public static float ConvertVector2FloatAngle(Vector2 inputVector)
		{
			return Mathf.Atan2 (inputVector.y, inputVector.x) * Mathf.Rad2Deg;
		}

		public static int ConvertVector2IntAngle(Vector2 inputVector, float xVelocity)
		{
			int pixelAngle = Mathf.RoundToInt (Mathf.Atan2 (inputVector.y, inputVector.x) * Mathf.Rad2Deg);
			if (xVelocity<0){
				pixelAngle = 180 - pixelAngle; 
			}
			return pixelAngle;
		}

		public static int ConvertVector2SpearAngle(Vector2 inputVector, float yVelocity)
		{
			return Mathf.RoundToInt (Mathf.Atan2 (inputVector.y, inputVector.x) * Mathf.Rad2Deg)-90;
		}

		/// <summary>Uses x,y of a (Vector3)vector and
		/// <para> transforms it into a (float)angle </para>
		/// </summary>
		public static int ConvertVector3Angle(Vector3 inputVector)
		{
			return Mathf.RoundToInt (Mathf.Atan2 (inputVector.y, inputVector.x) * Mathf.Rad2Deg);
		}

		public static int ConvertVector2PositiveAngle(Vector2 inputVector)
		{
			return Mathf.RoundToInt (Mathf.Atan2 (Mathf.Abs(inputVector.y), inputVector.x) * Mathf.Rad2Deg);
		}
	}
}
