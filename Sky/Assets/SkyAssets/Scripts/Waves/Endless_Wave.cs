﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System;
using System.Linq;
using GenericFunctions;
using UnityEngine.SceneManagement;

public struct Range
{
    public float min;
    public float max;
    public Range(float min, float max)
    {
        if (min>max) {
            float tempMax = max;
            max = min;
            min = tempMax;
        }
        this.min = min;
        this.max = max;
    }
}
public enum Difficulty{
    Easy = 1,
    Medium = 3,
    Hard = 5
}
public class Endless_Wave : Wave {

    
    [SerializeField] private Difficulty Toughness;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode) {
        if (scene.name !=Scenes.Endless) {
            StopAllCoroutines();
        }
    }

    protected override IEnumerator GenerateBirds()
    {
        StartCoroutine(UnlockBirdies(lockedStandardBirds, unlockedStandardBirds));
        StartCoroutine(UnlockBirdies(lockedBossBirds, unlockedBossBirds));
        StartCoroutine(SpawnBirdies(SelectStandardBirds, new Range(0.5f, 3f)));
        yield return StartCoroutine(SpawnBirdies(SelectBossBirds, new Range(30f, 45f)));
    }

    private BirdType[] SelectStandardBirds() {
        if (unlockedStandardBirds.Count > 0){
            BirdType[] birdTypes = new BirdType[(int)Toughness];
            for (int i = 0; i < (int)Toughness; i++){
                birdTypes[i] = unlockedStandardBirds[UnityEngine.Random.Range(0, unlockedStandardBirds.Count)];
            }
            return birdTypes;
        }
        return new[] { BirdType.All };
    }

    private BirdType[] SelectBossBirds() {
        if (unlockedBossBirds.Count > 0){
            return new[] { unlockedBossBirds[UnityEngine.Random.Range(0, unlockedBossBirds.Count)] };
        }
        else{
            return new[] { BirdType.All };
        }
    }

    private IEnumerator UnlockBirdies(OrderedDictionary lockedBirds, List<BirdType> unlockedBirds) {
        for (int i = 0; i < lockedBirds.Count; i++) {
            yield return new WaitForSeconds((float)lockedBirds.Cast<DictionaryEntry>().ElementAt(i).Value);
            BirdType unlockedBird = (BirdType)lockedBirds.Cast<DictionaryEntry>().ElementAt(i).Key;
            unlockedBirds.Add(unlockedBird);
            SpawnBirds(unlockedBird, SpawnPoint(Bool.TossCoin(), lowHeight, highHeight));
        }
    }

    private float emotionalCap = 50f;
    private float emotionalSafePoint = 10f;

    private IEnumerator SpawnBirdies(Func<BirdType[]> SelectBirds, Range timeRange) {
        while (true) {
            while (EmotionalIntensity.Intensity < emotionalCap) {
                yield return StartCoroutine(WaitUntilTimeRange(timeRange.min, timeRange.max));
                BirdType[] birdsToSpawn = SelectBirds();
                foreach (BirdType bird in birdsToSpawn) {
                    if (bird != BirdType.All) {
                        BirdSpawnDelegates[bird]();
                    }
                }
            }
            Debug.LogWarning("Waiting for less stress");
            while (EmotionalIntensity.Intensity > emotionalSafePoint) {
                yield return null;
            }
            yield return new WaitForSeconds(2f);
        }
    }

	#region BirdType Collections

    private OrderedDictionary lockedStandardBirds = new OrderedDictionary(){
		{BirdType.Pigeon, 		0f * 60f},
		{BirdType.Albatross,    .5f * 60f},
		{BirdType.Seagull,      1f * 60f},
		{BirdType.Duck,         1.5f * 60f},
		{BirdType.Pelican,      2f * 60f},
		{BirdType.Shoebill,		2.5f * 60f},
		{BirdType.Bat,          3f * 60f}
	};

    private List<BirdType> unlockedStandardBirds = new List<BirdType>();

    private OrderedDictionary lockedBossBirds = new OrderedDictionary(){
		{BirdType.DuckLeader,   3.5f * 60f },
        {BirdType.Tentacles,    4f * 60f },
		{BirdType.BabyCrow,     4.5f * 60f },
        {BirdType.Eagle,        5f * 60f }
    };

    private List<BirdType> unlockedBossBirds = new List<BirdType>();
    #endregion
}
