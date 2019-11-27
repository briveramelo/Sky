using BRM.Sky.CustomWaveData;
using GenericFunctions;
using UnityEngine;

namespace BRM.Sky.WaveEditor
{
    public class SaveWaveButton : SaveButton<WaveDataMarshal, WaveData, WaveView>
    {
        [SerializeField] private WaveDataMarshal _waveDataMarshal;

        protected override string _folderName => FileLocationUtilities.GetDataPath(Constants.WaveRelativeFolder);
        protected override string _fileName => $"{_dataMarshal.WaveName}.{Constants.WaveFileExtension}";

        protected override void Awake()
        {
            base.Awake();
            SetDataMarshal(_waveDataMarshal);
        }
    }
}