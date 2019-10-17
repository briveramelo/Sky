﻿using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;

public class AdDisplayer : MonoBehaviour {

    [SerializeField] string iOS_GameID, android_GameID;
    static string gameID;

    void Awake() {
        #if UNITY_IOS // If build platform is set to iOS...
        gameID = iOS_GameID;
        #elif UNITY_ANDROID // Else if build platform is set to Android...
        gameID = android_GameID;
        #endif
    }

    public static IEnumerator DisplayAd() {
        if (string.IsNullOrEmpty (gameID)) {
            gameID = null;
        }

        ShowOptions options = new ShowOptions();
        options.resultCallback = HandleShowResult;
        Advertisement.Show (gameID, options);
        while (Advertisement.isShowing) {
            yield return new WaitForSeconds(0.25f);
        }
    }

    static void HandleShowResult (ShowResult result){
        switch (result){
        case ShowResult.Finished:
            Debug.Log ("Video completed!");
            break;
        case ShowResult.Skipped:
            Debug.LogWarning ("Video skipped :(");
            break;
        case ShowResult.Failed:
            Debug.LogError ("Video failed to show.");
            break;
        }
    }
}