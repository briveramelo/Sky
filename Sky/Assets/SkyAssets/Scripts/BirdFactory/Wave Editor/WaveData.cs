using System;
using System.Collections.Generic;
using UnityEngine;

namespace BRM.Sky.CustomWaveData
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
        public string Name;
        public List<SpawnEventData> SpawnEventData = new List<SpawnEventData>();
    }

    [Serializable]
    public class SpawnEventData
    {
        public SpawnPrefab SpawnPrefab
        {
            get => SpawnPrefabType.ToEnum<SpawnPrefab>();
            set => SpawnPrefabType = value.ToString();
        }
        [SerializeField] private string SpawnPrefabType;
        public string BatchName;//only applicable for when SpawnPrefab is type "Batch"
        public Vector2 NormalizedPosition;
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

        public BatchTriggerType TriggerType
        {
            get => BatchTriggerType.ToEnum<BatchTriggerType>();
            set => BatchTriggerType = value.ToString();
        }

        [SerializeField] private string BatchTriggerType;
        public float Amount;
    }

    public enum BatchTriggerType
    {
        AllDead = 0,
        Dead = 1,
        Spears = 2,
        Time = 3
    }

    public enum SpawnPrefab : ushort
    {
        //original birds
        Pigeon = 0,
        Duck = 1,
        LeadDuck = 2,
        Albatross = 3,
        BabyCrow = 4,
        Murder = 5,
        Seagull = 6,
        TheMightyTentacles = 7,
        Pelican = 8,
        Shoebill = 9,
        Bat = 10,
        Eagle = 11,
        BirdOfParadise = 12,
        Batch=13,//special case, wherein a collection of spawn prefabs may exist.
    }
}