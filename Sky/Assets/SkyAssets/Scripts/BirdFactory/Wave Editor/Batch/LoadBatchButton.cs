using System.Collections;
using BRM.Sky.CustomWaveData;
using UnityEngine;

namespace BRM.Sky.WaveEditor.Ui
{
    public class LoadBatchButton : LoadButton<BatchDataMarshal, BatchData, BatchView>
    {

        [SerializeField] private SpawnEventButtonFactory _buttonFactory;
        protected override string _folderName => $"{Application.dataPath}/SkyAssets/WaveData/Batches/";
        protected override string _fileName => $"{_dataMarshal.BatchName}.json";
        
        protected override IEnumerator OnClickRoutine()
        {
            _buttonFactory.DestroyButtons();
            yield return null;
            var data = _fileReader.Read<BatchData>(_filePath);
            _buttonFactory.CreateButtons(data.SpawnEventData.Count);
            _dataMarshal.Data = data;
        }
    }
}