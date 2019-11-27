using System.Collections.Generic;
using System.IO;
using BRM.DebugAdapter;
using BRM.FileSerializers;
using BRM.FileSerializers.Interfaces;
using BRM.Sky.CustomWaveData;
using BRM.TextSerializers;
using GenericFunctions;

namespace BRM.Sky.WaveEditor
{
    public class CustomBatchDataLoader
    {
        private static string _dataFolder => FileLocationUtilities.GetDataPath(Constants.BatchRelativeFolder);
        private static IReadFiles _fileReader = new TextFileSerializer(new UnityJsonSerializer(), new UnityDebugger());

        public static List<BatchData> GetCustomBatchData()
        {
            var dataList = new List<BatchData>();
            var jsonFilePaths = Directory.GetFiles(_dataFolder, $"*.{Constants.BatchFileExtension}");
            for (int i = 0; i < jsonFilePaths.Length; i++)
            {
                var path = jsonFilePaths[i];
                var batchData = _fileReader.Read<BatchData>(path);
                if (batchData != null)
                {
                    dataList.Add(batchData);
                }
            }

            return dataList;
        }
    }
}