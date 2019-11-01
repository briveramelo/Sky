using GenericFunctions;
using UnityEngine;

public static class PositionConverter
{
    #region Conversion values
    private static Vector2 _pixelCenter => Constants.ScreenSizePixels / 2; //pixels start at bottom left as (0,0)

    private static Vector2 _worldCenter => Constants.ScreenSizeWorldUnits / 2f; //world units start at bottom left as (-size.x/2,-size.y/2)

    //canvas units start at bottom left as (0,0)
    private static Vector2 GetCenterCanvasUnits(this Canvas canvas)
    {
        return canvas.GetSizeCanvasUnits() / 2f;
    }
    
    private static float _pixelsPerWorldUnit
    {
        get
        {
            var pixelsPerWorldUnitsByAxis = Constants.ScreenSizePixels / Constants.ScreenSizeWorldUnits;
            return (pixelsPerWorldUnitsByAxis.x + pixelsPerWorldUnitsByAxis.y) / 2f;
        }
    }
    private static float GetPixelsPerCanvasUnit(this Canvas canvas)
    {
        var pixelsPerCanvasUnitsByAxis = Constants.ScreenSizePixels / canvas.GetSizeCanvasUnits(); 
        return (pixelsPerCanvasUnitsByAxis.x + pixelsPerCanvasUnitsByAxis.y) / 2f;
    }
    private static float GetWorldUnitsPerCanvasUnit(this Canvas canvas)
    {
        var worldUnitsPerCanvasUnitsByAxis = Constants.ScreenSizeWorldUnits / canvas.GetSizeCanvasUnits(); 
        return (worldUnitsPerCanvasUnitsByAxis.x + worldUnitsPerCanvasUnitsByAxis.y) / 2f;
    }

    #endregion

    #region Pixel<->World
    public static float PixelsToWorldUnits(this float pixels)
    {
        return pixels / _pixelsPerWorldUnit;
    }
    public static Vector2 PixelsToWorldUnits(this Vector2 pixelPosition)
    {
        return (pixelPosition - _pixelCenter) / _pixelsPerWorldUnit;
    }
    public static Vector2 PixelsToWorldUnits(this Vector3 pixelPosition)
    {
        return ((Vector2)pixelPosition - _pixelCenter) / _pixelsPerWorldUnit;
    }

    public static float WorldUnitsToPixels(this float worldUnits)
    {
        return worldUnits * _pixelsPerWorldUnit;
    }
    public static Vector2 WorldUnitsToPixels(this Vector2 worldPosition)
    {
        return worldPosition * _pixelsPerWorldUnit + _pixelCenter;
    }
    public static Vector2 WorldUnitsToPixels(this Vector3 worldPosition)
    {
        return (Vector2)worldPosition * _pixelsPerWorldUnit + _pixelCenter;
    }
    #endregion

    #region Canvas<->World
    public static float CanvasUnitsToWorldUnits(this float canvasUnits, Canvas canvas)
    {
        return canvasUnits * canvas.GetWorldUnitsPerCanvasUnit();
    }
    public static Vector2 CanvasUnitsToWorldUnits(this Vector2 canvasPosition, Canvas canvas)
    {
        return (canvasPosition - canvas.GetCenterCanvasUnits()) * canvas.GetWorldUnitsPerCanvasUnit();
    }
    public static Vector2 CanvasUnitsToWorldUnits(this Vector3 canvasPosition, Canvas canvas)
    {
        return ((Vector2)canvasPosition - canvas.GetCenterCanvasUnits()) * canvas.GetWorldUnitsPerCanvasUnit();
    }

    public static float WorldUnitsToCanvasUnits(this float worldUnits, Canvas canvas)
    {
        return worldUnits / canvas.GetWorldUnitsPerCanvasUnit();
    }
    public static Vector2 WorldUnitsToCanvasUnits(this Vector2 worldPosition, Canvas canvas)
    {
        return (worldPosition + _worldCenter) / canvas.GetWorldUnitsPerCanvasUnit();
    }
    public static Vector2 WorldUnitsToCanvasUnits(this Vector3 worldPosition, Canvas canvas)
    {
        return ((Vector2)worldPosition + _worldCenter) / canvas.GetWorldUnitsPerCanvasUnit();
    }
    #endregion

    #region Pixel<->Canvas
    public static float PixelsToCanvasUnits(this float pixels, Canvas canvas)
    {
        return pixels / canvas.GetPixelsPerCanvasUnit();
    }
    public static Vector2 PixelsToCanvasUnits(this Vector2 pixelPosition, Canvas canvas)
    {
        return pixelPosition / canvas.GetPixelsPerCanvasUnit();
    }
    public static Vector2 PixelsToCanvasUnits(this Vector3 pixelPosition, Canvas canvas)
    {
        return pixelPosition / canvas.GetPixelsPerCanvasUnit();
    }

    public static float CanvasUnitsToPixels(this float canvasUnits, Canvas canvas)
    {
        return canvasUnits * canvas.GetPixelsPerCanvasUnit();
    }
    public static Vector2 CanvasUnitsToPixels(this Vector2 canvasPosition, Canvas canvas)
    {
        return canvasPosition * canvas.GetPixelsPerCanvasUnit();
    }
    public static Vector2 CanvasUnitsToPixels(this Vector3 canvasPosition, Canvas canvas)
    {
        return canvasPosition * canvas.GetPixelsPerCanvasUnit();
    }
    #endregion


    private static Vector2 GetSizeCanvasUnits(this Canvas canvas)
    {
        return (canvas.transform as RectTransform).sizeDelta / 2f;
    }
}