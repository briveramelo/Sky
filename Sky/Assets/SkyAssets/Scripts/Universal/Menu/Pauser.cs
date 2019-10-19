using UnityEngine;

public class Pauser : MonoBehaviour, IBegin {

    public static bool Paused => _paused;
    private static bool _paused;
    public static Vector2 PauseSpot => _pauseSpot;
    private static Vector2 _pauseSpot;   
	public static readonly float PauseRadius = 0.5f;

	[SerializeField] private AudioClip _pause, _unPause;
    [SerializeField] private GameObject _joystick, _pauseMenu, _pauseButtonCanvas;

    private void Awake(){
        _pauseSpot = transform.position;
	}

	void IBegin.OnTouchBegin(int fingerId){
        if (!_paused) {
            float distFromStick = Vector2.Distance(InputManager.TouchSpot,_pauseSpot);
		    if (distFromStick < PauseRadius) {
                Pause();
            }
        }
    }

    private void Pause() {
        _paused = true;
        AudioManager.PlayAudio(_pause);
        Time.timeScale = 0f;
        ShowPauseMenu(true);
    }

    public void UnPause() {
        _paused = false;
        AudioManager.PlayAudio(_unPause);
        Time.timeScale = 1f;
        ShowPauseMenu(false);
    }

    public void ResetPause() {
        _paused = false;
        Time.timeScale = 1f;
    }

    private void ShowPauseMenu(bool setActive) {
        _pauseMenu.SetActive(setActive);
        _joystick.SetActive(!setActive);
        _pauseButtonCanvas.SetActive(!setActive);
        gameObject.SetActive(!setActive);
    }
}
