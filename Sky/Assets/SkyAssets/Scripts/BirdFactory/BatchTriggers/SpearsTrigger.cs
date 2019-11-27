using BRM.Sky.CustomWaveData;

namespace BRM.Sky.WaveEditor
{
    public class SpearsTrigger : BatchTrigger
    {
        public SpearsTrigger(BatchTriggerData data) : base(data)
        {
        }

        public override bool CanAdvance
        {
            get
            {
                var currentThrown = ScoreSheet.Reporter.GetScore(ScoreCounterType.SpearsThrown, WavePhase.CurrentBatch);
                var numSpearsToThrow = _data.Amount;
                return currentThrown >= numSpearsToThrow;
            }
        }
    }
}