﻿using UnityEditor;
using UnityEngine;

/*
 * [url]http://forum.unity3d.com/threads/24311-Replace-game-object-with-prefab/page2[/url]
 * */

public class ReplaceGameObjects : ScriptableWizard {

	public GameObject _useGameObject;

	[MenuItem("Custom/Replace GameObjects")]
	private static void CreateWizard(){
		ScriptableWizard.DisplayWizard("Replace GameObjects", typeof(ReplaceGameObjects), "Replace");
	}

	private void OnWizardCreate(){
		foreach (Transform t in Selection.transforms){
			GameObject newObject = (GameObject) PrefabUtility.InstantiatePrefab(_useGameObject);
			Undo.RegisterCreatedObjectUndo(newObject, "created prefab");
			Transform newT = newObject.transform;

			newT.position = t.position;
			newT.rotation = t.rotation;
			newT.localScale = t.localScale;
			newT.SetParent(t.parent);
		}

		foreach (GameObject go in Selection.gameObjects){
			Undo.DestroyObjectImmediate(go);
		}
	}
}
