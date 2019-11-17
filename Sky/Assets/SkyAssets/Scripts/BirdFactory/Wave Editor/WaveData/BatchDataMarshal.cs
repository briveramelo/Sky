namespace BRM.Sky.WaveEditor
{
    public class BatchDataMarshal : DataMarshal<BatchData>
    {
        public override BatchData Data { get; }
        public override bool IsDataReady { get; }
    }
}