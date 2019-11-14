using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EffectFactory))]
public class EffectFactoryEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (!Application.isPlaying)
        {
            return;
        }

        var pooPrefab = (serializedObject.targetObject as EffectFactory).PooEffectPrefab;

        if (GUILayout.Button("Splat poo"))
        {
            Instantiate(pooPrefab, Vector2.zero, Quaternion.identity);
        }
    }
}
