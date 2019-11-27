using BRM.Sky.CustomWaveData;

namespace BRM.Sky.WaveEditor
{
    public class NumDeadTrigger : BatchTrigger
    {
        public NumDeadTrigger(BatchTriggerData data) : base(data)
        {
        }

        public override bool CanAdvance
        {
            get
            {
                var killedInBatch = ScoreSheet.Reporter.GetCount(BirdCounterType.BirdsKilled, WavePhase.CurrentBatch, BirdType.All);
                var numDeadNeeded = _data.Amount;
                return killedInBatch >= numDeadNeeded;
            }
        }
    }
}