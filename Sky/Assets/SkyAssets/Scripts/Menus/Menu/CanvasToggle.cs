using UnityEngine;

public class CanvasToggle : MonoBehaviour, IDisplayable
{
    public void ToggleDisplay(bool show)
    {
        gameObject.SetActive(show);
    }
}
