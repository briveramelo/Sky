﻿using UnityEngine;
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

		public static float timeToThrow = .66667f;
		public static float timeToStab = .1f;

		public static int basketLayer = 13;
		public static int spearLayer = 14;
		public static int joystickLayer = 15;
		public static int birdLayer = 16;
		public static int balloonLayer = 17;
		public static int balloonFloatingLayer = 18;
		public static int tentacleLayer = 19;
		public static int jaiLayer = 20;

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

		public static Vector3 balloonOffset = new Vector3 (0.1f, 2.7f, 0f);
		public static Vector3 tentacleTipOffset = new Vector3 (-0f, 1.25f, 0f);
		public static Vector3 tentacleHomeSpot = new Vector3 (0f, -6.75f, 0f);
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
