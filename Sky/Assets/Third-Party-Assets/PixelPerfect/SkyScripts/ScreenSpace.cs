#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using UnityEngine;
using Object = UnityEngine.Object;

public static class ScreenSpace
{
    public static Vector2 ScreenSizePixels
    {
        get
        {
        #if UNITY_EDITOR
            string[] res = UnityStats.screenRes.Split('x');
            return new Vector2(int.Parse(res[0]), int.Parse(res[1]));
        #endif
            return new Vector2(Screen.width, Screen.height);
        }
    }

    public static Vector2 ScreenSizeWorldUnits => WorldEdge * 2f;
    public static Vector2 ScreenSizeCanvasUnits(Canvas canvas) => canvas.GetSizeCanvasUnits();
    public static Vector2 WorldEdge
    {
        get
        {
            var pixelCam = _pixelCam.Value;
            var cam = pixelCam.normalCamera;
            var height = cam.orthographicSize / _pixelCam.Value.cameraZoom;
            var width = height * cam.aspect;
            var size = new Vector2(width, height);
            return size;
        }
    }

    public static float ScreenZoom => _pixelCam.Value.cameraZoom;

    private static Lazy<PixelPerfectCamera> _pixelCam = new Lazy<PixelPerfectCamera>(Object.FindObjectOfType<PixelPerfectCamera>);
    
    public static GUIStyle LeftAlignedButtonStyle => new GUIStyle(GUI.skin.button)
    {
        alignment = TextAnchor.MiddleLeft,
        fontSize = ScreenAdjustedFontSize,
        wordWrap = false,
        padding = _padding
    };
    public static GUIStyle RightAlignedButtonStyle => new GUIStyle(GUI.skin.button)
    {
        alignment = TextAnchor.MiddleRight,
        fontSize = ScreenAdjustedFontSize,
        padding = _padding
    };
    public static GUIStyle CenterAlignedButtonStyle => new GUIStyle(GUI.skin.button)
    {
        alignment = TextAnchor.MiddleCenter,
        fontSize = ScreenAdjustedFontSize,
        padding = _padding
    };

    public static GUIStyle CenterAlignedLabelStyle => new GUIStyle(GUI.skin.label)
    {
        fontSize = ScreenAdjustedFontSize,
        alignment = TextAnchor.MiddleCenter,
        padding = _padding,
        wordWrap = false
    };
    public static GUIStyle LeftAlignedLabelStyle => new GUIStyle(GUI.skin.label)
    {
        fontSize = ScreenAdjustedFontSize,
        alignment = TextAnchor.MiddleLeft,
        padding = _padding
    };


    public static int ScreenAdjustedFontSize => (int) ( (1 / 39f) * ScreenSizePixels.magnitude);
    private static RectOffset _padding => new RectOffset(Mathf.CeilToInt(ScreenAdjustedFontSize / 3f), ScreenAdjustedFontSize, 1, 1);
}