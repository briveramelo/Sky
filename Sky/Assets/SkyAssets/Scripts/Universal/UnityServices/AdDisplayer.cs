using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;

public class AdDisplayer : MonoBehaviour
{
    [SerializeField] private string _iOsGameId;
    [SerializeField] private string _androidGameId;
    private static string _gameId;

    private void Awake()
    {
#if UNITY_IOS // If build platform is set to iOS...
        gameID = iOS_GameID;
#elif UNITY_ANDROID // Else if build platform is set to Android...
        gameID = android_GameID;
#endif
    }

    public static IEnumerator DisplayAd()
    {
        if (string.IsNullOrEmpty(_gameId))
        {
            _gameId = null;
        }

        var options = new ShowOptions();
        options.resultCallback = HandleShowResult;
        Advertisement.Show(_gameId, options);
        while (Advertisement.isShowing)
        {
            yield return new WaitForSeconds(0.25f);
        }
    }

    private static void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("Video completed!");
                break;
            case ShowResult.Skipped:
                Debug.LogWarning("Video skipped :(");
                break;
            case ShowResult.Failed:
                Debug.LogError("Video failed to show.");
                break;
        }
    }
}