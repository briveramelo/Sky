﻿using UnityEngine;

public class Pauser : MonoBehaviour, IBegin {

    public static bool Paused => paused;
    static bool paused;
    public static Vector2 PauseSpot => pauseSpot;
    static Vector2 pauseSpot;   
	public static readonly float pauseRadius = 0.5f;

	[SerializeField] AudioClip pause, unPause;
    [SerializeField] GameObject joystick, pauseMenu, pauseButtonCanvas;

	void Awake(){
        pauseSpot = transform.position;
	}

	void IBegin.OnTouchBegin(int fingerID){
        if (!paused) {
            float distFromStick = Vector2.Distance(InputManager.touchSpot,pauseSpot);
		    if (distFromStick < pauseRadius) {
                Pause();
            }
        }
    }

    void Pause() {
        paused = true;
        AudioManager.PlayAudio(pause);
        Time.timeScale = 0f;
        ShowPauseMenu(true);
    }

    public void UnPause() {
        paused = false;
        AudioManager.PlayAudio(unPause);
        Time.timeScale = 1f;
        ShowPauseMenu(false);
    }

    public void ResetPause() {
        paused = false;
        Time.timeScale = 1f;
    }

    void ShowPauseMenu(bool setActive) {
        pauseMenu.SetActive(setActive);
        joystick.SetActive(!setActive);
        pauseButtonCanvas.SetActive(!setActive);
        gameObject.SetActive(!setActive);
    }
}
