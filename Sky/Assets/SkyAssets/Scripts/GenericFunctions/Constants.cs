﻿using UnityEngine;
using System.Collections;

namespace GenericFunctions
{
    public static class Bool
    {
        public static bool TossCoin()
        {
            return Random.value > 0.5f;
        }

        public static IEnumerator Toggle(System.Action<bool> lambda, float time2Wait)
        {
            lambda(false);
            yield return new WaitForSeconds(time2Wait);
            lambda(true);
        }
    }

    public static class Constants
    {
        public const float SpeedMultiplier = 0.25f;
        public static Transform JaiTransform;
        public static Transform BalloonCenter;
        public static Transform BasketTransform;
        public static Collider2D BottomOfTheWorldCollider;

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

        public static Vector2 ScreenSize => new Vector2 (Screen.width, Screen.height);

        public static Vector2 WorldSize
        {
            get
            {
                var pixelCam = Object.FindObjectOfType<PixelPerfectCamera>();
                var cam = pixelCam.normalCamera;
                var scaleFactor = 1f / pixelCam.cameraZoom;
                var height = cam.orthographicSize * scaleFactor;
                
                var size = new Vector2(height * cam.aspect, height);//1f / pixelCam.cameraZoom * ;
                return size;
            }
        }

        public static Vector2 WorldPadding => new Vector2(1f, 1f);

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

        public const float Time2Destroy = 2f;
        public const float Time2ThrowSpear = 0.333333f;
        public const float Time2StrikeLightning = 0.5f;

        private static int? _animState;

        public static int AnimState
        {
            get
            {
                if (!_animState.HasValue)
                {
                    _animState = Animator.StringToHash("AnimState");
                }

                return _animState.Value;
            }
        }
    }
}