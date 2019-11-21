using System;
using BRM.EventBrokers;
using BRM.EventBrokers.Interfaces;
using UnityEngine;

public class Continuer : MonoBehaviour
{
    [SerializeField] private GameObject _continueMenu, _joystick, _pauseButtonGroup;
    [SerializeField] private TouchInputManager _inputManager;

    private IBrokerEvents _eventBroker = new StaticEventBroker();

    private void Awake()
    {
        _eventBroker.Subscribe<BasketDeathData>(OnBasketDeath);
    }

    private void OnDestroy()
    {
        _eventBroker.Unsubscribe<BasketDeathData>(OnBasketDeath);
    }

    private void OnBasketDeath(BasketDeathData data)
    {
        if (data.NumContinuesRemaining < 0)
        {
            return;
        }

        DisplayContinueMenu(true);
    }

    public void DisplayContinueMenu(bool show)
    {
        GameClock.TimeScale = show ? 0f : 1f;
        ((IFreezable) _inputManager).IsFrozen = show;
        _continueMenu.SetActive(show);
        _joystick.SetActive(!show);
        _pauseButtonGroup.SetActive(!show);
    }
}