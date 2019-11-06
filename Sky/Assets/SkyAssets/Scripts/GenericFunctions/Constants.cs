﻿﻿using System;
using UnityEngine;
using System.Collections;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
#if UNITY_EDITOR
using UnityEditor;
#endif
 
 namespace GenericFunctions
{
    public static class Bool
    {
        public static bool TossCoin()
        {
            return Random.value > 0.5f;
        }

        public static IEnumerator Toggle(Action<bool> lambda, float time2Wait)
        {
            lambda(false);
            yield return new WaitForSeconds(time2Wait);
            lambda(true);
        }
    }

    public static class Delay
    {
        public static IEnumerator DelayAction(Action onComplete, float time2Wait)
        {
            yield return new WaitForSeconds(time2Wait);
            onComplete?.Invoke();
        }
        public static IEnumerator DelayAction(Action onComplete, int numFrames)
        {
            for (int i = 0; i < numFrames; i++)
            {
                yield return null;
            }
            onComplete?.Invoke();
        }
    }

    public static class Constants
    {
        public static Transform JaiTransform;
        public static Transform BalloonCenter;
        public static Transform BasketTransform;
        public static Collider2D WorldCollider;
        public static float PositionFrameSpeed => Time.deltaTime * TargetFramesPerSecond;
        public const float TargetFramesPerSecond = 60;
        public const int UnusedFingerId = -1;
        private static int _targetPooInt;

        public static int TargetPooInt
        {
            get => _targetPooInt;
            set
            {
                _targetPooInt = value;
                if (_targetPooInt > 4)
                {
                    _targetPooInt = 0;
                }
            }
        }

        public static void FaceForward(this Transform trans, bool forward)
        {
            var localScale = trans.localScale;
            localScale = new Vector3((forward ? 1 : -1) * Mathf.Abs(localScale.x), localScale.y, localScale.z);
            trans.localScale = localScale;
        }

        public static Vector2 WorldPadding => new Vector2(1f, 1f);

        public static float SpeedMultiplier => 0.25f;//accounts

        public const float Time2Destroy = 2f;
        public const float Time2ThrowSpear = 0.333333f;
        public const float Time2StrikeLightning = 0.5f;

        private static Lazy<int> _animState = new Lazy<int>(() => Animator.StringToHash("AnimState"));
        public static int AnimState => _animState.Value;
    }
    
    public static class Layers
    {
        public const int BasketLayer = 8;
        public const int SpearLayer = 9;
        public const int BirdLayer = 10;
        public const int BalloonLayer = 11;
        public const int BalloonFloatingLayer = 12;
        public const int JaiLayer = 13;
        public const int FaceLayer = 14;
        public const int TentacleLayer = 15;
        public const int TentacleSensorLayer = 16;
        public const int TeleporterLayer = 17;
        public const int WorldBoundsLayer = 18;
        public const int PooNuggetLayer = 19;
        public const int BalloonBoundsLayer = 20;
        public const int ShoebillHornLayer = 21;
        public const int CollectableWeaponLayer = 22;
    }
}