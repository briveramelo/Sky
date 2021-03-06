﻿using System.Collections;
using GenericFunctions;
using UnityEngine;

public class Pauser : Selector
{
    public static bool Paused { get; private set; }
    
    [SerializeField] protected Canvas _parentCanvas;
    [SerializeField] private GameObject _joystick, _pauseMenu;
    [SerializeField] private float _clickRadiusCanvUnits;

    private const string _pauseName = nameof(Pauser);

    private Vector2 _position 
    {
        get
        {
            var spot = (transform as RectTransform).sizeDelta / 2 * _parentCanvas.transform.lossyScale;
            return (Vector2) transform.position + new Vector2(spot.x, -spot.y);
        }
    }
    private int _fingerId = Constants.UnusedFingerId;

    private void Start()
    {
        OrderedTouchEventRegistry.Instance.OnTouchWorldBegin(typeof(Pauser), OnTouchWorldBegin, true);
    }

    private void OnDestroy()
    {
        if (TouchInputManager.Instance == null)
        {
            return;
        }
        OrderedTouchEventRegistry.Instance.OnTouchWorldBegin(typeof(Pauser), OnTouchWorldBegin, false);
    }

    //used to eat inputs and prevent jai from throwing a spear accidentally
    private void OnTouchWorldBegin(int fingerId, Vector2 touchWorldPosition)
    {
        var touchCanvasPosition = touchWorldPosition.WorldPositionToCanvasPosition(_parentCanvas);
        var localPosition = (touchCanvasPosition - _position) / _parentCanvas.transform.lossyScale;
        bool isCloseEnough = Vector2.Distance(localPosition, Vector2.zero) < _clickRadiusCanvUnits;
        if (!gameObject.activeInHierarchy || !isCloseEnough || !TouchInputManager.Instance.TryClaimFingerId(fingerId, _pauseName))
        {
            return;
        }

        _fingerId = fingerId;
        StartCoroutine(OnClickRoutine());
    }

    protected override void OnClick()
    {
        if (!Paused)
        {
            AudioManager.PlayAudio(_audioType);
            Pause();
        }
    }

    protected override IEnumerator OnClickRoutine()
    {
        yield return null;
        TouchInputManager.Instance.ReleaseFingerId(_fingerId, _pauseName);
        _fingerId = Constants.UnusedFingerId;
    }

    private void Pause()
    {
        Paused = true;
        GameClock.TimeScale = 0f;
        ShowPauseMenu(true);
    }

    public void UnPause()
    {
        Paused = false;
        GameClock.TimeScale = 1f;
        ShowPauseMenu(false);
    }

    public void ResetPause()
    {
        Paused = false;
        GameClock.TimeScale = 1f;
    }

    private void ShowPauseMenu(bool setActive)
    {
        _pauseMenu.SetActive(setActive);
        
        _joystick.SetActive(!setActive);
        gameObject.SetActive(!setActive);
    }
}