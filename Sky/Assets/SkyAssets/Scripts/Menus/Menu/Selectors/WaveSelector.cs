using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public static class Scenes
{
    public const string Menu = "Menu";
    public const string Intro = "Intro";
    public const string Story = "Story";
    public const string Endless = "Endless";
    public const string Scores = "Scores";

    private static readonly List<string> MenuScenes = new List<string>
    {
        Menu,
        Scores
    };
    private static readonly List<string> GameplayScenes = new List<string>
    {
        Story,
        Endless,
    };

    public static bool IsMenu(string sceneName) => MenuScenes.Contains(sceneName);
    public static bool IsGameplay(string sceneName) => GameplayScenes.Contains(sceneName);
}

public enum WaveType
{
    Story = 2,
    Endless = 3
}

public class WaveSelector : Selector
{
    [SerializeField] private WaveType _myWaveType;

    protected override IEnumerator OnClickRoutine()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene((int) _myWaveType);
    }
}