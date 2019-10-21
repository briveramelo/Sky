using UnityEngine;

public class Continuer : MonoBehaviour
{
    [SerializeField] private GameObject _continueMenu, _joystick, _pauseButtonGroup;
    [SerializeField] private TouchInputManager _inputManager;

    public void DisplayContinueMenu(bool show)
    {
        Time.timeScale = show ? 0f : 1f;
        ((IFreezable) _inputManager).IsFrozen = show;
        _continueMenu.SetActive(show);
        _joystick.SetActive(!show);
        _pauseButtonGroup.SetActive(!show);
    }
}