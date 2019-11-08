using UnityEngine;

public abstract class DebugGui : MonoBehaviour
{
    [HideInInspector] public bool CanGuiDisplay;
    
    private const int _numSimulTouches = 4;

    protected abstract void OnGuiEnabled();

    private void Update()
    {
        var touchPassed = (Input.touchCount == _numSimulTouches && Input.GetTouch(_numSimulTouches - 1).phase == TouchPhase.Began);
        var mousePressed = Input.GetMouseButtonDown(1);//right click
        if (touchPassed || mousePressed)
        {
            CanGuiDisplay = !CanGuiDisplay;
        }
    }
    
    private void OnGUI()
    {
        if (!CanGuiDisplay)
        {
            return;
        }
        OnGuiEnabled();
    }
}