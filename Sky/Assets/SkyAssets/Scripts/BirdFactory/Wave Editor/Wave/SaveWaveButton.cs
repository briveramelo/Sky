using BRM.Sky.CustomWaveData;
using UnityEngine;

namespace BRM.Sky.WaveEditor
{
    public class SaveWaveButton : SaveButton<WaveDataMarshal, WaveData, WaveView>
    {
        [SerializeField] private WaveDataMarshal _waveDataMarshal;

        protected override string _folderName => FileLocationUtilities.GetDataPath("/SkyAssets/WaveData/Waves/");
        protected override string _fileName => $"{_dataMarshal.WaveName}.json";

        protected override void Awake()
        {
            base.Awake();
            SetDataMarshal(_waveDataMarshal);
        }
    }
}