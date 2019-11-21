using System.Collections;
using BRM.EventBrokers;
using BRM.EventBrokers.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private IBrokerEvents _eventBroker = new StaticEventBroker();
    private void Start()
    {
        Application.targetFrameRate = 60;
        _eventBroker.Subscribe<BasketDeathData>(OnBasketDeath);
    }

    private void OnBasketDeath(BasketDeathData data)
    {
        if (data.NumContinuesRemaining < 0)
        {
            StartCoroutine(DisplayGameEndAndLoadMainMenu());
        }
    }

    private IEnumerator DisplayGameEndAndLoadMainMenu()
    {
        ScoreSheet.Reporter.ReportScores();
        yield return StartCoroutine(ScoreSheet.Reporter.DisplayTotal());
        SceneManager.LoadScene(Scenes.Menu);
    }
}