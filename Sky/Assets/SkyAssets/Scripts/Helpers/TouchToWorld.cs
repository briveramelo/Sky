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
            _correctionPixels = new Vector2(Constants.ScreenSize.x / 2, -3 * Constants.ScreenSize.y / 2);
            _correctionPixelFactor = Constants.WorldSize.y * 2 / Constants.ScreenSize.y;
        }
        else
        {
            //correctionPixels = -Constants.ScreenSize / 2;
            _correctionPixels = new Vector2(-Constants.ScreenSize.x / 2, -1 * Constants.ScreenSize.y / 2);
            _correctionPixelFactor = .01f;
        }
    }

    public static Vector2 GetWorldPosition(Vector2 pixelPosition)
    {
        return (pixelPosition + _correctionPixels) * _correctionPixelFactor;
    }
}