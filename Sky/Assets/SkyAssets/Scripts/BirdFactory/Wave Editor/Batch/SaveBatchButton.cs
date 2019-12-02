using BRM.EventBrokers;
using BRM.EventBrokers.Interfaces;
using BRM.FileSerializers;
using BRM.Sky.CustomWaveData;
using GenericFunctions;

namespace BRM.Sky.WaveEditor
{
    public class SaveBatchButton : SaveButton<BatchDataMarshal, BatchData, BatchView>
    {
        protected override string _folderName => FileLocationUtilities.GetDataPath(Constants.BatchRelativeFolder);
        protected override string _fileName => $"{_dataMarshal.BatchName}.{Constants.BatchFileExtension}";
        
        private IPublishEvents _eventPublisher = new StaticEventBroker();
        
        protected override void OnClick()
        {
            string filePath = FileUtilities.GetUniqueFilePath(_filePath);
            var data = _dataMarshal.Data;
            _fileWriter.Write(filePath, data);
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif
            _eventPublisher.Publish(new BatchSavedData());
            AudioManager.PlayAudio(_audioType);
        }
    }
}