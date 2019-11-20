using System.Collections.Generic;
using UnityEngine;

public enum WaveName
{
    Intro = 0,
    Pigeon = 1,
    Duck = 2,
    Pigeuck = 3,
    Seagull = 4,
    Pelican = 5,
    Shoebill = 6,
    Bat = 7,
    Eagle = 8,
    Complete = 9,
    Endless = 10,
    Data=11,
}
public static class WaveLabels
{
    private static Dictionary<WaveName, string> _waveSubtitles = new Dictionary<WaveName, string>()
    {
        {WaveName.Intro, "Rescue your son!"},
        {WaveName.Pigeon, "Clear out the sky rats"},
        {WaveName.Duck, "Carve through these meatsacks"},
        {WaveName.Pigeuck, "Get bloody"},
        {WaveName.Seagull, "Prepare for a shitstorm"},
        {WaveName.Pelican, "Watch your head"},
        {WaveName.Shoebill, "Hold on to your butts!"},
        {WaveName.Bat, "Tread lightly"},
        {WaveName.Eagle, "Make him pay..."},
        {WaveName.Endless, "Indulge yourself"}
    };

    public static string GetWaveSubtitle(WaveName waveName)
    {
        string subtitle = "";
        if (!_waveSubtitles.TryGetValue(waveName, out subtitle))
        {
            Debug.LogError("No subtitle found for " + waveName);
        }

        return subtitle;
    }
}