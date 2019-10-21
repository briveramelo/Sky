﻿using System.Collections;
using UnityEngine;

public class Pauser : Selector
{
    public static bool Paused { get; private set; }

    [SerializeField] private GameObject _joystick, _pauseMenu;

    private const string _pauseName = nameof(Pauser);
    private int _fingerId;

    protected override void Awake()
    {
        base.Awake();
        TouchInputManager.Instance.OnTouchBegin += OnTouchBegin;
    }

    private void OnDestroy()
    {
        TouchInputManager.Instance.OnTouchBegin -= OnTouchBegin;
    }

    private void OnTouchBegin(int fingerId, Vector2 fingerPosition)
    {
        bool isCloseEnough = Vector2.Distance(fingerPosition, TouchToWorld.GetWorldPosition(transform.position)) < 1f;
        if (!isCloseEnough || !TouchInputManager.Instance.TryClaimFingerId(fingerId, _pauseName))
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
            AudioManager.PlayAudio(_buttonPress);
            Pause();
        }
    }

    protected override IEnumerator OnClickRoutine()
    {
        yield return null;
        TouchInputManager.Instance.ReleaseFingerId(_fingerId, _pauseName);
        _fingerId = -1;
    }

    private void Pause()
    {
        Paused = true;
        Time.timeScale = 0f;
        ShowPauseMenu(true);
    }

    public void UnPause()
    {
        Paused = false;
        Time.timeScale = 1f;
        ShowPauseMenu(false);
    }

    public void ResetPause()
    {
        Paused = false;
        Time.timeScale = 1f;
    }

    private void ShowPauseMenu(bool setActive)
    {
        _pauseMenu.SetActive(setActive);
        
        _joystick.SetActive(!setActive);
        gameObject.SetActive(!setActive);
    }
}