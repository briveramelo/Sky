using UnityEditor;

[CustomEditor(typeof(EagleFriends)), CanEditMultipleObjects]
public class EagleFriendsEditor : Editor {

    public override void OnInspectorGUI() {
		serializedObject.Update();
		serializedObject.ApplyModifiedProperties();
	}
}
	

