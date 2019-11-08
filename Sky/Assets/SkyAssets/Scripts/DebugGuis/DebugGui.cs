using UnityEngine;

public abstract class DebugGui : MonoBehaviour
{
    public virtual bool CanGuiDisplay
    {
        get => enabled;
        set => enabled = value;
    }

    protected abstract void OnGuiEnabled();
    
    private void OnGUI()
    {
        if (!CanGuiDisplay)
        {
            return;
        }
        OnGuiEnabled();
    }
}