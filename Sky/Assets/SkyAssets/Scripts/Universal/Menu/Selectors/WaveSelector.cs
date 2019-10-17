using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public static class Scenes
{
    public const string Menu = "Menu";
    public const string Intro = "Intro";
    public const string Story = "Story";
    public const string Endless = "Endless";
    public const string Scores = "Scores";
}
public enum WaveType {
    Story = 2,
    Endless = 3
}

public class WaveSelector : Selector {

    [SerializeField] private WaveType MyWaveType;
    private IFreezable inputManager;

    protected override Vector2 TouchSpot => MenuInputHandler.touchSpot;

    private void Awake() {
        inputManager = FindObjectOfType<MenuInputHandler>().GetComponent<IFreezable>();
    }

    protected override IEnumerator PressButton() {
        AudioManager.PlayAudio(buttonPress);
        //inputManager.IsFrozen = true;
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene((int)MyWaveType);
    }
}
