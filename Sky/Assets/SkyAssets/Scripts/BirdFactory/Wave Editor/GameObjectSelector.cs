using UnityEngine;

namespace BRM.Sky.WaveEditor
{
    public abstract class GameObjectSelector
    {
        public abstract void Select(GameObject go);
    }

    public class EditorSelector : GameObjectSelector
    {
        public override void Select(GameObject go)
        {
            #if UNITY_EDITOR
            UnityEditor.Selection.activeGameObject = go;
            #endif
        }
    }

    public class GameplaySelector : GameObjectSelector
    {
        public override void Select(GameObject go)
        {
            var selectable = go.GetComponent<ISelectable>();
            selectable?.Select(true);
        }
    }
}