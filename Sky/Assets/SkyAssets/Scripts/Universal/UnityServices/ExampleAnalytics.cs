using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Analytics;
using UnityEngine.Advertisements;

public class ExampleAnalytics : MonoBehaviour {

	void CustomEvents() {
        Debug.Log("Achieved!");
        Analytics.CustomEvent("gameOver", new Dictionary<string, object>{
        { "potions", 1 },
        { "coins", 2 },
        { "activeWeapon", 3}
      });
    }

    void Transactions() {
        Debug.Log("Transacted!");
        Analytics.Transaction("Take MY MONEY", 0.99m, "USD", null, null);
    }

    void Demographics() {
        Debug.Log("Demographed!");
        Gender gender = Gender.Male;
        Analytics.SetUserGender(gender);
        int birthYear = 1991;
        Analytics.SetUserBirthYear(birthYear);
    }

    void ShowAchievementButton (){
        Rect buttonRect = new Rect (500, 500, 150, 50);

        if (GUI.Button (buttonRect, "Achieve")) {
            CustomEvents();
            //Transactions();
            //Demographics();
        }
    }

    public string zoneId = "1065731";
    public int rewardQty = 250;

    void OnGUI (){
        ShowAchievementButton();
        if (string.IsNullOrEmpty (zoneId)) {
            zoneId = null;
        }

        Rect buttonRect = new Rect (100, 100, 150, 50);
        string buttonText = Advertisement.IsReady (zoneId) ? "Show Ad" : "Waiting...";

        ShowOptions options = new ShowOptions();
        options.resultCallback = HandleShowResult;

        if (GUI.Button (buttonRect, buttonText)) {
            Advertisement.Show (zoneId, options);
        }
    }

    void HandleShowResult (ShowResult result){
        switch (result){
        case ShowResult.Finished:
            Debug.Log ("Video completed. User rewarded " + rewardQty + " credits.");
            break;
        case ShowResult.Skipped:
            Debug.LogWarning ("Video was skipped.");
            break;
        case ShowResult.Failed:
            Debug.LogError ("Video failed to show.");
            break;
        }
    }
}
