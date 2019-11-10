using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Analytics;
using UnityEngine.Advertisements;

public class ExampleAnalytics : MonoBehaviour
{
    private void CustomEvents()
    {
        Debug.Log("Achieved!");
        Analytics.CustomEvent("gameOver", new Dictionary<string, object>
        {
            {"potions", 1},
            {"coins", 2},
            {"activeWeapon", 3}
        });
    }

    private void Transactions()
    {
        Debug.Log("Transacted!");
        Analytics.Transaction("Take MY MONEY", 0.99m, "USD", null, null);
    }

    private void Demographics()
    {
        Debug.Log("Demographed!");
        var gender = Gender.Male;
        Analytics.SetUserGender(gender);
        var birthYear = 1991;
        Analytics.SetUserBirthYear(birthYear);
    }

    private void ShowAchievementButton()
    {
        var buttonRect = new Rect(500, 500, 150, 50);

        if (GUI.Button(buttonRect, "Achieve"))
        {
            CustomEvents();
            //Transactions();
            //Demographics();
        }
    }

    public string _zoneId = "1065731";
    public int _rewardQty = 250;

    private void OnGUI()
    {
        ShowAchievementButton();
        if (string.IsNullOrEmpty(_zoneId))
        {
            _zoneId = null;
        }

        var buttonRect = new Rect(100, 100, 150, 50);
        var buttonText = Advertisement.IsReady(_zoneId) ? "Show Ad" : "Waiting...";

        var options = new ShowOptions();
        options.resultCallback = HandleShowResult;

        if (GUI.Button(buttonRect, buttonText))
        {
            Advertisement.Show(_zoneId, options);
        }
    }

    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("Video completed. User rewarded " + _rewardQty + " credits.");
                break;
            case ShowResult.Skipped:
                Debug.LogWarning("Video was skipped.");
                break;
            case ShowResult.Failed:
                Debug.LogError("Video failed to show.");
                break;
        }
    }
}