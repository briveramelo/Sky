﻿using System;
using UnityEngine;
using System.Collections;
 using System.Collections.Generic;
 using System.Linq;
 using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
 
namespace GenericFunctions
{
    public static class Chance
    {
        public static T GetRandom<T>(this List<T> list)
        {
            var listCount = list.Count;
            var randomIndex = Random.Range(0, listCount);
            return list[randomIndex];
        }
    }

    public static class Transforms
    {
        public static void DestroyChildren(this Transform tran)
        {
            for (int i = 0; i < tran.childCount; i++)
            {
                Object.Destroy(tran.GetChild(i));
            }
        }
        public static List<T> GetComponentsRecursively<T>(this GameObject go) where T : Component
        {
            var components = go.GetComponents<T>().ToList();
            var childBehaviours = go.GetComponentsInChildren<T>(true);
            components.AddRange(childBehaviours);
            return components;
        }
    }

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
        
        public static void FaceForward(this Transform trans, bool forward)
        {
            var localScale = trans.localScale;
            localScale = new Vector3((forward ? 1 : -1) * Mathf.Abs(localScale.x), localScale.y, localScale.z);
            trans.localScale = localScale;
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

    //todo, phase out when DI framework is introduced
    public static class EasyAccess
    {
        public static Transform JaiTransform;
        public static Transform BalloonCenter;
        public static Transform BasketTransform;
    }

    public static class Constants
    {
        public const int UnusedFingerId = -1;
        public const float SpeedMultiplier = 0.25f;//accounts for the time things were all scaled up by 4
        public const float Time2ThrowSpear = 0.333333f;
        public const float Time2StrikeLightning = 0.5f;

        public static int AnimState => _animState.Value;
        private static Lazy<int> _animState = new Lazy<int>(() => Animator.StringToHash("AnimState"));
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