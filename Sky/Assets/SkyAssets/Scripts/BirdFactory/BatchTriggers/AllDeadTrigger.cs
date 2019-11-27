using BRM.Sky.CustomWaveData;

namespace BRM.Sky.WaveEditor
{
    public class AllDeadTrigger : BatchTrigger
    {
        public AllDeadTrigger(BatchTriggerData data) : base(data)
        {
        }

        public override bool CanAdvance
        {
            get
            {
                var numAlive = ScoreSheet.Reporter.GetCount(BirdCounterType.BirdsAlive, WavePhase.CurrentWave, BirdType.All);
                const int numAliveNeed = 0;
                return numAlive <= numAliveNeed;
            }
        }
    }
}