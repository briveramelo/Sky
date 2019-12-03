public enum BirdType
{
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
    All = 13
}

namespace BRM.Sky.CustomWaveData
{
    public enum SpawnPrefab : ushort
    {
        //original birds
        Pigeon,
        Duck,
        LeadDuck,
        Albatross,
        BabyCrow,
        Murder,
        Seagull,
        TheMightyTentacles,
        Pelican,
        Shoebill,
        Bat,
        Eagle,
        BirdOfParadise,
        Batch, //special case, wherein a collection of spawn prefabs may exist.
    }
}