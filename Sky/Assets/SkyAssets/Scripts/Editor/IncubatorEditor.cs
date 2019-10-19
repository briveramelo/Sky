using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Incubator))]
public class IncubatorEditor : Editor
{
    public override void OnInspectorGUI() {
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
                ((Incubator) serializedObject.targetObject).SpawnNextBird(birdType);
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}