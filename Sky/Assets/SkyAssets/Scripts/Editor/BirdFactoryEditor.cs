using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BirdFactory))]
public class BirdFactoryEditor : Editor
{
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        if (!Application.isPlaying)
        {
            return;
        }

        serializedObject.Update();
        var birdTypes = EnumHelpers.GetAll<BirdType>().OrderBy(item => item.ToString());
        
        foreach (var birdType in birdTypes)
        {
            if (birdType == BirdType.All)
            {
                continue;
            }
            //GUIStyle style = new GUIStyle("button");
            // or
            GUIStyle leftAlignedButtonStyle = new GUIStyle(GUI.skin.button) {alignment = TextAnchor.MiddleLeft};

            if (GUILayout.Button(birdType.ToString(), leftAlignedButtonStyle))
            {
                ((BirdFactory) serializedObject.targetObject).CreateNextBird(birdType);
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}