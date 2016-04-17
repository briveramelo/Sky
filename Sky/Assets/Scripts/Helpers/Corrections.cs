using System.Collections;
using UnityEngine;
using GenericFunctions;

public struct Corrections
{
    public float correctionPixelFactor;
    public Vector2 correctionPixels;
    public Corrections(bool whatever)
    {
        bool isMacEditor = Application.platform == RuntimePlatform.OSXEditor;
        bool isWindowsEditor = Application.platform == RuntimePlatform.WindowsEditor;
        if (isMacEditor)
        {
            correctionPixels = new Vector2(Constants.ScreenDimensions.x / 2, (-3 * Constants.ScreenDimensions.y / 2));
            correctionPixelFactor = Constants.WorldDimensions.y * 2 / Constants.ScreenDimensions.y;
        }
        else
        {
            correctionPixels = -Constants.ScreenDimensions / 2;
            correctionPixelFactor = .01f;
        }
    }
}
