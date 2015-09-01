using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PixelPerfectSprite : PixelPerfectObject {
	
	SpriteRenderer spriteRenderer {get { if (spriteRenderer_==null) {spriteRenderer_=GetComponent<SpriteRenderer>();} return spriteRenderer_;}}
	SpriteRenderer spriteRenderer_;
	
	new protected void LateUpdate() {
		base.LateUpdate();
		//spriteRenderer.sortingOrder=-parallaxLayer;
	}
	
	override protected float GetTransformScaleFactor() {
		float parallaxScale;
		if (pixelPerfectCamera!=null && !pixelPerfectCamera.normalCamera.orthographic) {
			parallaxScale=pixelPerfectCamera.GetParallaxLayerScale(parallaxLayer);
		} else {
			parallaxScale=1;
		}
		return spriteRenderer.sprite.pixelsPerUnit*PixelPerfect.worldPixelSize*pixelScale*parallaxScale;
	}
	
	override protected Vector2 GetPivotToCenter() {
		Vector2 normalizedPivot=new Vector2(spriteRenderer.sprite.rect.width*0.5f-spriteRenderer.sprite.pivot.x, spriteRenderer.sprite.rect.height*0.5f-spriteRenderer.sprite.pivot.y);
		return (new Vector2(normalizedPivot.x, normalizedPivot.y))*pixelScale*PixelPerfect.worldPixelSize;
	}
	
	override protected Vector2 GetCenterToOrigin() {
		return (new Vector2(-(float)spriteRenderer.sprite.rect.width*0.5f, (float)spriteRenderer.sprite.rect.height*0.5f))*pixelScale*PixelPerfect.worldPixelSize;
	}
}

#if UNITY_EDITOR
[CustomEditor(typeof(PixelPerfectSprite))]
public class PixelPerfectSpriteEditor : Editor {
	SerializedProperty pixelPerfectCamera;
	SerializedProperty pixelPerfectFitType;
	SerializedProperty parallaxLayer;
	SerializedProperty pixelScale;
	SerializedProperty runContinously;
	SerializedProperty useParentTransform;
	SerializedProperty displayGrid;
	
	override public void OnInspectorGUI() {
		FindSerializedProperties();
		DrawInspector();
	}
	
	void FindSerializedProperties() {
		pixelPerfectCamera	=serializedObject.FindProperty("pixelPerfectCamera");
		pixelPerfectFitType	=serializedObject.FindProperty("fitType");
		parallaxLayer		=serializedObject.FindProperty("parallaxLayer");
		pixelScale			=serializedObject.FindProperty("pixelScale");
		runContinously		=serializedObject.FindProperty("runContinously");
		useParentTransform	=serializedObject.FindProperty("useParentTransform");
		displayGrid			=serializedObject.FindProperty("displayGrid");
	}
	
	void DrawInspector() {
		EditorGUILayout.PropertyField(pixelPerfectFitType);
		EditorGUILayout.PropertyField(pixelScale);
		pixelScale.intValue=Mathf.Max(pixelScale.intValue, 0, pixelScale.intValue);
		DrawParallaxField();
		DrawButtons();
		
		serializedObject.ApplyModifiedProperties();
	}
	
	void DrawButtons() {
		EditorGUILayout.PrefixLabel("Options:");
		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		runContinously.boolValue=GUILayout.Toggle(runContinously.boolValue, "Run Continously", GUI.skin.FindStyle("Button"), GUILayout.Height(24), GUILayout.Width(150));
		useParentTransform.boolValue=GUILayout.Toggle(useParentTransform.boolValue, "Use Parent Transform", GUI.skin.FindStyle("Button"), GUILayout.Height(24), GUILayout.Width(150));
		displayGrid.boolValue=GUILayout.Toggle(displayGrid.boolValue, "Show Grid", GUI.skin.FindStyle("Button"), GUILayout.Height(24), GUILayout.Width(150));
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();
	}
	
	void DrawParallaxField() {
		PixelPerfectCamera targetCamera=((PixelPerfectCamera)pixelPerfectCamera.objectReferenceValue);
		if (targetCamera!=null && targetCamera.normalCamera!=null && !targetCamera.normalCamera.orthographic) {
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Parallax Layer");
			parallaxLayer.intValue=EditorGUILayout.IntSlider(parallaxLayer.intValue, 0, targetCamera.parallaxLayerCount);
			EditorGUILayout.EndHorizontal();
		} else {
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Parallax Layer");
			EditorGUILayout.LabelField("(Requires a camera set to 'Perspective')");
			EditorGUILayout.EndHorizontal();
		}
	}
}
#endif