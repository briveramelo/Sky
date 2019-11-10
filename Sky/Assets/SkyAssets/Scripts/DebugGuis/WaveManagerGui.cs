using System;
using System.Linq;
using UnityEngine;

public class WaveManagerGui : SubDebugGui
{
    public override bool AllDependenciesPresent => _waveManager && BirdFactory.Instance;
    private WaveManager _waveManager => FindObjectOfType<WaveManager>();

    protected override void OnGuiEnabled()
    {
        base.OnGuiEnabled();
        var waveNames = Enum.GetValues(typeof(WaveName)).Cast<WaveName>();
        foreach (var waveName in waveNames)
        {
            if (GUILayout.Button(waveName.ToString(), ScreenSpace.LeftAlignedButtonStyle))
            {
                _waveManager.KillRunningWaves();
                BirdFactory.Instance.KillAllLivingBirds();
                _waveManager.RunWave(waveName);
                break;
            }
        }
    }
}
