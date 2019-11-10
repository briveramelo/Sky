using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ParallaxParent))]
public class ParallaxParentEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var myTarget = serializedObject.targetObject as ParallaxParent;
        if (myTarget.Children == null)
        {
            return;
        }

        serializedObject.Update();
        myTarget.AssignMoveSpeeds();
        for (int i = 0; i < myTarget.Children.Count; i++)
        {
            var moveSpeed = myTarget.Children[i].MoveSpeed;
            GUILayout.Label($"{i}. {moveSpeed}");
        }
        serializedObject.ApplyModifiedProperties();
    }
}