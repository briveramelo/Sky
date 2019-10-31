using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BirdFactory))]
public class IncubatorEditor : Editor
{
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        serializedObject.Update();
        var birdTypes = Enum.GetValues(typeof(BirdType)).Cast<BirdType>().ToList();
        
        foreach (var birdType in birdTypes)
        {
            if (birdType == BirdType.All)
            {
                continue;
            }

            if (GUILayout.Button(birdType.ToString()))
            {
                ((BirdFactory) serializedObject.targetObject).CreateNextBird(birdType);
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}