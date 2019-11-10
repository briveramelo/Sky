using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameCamera))]
public class GameCameraEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var test = ScreenSpace.ScreenSizePixels;
        GUILayout.Label($"Resolution: ({test.x}, {test.y})");
        GUILayout.Label($"Aspect x/y:{test.x / test.y}");
        serializedObject.ApplyModifiedProperties();
    }
}