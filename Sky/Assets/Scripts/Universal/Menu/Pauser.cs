using UnityEngine;
using GenericFunctions;

public class Pauser : MonoBehaviour, IBegin {

	static bool paused = false; public static bool Paused { get { return paused; } }
    static Vector2 pauseSpot;   public static Vector2 PauseSpot {get { return pauseSpot; } }
	public static readonly float pauseRadius = 0.5f;

    [SerializeField] AudioSource pauseSounds;
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
        pauseSounds.PlayOneShot(pause);
        Time.timeScale = 0f;
        ShowPauseMenu(true);
    }

    public void UnPause() {
        paused = false;
        pauseSounds.PlayOneShot(unPause);
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
