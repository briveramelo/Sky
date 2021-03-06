﻿using UnityEngine;

public static class ScreenSpaceConverter
{
    #region Conversion values
    private static Vector2 _pixelCenter => ScreenSpace.ScreenSizePixels / 2; //pixels start at bottom left as (0,0)
    private static Vector2 _viewportCenter => Vector2.one * 0.5f;
    private static Vector2 GetCenterCanvasUnits(this Canvas canvas) => canvas.transform.position;
    
    //canvas units start at bottom left as (0,0)
    private static Vector2 _pixelsPerWorldUnit => ScreenSpace.ScreenSizePixels / ScreenSpace.ScreenSizeWorldUnits;
    private static Vector2 GetWorldUnitsPerCanvasUnit(this Canvas canvas) => ScreenSpace.ScreenSizeWorldUnits / canvas.GetSizeCanvasUnits();
    private static Vector2 GetPixelsPerCanvasUnit(this Canvas canvas) => ScreenSpace.ScreenSizePixels / canvas.GetSizeCanvasUnits();
    public static Vector2 GetSizeCanvasUnits(this Canvas canvas)
    {
        var transform = (RectTransform) canvas.transform;
        return transform.sizeDelta * transform.lossyScale;
    }

    #endregion
    
    #region Pixel<->Viewport

    public static Vector2 PixelsToViewport(this Vector2 positionPixels)
    {
        return positionPixels / ScreenSpace.ScreenSizePixels;
    }
    public static Vector2 ViewportToPixels(this Vector2 positionViewport)
    {
        return positionViewport * ScreenSpace.ScreenSizePixels;
    }
    

    #endregion
    
    #region World<->Viewport

    public static Vector2 WorldToViewportPosition(this Vector2 positionWorld)
    {
        return positionWorld / ScreenSpace.ScreenSizeWorldUnits + _viewportCenter;
    }
    public static Vector2 WorldToViewportPosition(this Vector3 positionWorld)
    {
        return positionWorld / ScreenSpace.ScreenSizeWorldUnits + _viewportCenter;
    }
    public static Vector2 ViewportToWorldPosition(this Vector2 positionViewport)
    {
        return (positionViewport - _viewportCenter) * ScreenSpace.ScreenSizeWorldUnits;
    }
    public static Vector2 ViewportToWorldPosition(this Vector3 positionViewport)
    {
        return ((Vector2)positionViewport - _viewportCenter) * ScreenSpace.ScreenSizeWorldUnits;
    }
    public static Vector2 ViewportToWorldUnits(this Vector2 viewportUnits)
    {
        return viewportUnits * ScreenSpace.ScreenSizeWorldUnits;
    }
    public static Vector2 WorldToViewportUnits(this Vector2 worldUnits)
    {
        return worldUnits / ScreenSpace.ScreenSizeWorldUnits;
    }
    

    #endregion

    #region Pixel<->World
    public static Vector2 PixelsToWorldUnits(this Vector2 pixels)
    {
        return pixels / _pixelsPerWorldUnit;
    }
    public static Vector2 PixelsToWorldPosition(this Vector2 pixelPosition)
    {
        return (pixelPosition - _pixelCenter) / _pixelsPerWorldUnit;
    }
    public static Vector2 PixelsToWorldPosition(this Vector3 pixelPosition)
    {
        return ((Vector2)pixelPosition - _pixelCenter) / _pixelsPerWorldUnit;
    }

    public static Vector2 WorldSizeToPixels(this Vector2 worldSize)
    {
        return worldSize * _pixelsPerWorldUnit;
    }
    public static Vector2 WorldPositionToPixels(this Vector2 worldPosition)
    {
        return worldPosition * _pixelsPerWorldUnit + _pixelCenter;
    }
    public static Vector2 WorldPositionToPixels(this Vector3 worldPosition)
    {
        return worldPosition * _pixelsPerWorldUnit + _pixelCenter;
    }
    #endregion

    #region Canvas<->World
    public static Vector2 CanvasUnitsToWorldUnits(this Vector2 canvasUnits, Canvas canvas)
    {
        return canvasUnits * canvas.GetWorldUnitsPerCanvasUnit();
    }
    public static Vector2 CanvasPositionToWorldPosition(this Vector2 canvasPosition, Canvas canvas)
    {
        return (canvasPosition - canvas.GetCenterCanvasUnits()) * canvas.GetWorldUnitsPerCanvasUnit();
    }
    public static Vector2 CanvasPositionToWorldPosition(this Vector3 canvasPosition, Canvas canvas)
    {
        return ((Vector2)canvasPosition - canvas.GetCenterCanvasUnits()) * canvas.GetWorldUnitsPerCanvasUnit();
    }

    public static Vector2 WorldUnitsToCanvasUnits(this Vector2 worldUnits, Canvas canvas)
    {
        return worldUnits / canvas.GetWorldUnitsPerCanvasUnit();
    }
    public static Vector2 WorldPositionToCanvasPosition(this Vector2 worldPosition, Canvas canvas)
    {
        return worldPosition / canvas.GetWorldUnitsPerCanvasUnit() + canvas.GetCenterCanvasUnits();
    }
    public static Vector2 WorldPositionToCanvasPosition(this Vector3 worldPosition, Canvas canvas)
    {
        return worldPosition / canvas.GetWorldUnitsPerCanvasUnit() + canvas.GetCenterCanvasUnits();
    }
    #endregion

    #region Pixel<->Canvas
    public static Vector2 PixelsToCanvasUnits(this Vector2 pixels, Canvas canvas)
    {
        return pixels / canvas.GetPixelsPerCanvasUnit();
    }
    public static Vector2 PixelsToCanvasPosition(this Vector2 pixelPosition, Canvas canvas)
    {
        return pixelPosition / canvas.GetPixelsPerCanvasUnit();
    }
    public static Vector2 PixelsToCanvasPosition(this Vector3 pixelPosition, Canvas canvas)
    {
        return pixelPosition / canvas.GetPixelsPerCanvasUnit();
    }

    public static Vector2 CanvasUnitsToPixels(this Vector2 canvasUnits, Canvas canvas)
    {
        return canvasUnits * canvas.GetPixelsPerCanvasUnit();
    }
    public static Vector2 CanvasPositionToPixels(this Vector2 canvasPosition, Canvas canvas)
    {
        return canvasPosition * canvas.GetPixelsPerCanvasUnit();
    }
    public static Vector2 CanvasPositionToPixels(this Vector3 canvasPosition, Canvas canvas)
    {
        return canvasPosition * canvas.GetPixelsPerCanvasUnit();
    }
    #endregion
}
