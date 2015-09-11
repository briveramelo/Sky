using UnityEngine;
using System.Collections;

namespace GenericFunctions{
	public static class Constants{

		public static Vector3 Pixel3125(bool forward){
			return forward ? new Vector3 (3.125f, 3.125f, 1f) : new Vector3 (-3.125f, 3.125f, 1f);
		}

		public static Vector3 Pixel625(bool forward){
			return forward ? new Vector3 (6.25f, 6.25f, 1f) : new Vector3 (-6.25f, 6.25f, 1f);
		}

		public static Vector3 Pixel1(bool forward){
			return forward ? Vector3.one : new Vector3 (-1f, 1f, 1f);
		}

		public static float timeToThrow = .67f;
		public static Vector3 balloonOffset = new Vector3 (0.2f, 2.7f, 0f);

		public static int[] NegativeOnes(int numberOfNegativeOnes){
			int[] negs = new int[numberOfNegativeOnes];
			for (int i = 0; i<numberOfNegativeOnes; i++){
				negs[i] = -1;
			}
			return negs;
		}

		public static Vector3[] crowPositions  = new Vector3[]{
			new Vector3(0f   ,  7f , 0f),
			new Vector3(9.5f ,  5.3f , 0f),
			new Vector3(9.5f   , -5.3f , 0f),
			new Vector3(0f   , -7f , 0f),
			new Vector3(-9.5f, -5.3f , 0f),
			new Vector3(-9.5f,  5.3f , 0f),
		};
	}
}
