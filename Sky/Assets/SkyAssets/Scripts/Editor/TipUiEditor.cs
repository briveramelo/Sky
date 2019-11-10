using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TipUi))]
public class TipUiEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (!Application.isPlaying)
        {
            return;
        }

        serializedObject.Update();
        var tips = Enum.GetValues(typeof(Tip)).Cast<Tip>();
        foreach (var tip in tips)
        {
            if (GUILayout.Button(tip.ToString()))
            {
                var ui = serializedObject.targetObject as TipUi;
                ui.StopAllCoroutines();
                ui.DisplayTip(tip);
                break;
            }
        }
        serializedObject.ApplyModifiedProperties();
    }
}
