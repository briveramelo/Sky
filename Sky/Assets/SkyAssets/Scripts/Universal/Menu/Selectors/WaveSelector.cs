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

    [SerializeField] private WaveType _myWaveType;

    protected override Vector2 TouchSpot => MenuInputHandler.TouchSpot;

    protected override IEnumerator PressButton() {
        AudioManager.PlayAudio(_buttonPress);
        //inputManager.IsFrozen = true;
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene((int)_myWaveType);
    }
}
