using GenericFunctions;
using UnityEngine;

public static class TouchToWorld
{
    private static readonly Vector2 _correctionPixels;
    private static readonly float _correctionPixelFactor;
    
    static TouchToWorld()
    {
        var isMacEditor = Application.platform == RuntimePlatform.OSXEditor;
        //bool isWindowsEditor = Application.platform == RuntimePlatform.WindowsEditor;
        if (isMacEditor)
        {
            _correctionPixels = new Vector2(Constants.ScreenDimensions.x / 2, -3 * Constants.ScreenDimensions.y / 2);
            _correctionPixelFactor = Constants.WorldDimensions.y * 2 / Constants.ScreenDimensions.y;
        }
        else
        {
            //correctionPixels = -Constants.ScreenDimensions / 2;
            _correctionPixels = new Vector2(-Constants.ScreenDimensions.x / 2, -1 * Constants.ScreenDimensions.y / 2);
            _correctionPixelFactor = .01f;
        }
    }

    public static Vector2 GetWorldPosition(Vector2 position)
    {
        return (position + _correctionPixels) * _correctionPixelFactor;
    }
}