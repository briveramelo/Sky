using BRM.Sky.CustomWaveData;
using UnityEngine;

namespace BRM.Sky.WaveEditor
{
    public class SaveWaveDataButton : SaveButton<WaveDataMarshal, WaveData, WaveView>
    {
        [SerializeField] private WaveDataMarshal _waveDataMarshal;

        protected override void Awake()
        {
            base.Awake();
            SetDataMarshal(_waveDataMarshal);
        }

        protected override string _folderName => $"{Application.dataPath}/SkyAssets/WaveData/Waves/";
        protected override string _fileName => $"{_dataMarshal.WaveName}.json";
    }
}
