﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System;
using System.Linq;
using GenericFunctions;
using UnityEngine.SceneManagement;

[Serializable]
public struct Range
{
    public float Min;
    public float Max;
    public float Diff => Max - Min;
    public float Average => (Min + Max) / 2;
    
    public Range(float min, float max)
    {
        if (min > max)
        {
            var tempMax = max;
            max = min;
            min = tempMax;
        }

        Min = min;
        Max = max;
    }

    public Range(Vector2 vec)
    {
        Min = vec.x;
        Max = vec.y;
    }

    public override string ToString()
    {
        return $"({Min}, {Max})";
    }
    public string ToString(string format)
    {
        return $"({Min.ToString(format)}, {Max.ToString(format)})";
    }
}


public class EndlessWave : Wave
{
    private enum Difficulty
    {
        Easy = 1,
        Medium = 3,
        Hard = 5
    }
    [SerializeField] private Difficulty _toughness;
    
    private OrderedDictionary _lockedStandardBirds = new OrderedDictionary()
    {
        {BirdType.Pigeon, 0f * 60f},
        {BirdType.Albatross, .5f * 60f},
        {BirdType.Seagull, 1f * 60f},
        {BirdType.Duck, 1.5f * 60f},
        {BirdType.Pelican, 2f * 60f},
        {BirdType.Shoebill, 2.5f * 60f},
        {BirdType.Bat, 3f * 60f}
    };
    private List<BirdType> _unlockedStandardBirds = new List<BirdType>();
    private OrderedDictionary _lockedBossBirds = new OrderedDictionary()
    {
        {BirdType.DuckLeader, 3.5f * 60f},
        {BirdType.Tentacles, 4f * 60f},
        {BirdType.BabyCrow, 4.5f * 60f},
        {BirdType.Eagle, 5f * 60f}
    };

    private List<BirdType> _unlockedBossBirds = new List<BirdType>();
    private float _emotionalCap = 50f;
    private float _emotionalSafePoint = 10f;

    protected override IEnumerator GenerateBirds()
    {
        StartCoroutine(UnlockBirdies(_lockedStandardBirds, _unlockedStandardBirds));
        StartCoroutine(UnlockBirdies(_lockedBossBirds, _unlockedBossBirds));
        StartCoroutine(SpawnBirdies(SelectStandardBirds, new Range(0.5f, 3f)));
        yield return StartCoroutine(SpawnBirdies(SelectBossBirds, new Range(30f, 45f)));
    }

    private BirdType[] SelectStandardBirds()
    {
        if (_unlockedStandardBirds.Count > 0)
        {
            var numBirds = (int) _toughness;
            var birdTypes = new BirdType[numBirds];
            for (var i = 0; i < numBirds; i++)
            {
                birdTypes[i] = _unlockedStandardBirds[UnityEngine.Random.Range(0, _unlockedStandardBirds.Count)];
            }

            return birdTypes;
        }

        return new[] {BirdType.All};
    }

    private BirdType[] SelectBossBirds()
    {
        if (_unlockedBossBirds.Count > 0)
        {
            return new[] {_unlockedBossBirds[UnityEngine.Random.Range(0, _unlockedBossBirds.Count)]};
        }
        
        return new[] {BirdType.All};
    }

    private IEnumerator UnlockBirdies(OrderedDictionary lockedBirds, List<BirdType> unlockedBirds)
    {
        for (var i = 0; i < lockedBirds.Count; i++)
        {
            yield return new WaitForSeconds((float) lockedBirds.Cast<DictionaryEntry>().ElementAt(i).Value);
            var unlockedBird = (BirdType) lockedBirds.Cast<DictionaryEntry>().ElementAt(i).Key;
            unlockedBirds.Add(unlockedBird);
            SpawnBirds(unlockedBird, SpawnPoint(Bool.TossCoin(), LowHeight, HighHeight));
        }
    }

    private IEnumerator SpawnBirdies(Func<BirdType[]> selectBirds, Range timeRange)
    {
        while (true)
        {
            while (EmotionalIntensity.Intensity < _emotionalCap)
            {
                yield return StartCoroutine(WaitUntilTimeRange(timeRange.Min, timeRange.Max));
                var birdsToSpawn = selectBirds();
                foreach (var bird in birdsToSpawn)
                {
                    if (bird != BirdType.All)
                    {
                        BirdSpawnDelegates[bird]();
                    }
                }
            }

            Debug.LogWarning("Waiting for less stress");
            while (EmotionalIntensity.Intensity > _emotionalSafePoint)
            {
                yield return null;
            }

            yield return new WaitForSeconds(2f);
        }
    }
}