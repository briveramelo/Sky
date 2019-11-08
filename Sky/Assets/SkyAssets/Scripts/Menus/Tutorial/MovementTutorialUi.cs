using System;
using System.Collections;
using BRM.EventBrokers;
using BRM.EventBrokers.Interfaces;
using UnityEngine;

public class MovementTutorialUi : MonoBehaviour
{
    [SerializeField] private GameObject _joystickHelp, _swipeHelp;
    
    private bool _hasWeapon;
    private IBrokerEvents _eventBroker = new StaticEventBroker();
    
    private void Awake()
    {
        _eventBroker.Subscribe<WeaponGrabbedData>(GrabbedWeapon);
    }

    private void OnDestroy()
    {
        _eventBroker.Unsubscribe<WeaponGrabbedData>(GrabbedWeapon);
    }

    public void GrabbedWeapon(WeaponGrabbedData data)
    {
        _hasWeapon = true;
    }

    public IEnumerator AnimateStoryStart()
    {
        if (!_hasWeapon)
        {
            yield return new WaitForSeconds(1f);
            yield return ShowHelpTip(isActive => _joystickHelp.SetActive(isActive));
            while (!_hasWeapon)
            {
                yield return null;
            }

            yield return ShowHelpTip(isActive => _swipeHelp.SetActive(isActive));
        }
    }

    private IEnumerator ShowHelpTip(System.Action<bool> lambda)
    {
        lambda(true);
        yield return new WaitForSeconds(5.5f);
        lambda(false);
    }
}