using System.IO;
using UnityEngine;

public static class FileLocationUtilities
{
    /// <summary>
    /// Accounts for persistent data path requirement in builds and convenience of project folder location in editor
    /// </summary>
    public static string GetDataPath(string relativeLocation)
    {
        string dataPath = Application.isEditor ? Application.dataPath : Application.persistentDataPath;
        return Path.Combine(dataPath, relativeLocation);
    }
}