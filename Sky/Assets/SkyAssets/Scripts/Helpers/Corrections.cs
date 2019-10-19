using UnityEngine;
using GenericFunctions;

public struct Corrections
{
    public float CorrectionPixelFactor;
    public Vector2 CorrectionPixels;
    public Corrections(bool whatever)
    {
        bool isMacEditor = Application.platform == RuntimePlatform.OSXEditor;
        //bool isWindowsEditor = Application.platform == RuntimePlatform.WindowsEditor;
        if (isMacEditor)
        {
            CorrectionPixels = new Vector2(Constants.ScreenDimensions.x / 2, (-3 * Constants.ScreenDimensions.y / 2));
            CorrectionPixelFactor = Constants.WorldDimensions.y * 2 / Constants.ScreenDimensions.y;
        }
        else
        {
            //correctionPixels = -Constants.ScreenDimensions / 2;
            CorrectionPixels = new Vector2(-Constants.ScreenDimensions.x / 2, (-1 * Constants.ScreenDimensions.y / 2));
            CorrectionPixelFactor = .01f;
        }
    }
}
