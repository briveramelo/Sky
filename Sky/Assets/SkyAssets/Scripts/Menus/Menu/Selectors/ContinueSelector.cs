using UnityEngine;
using System.Collections;
using BRM.EventBrokers;
using BRM.EventBrokers.Interfaces;

public class ContinueSelector : Selector
{
    [SerializeField] private Continuer _continuer;

    private IPublishEvents _eventPublisher = new StaticEventBroker();
    private int _numContinuesRemaining = 1;
    
    protected override IEnumerator OnClickRoutine()
    {
        yield return StartCoroutine(AdDisplayer.DisplayAd());
        
        GameClock.TimeScale = 1f;
        _continuer.DisplayContinueMenu(false);
        _numContinuesRemaining--;
        _eventPublisher.Publish(new ContinueData{NumContinuesRemaining = _numContinuesRemaining});
    }
}