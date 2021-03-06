using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WaveManager))]
public class WaveManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (!Application.isPlaying)
        {
            return;
        }

        serializedObject.Update();
        var waveNames = Enum.GetValues(typeof(WaveName)).Cast<WaveName>();
        foreach (var waveName in waveNames)
        {
            if (GUILayout.Button(waveName.ToString()))
            {
                var waveManager = serializedObject.targetObject as WaveManager;
                waveManager.KillRunningWaves();
                BirdFactory.Instance.KillAllLivingBirds();
                waveManager.RunWave(waveName);
                break;
            }
        }

        
        serializedObject.ApplyModifiedProperties();
    }
}