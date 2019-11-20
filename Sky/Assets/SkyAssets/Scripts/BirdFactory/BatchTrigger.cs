using BRM.Sky.CustomWaveData;

namespace SkyAssets.Scripts.BirdFactory
{
    public class BatchTrigger
    {
        public bool Wait => _wait(_movingNumber);
    
        private BirdCounterType _birdCounterType;
        private BirdType[] _birdTypes;
        private int _numberToWaitFor;
        private int _movingNumber => ScoreSheet.Reporter.GetCounts(_birdCounterType, true, _birdTypes);

        private delegate bool BoolDelegate(int mover);
        private BoolDelegate _wait;
        
        private BatchTriggerData _data;

        public bool IsWaiting { get; }
    }

    public static class Triggers
    {
        public static bool IsWaitingForAllDead()
        {
            var currentAlive = ScoreSheet.Reporter.GetCounts(BirdCounterType.BirdsAlive, true, BirdType.All);
            return currentAlive == 0;
        }
        
        public static bool IsWaitingForNumDead(int numDead)
        {
            var currentKilled = ScoreSheet.Reporter.GetCounts(BirdCounterType.BirdsKilled, true, BirdType.All);
            return currentKilled == numDead;
        }
        
        public static bool IsWaitingForNumSpears(int numSpears)
        {
            var currentThrown = ScoreSheet.Reporter.GetScore(ScoreCounterType.SpearsThrown, true);
            return currentThrown == numSpears;
        }
    }
}