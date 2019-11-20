using BRM.Sky.CustomWaveData;
using UnityEngine;

namespace BRM.Sky.WaveEditor
{
    public class SaveBatchButton : SaveButton<BatchDataMarshal, BatchData, BatchView>
    {
        protected override string _folderName => $"{Application.dataPath}/SkyAssets/WaveData/Batches/";
        protected override string _fileName => $"{_dataMarshal.BatchName}.json";
    }
}