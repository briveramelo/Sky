using System;
using System.Collections.Generic;
using UnityEngine;

namespace BRM.Sky.WaveEditor
{
    [Serializable]
    public class WaveData
    {
        public string Name;
        public string Subtitle;

        public WaveTimeline WaveTimeline;
    }

    [Serializable]
    public class WaveTimeline
    {
        public List<BatchData> Batches = new List<BatchData>();
        public List<BatchTriggerData> Triggers = new List<BatchTriggerData>();
    }

    [Serializable]
    public class BatchData
    {
        public List<SpawnEventData> SpawnEventData = new List<SpawnEventData>();
    }

    [Serializable]
    public class SpawnEventData
    {
        public SpawnPrefab SpawnPrefab;
        public Vector2 Position;
        public float TimeAfterBatchStartSec;
    }

    [Serializable]
    public class BatchTriggerData
    {
        public BatchTriggerData()
        {
        }

        public BatchTriggerData(BatchTriggerData other)
        {
            TriggerType = other.TriggerType;
            Amount = other.Amount;
        }

        public BatchTriggerType TriggerType;
        public float Amount;
    }

    public enum BatchTriggerType
    {
        AllDead = 0,
        NumDead = 1,
        NumSpears = 2,
        Time = 3
    }

    public enum SpawnPrefab : ushort
    {
        //original birds
        Pigeon = 0,
        Duck = 1,
        DuckLeader = 2,
        Albatross = 3,
        BabyCrow = 4,
        Crow = 5,
        Seagull = 6,
        Tentacles = 7,
        Pelican = 8,
        Shoebill = 9,
        Bat = 10,
        Eagle = 11,
        BirdOfParadise = 12,

        //bird collections
        PigeonMeatball = 20,
        PigeonWall = 21,
        PigeonSlantWall = 22,
        PigeonBite = 23,
        PigeonSine = 24,
        PigeonLine = 25,
    }
}