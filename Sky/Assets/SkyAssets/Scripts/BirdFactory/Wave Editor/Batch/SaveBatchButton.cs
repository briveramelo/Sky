using BRM.Sky.CustomWaveData;

namespace BRM.Sky.WaveEditor
{
    public class SaveBatchButton : SaveButton<BatchDataMarshal, BatchData, BatchView>
    {
        protected override string _folderName => FileLocationUtilities.GetDataPath("/SkyAssets/WaveData/Batches/");
        protected override string _fileName => $"{_dataMarshal.BatchName}.json";
    }
}