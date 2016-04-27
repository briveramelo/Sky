﻿using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(EagleFriends)), CanEditMultipleObjects]
public class EagleFriends_Editor : Editor {

    public override void OnInspectorGUI() {
		serializedObject.Update();
		serializedObject.ApplyModifiedProperties();
	}
}
	

