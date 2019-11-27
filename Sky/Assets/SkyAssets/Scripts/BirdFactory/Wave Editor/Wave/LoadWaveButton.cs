using System.Collections;
using BRM.Sky.CustomWaveData;
using BRM.Sky.WaveEditor.Ui;
using GenericFunctions;
using UnityEngine;

namespace BRM.Sky.WaveEditor
{
    public class LoadWaveButton : LoadButton<WaveDataMarshal, WaveData, WaveView>
    {
        [SerializeField] private WaveDataMarshal _waveDataMarshal;
        [SerializeField] private BatchButtonFactory _batchButtonFactory;

        protected override string _folderName => FileLocationUtilities.GetDataPath(Constants.WaveRelativeFolder);
        protected override string _fileName => $"{_dataMarshal.WaveName}.{Constants.WaveFileExtension}";

        protected override void Awake()
        {
            base.Awake();
            _dataMarshal = _waveDataMarshal;
        }

        protected override IEnumerator OnClickRoutine()
        {
            _batchButtonFactory.DestroyButtons();
            yield return null;
            var data = _fileReader.Read<WaveData>(_filePath);
            _batchButtonFactory.CreateButtons(data.WaveTimeline.Batches.Count);
            _waveDataMarshal.Data = data;
        }
    }
}