using UnityEngine;

public class Continuer : MonoBehaviour {

    [SerializeField] GameObject continueMenu, joystick, pauseButtonGroup;
    [SerializeField] InputManager inputManager;

	public void DisplayContinueMenu(bool show) {
        Time.timeScale = show? 0f : 1f;
        ((IFreezable)inputManager).IsFrozen = show;
        continueMenu.SetActive(show);
        joystick.SetActive(!show);
        pauseButtonGroup.SetActive(!show);
    }
}
