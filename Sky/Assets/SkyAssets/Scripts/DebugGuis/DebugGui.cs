using UnityEngine;

public abstract class DebugGui : MonoBehaviour
{
    public virtual bool CanGuiDisplay
    {
        get => enabled;
        set => enabled = value;
    }

    protected abstract void OnGuiEnabled();
    private Vector2 _scrollPosition;
    private void OnGUI()
    {
        if (!CanGuiDisplay)
        {
            return;
        }

        _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
        OnGuiEnabled();
        GUILayout.EndScrollView();
    }
}