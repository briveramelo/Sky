using UnityEngine;

public class Pauser : MonoBehaviour, IBegin {

    public static bool Paused => paused;
    private static bool paused;
    public static Vector2 PauseSpot => pauseSpot;
    private static Vector2 pauseSpot;   
	public static readonly float pauseRadius = 0.5f;

	[SerializeField] private AudioClip pause, unPause;
    [SerializeField] private GameObject joystick, pauseMenu, pauseButtonCanvas;

    private void Awake(){
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

    private void Pause() {
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

    private void ShowPauseMenu(bool setActive) {
        pauseMenu.SetActive(setActive);
        joystick.SetActive(!setActive);
        pauseButtonCanvas.SetActive(!setActive);
        gameObject.SetActive(!setActive);
    }
}
