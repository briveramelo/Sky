using System;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class MaskCamera : MonoBehaviour
{
    [SerializeField] private Material _eraserMaterial;
    [SerializeField] private Camera _myCam;
    [SerializeField] private PooSlide _pooSlide;
    [SerializeField] private SpriteRenderer _pooRenderer;
    
    private bool _firstFrame;
    private Vector2? _holeLastFrameCenterPixels;

    #region Pixels
    private Vector2? _newHoleCenterPixels;
    private Vector2 _holeCenterPixels
    {
        get
        {
            if (!_newHoleCenterPixels.HasValue)
            {
                return ScreenSpace.ScreenSizePixels / 2;
            }

            return _newHoleCenterPixels.Value;
        }
    }
    private Vector2 _holeSizePixels => 32 * ScreenSpace.ScreenZoom * Vector2.one;
    private Rect _holeRectPixels => new Rect(_holeCenterPixels - _holeSizePixels / 2, _holeSizePixels);
    
    private Vector2 _pooSizeTexturePixels => _pooRenderer.sprite.rect.size * ScreenSpace.ScreenZoom;
    private Rect _pooRectPixels => new Rect(Vector2.zero, _pooSizeTexturePixels);
    #endregion

    #region World
    private Rect _pooRectWorld => new Rect(-_pooRectPixels.size.PixelsToWorldUnits() / 2, _pooRectPixels.size.PixelsToWorldUnits());
    private Rect _holeRectWorld => new Rect(_holeRectPixels.position.PixelsToWorldPosition(), _holeRectPixels.size.PixelsToWorldUnits());
    #endregion
    
    #region Screen To Texture Normalized Space Conversion Values
    private Vector2 _screenToTextureOffset => 0.5f * (Vector2.one - ScreenSpace.ScreenSizePixels / _pooSizeTexturePixels);
    private Vector2 _screenToTextureFactor => ScreenSpace.ScreenSizePixels / _pooSizeTexturePixels;
    #endregion

    #region Gizmos
    private void OnDrawGizmosSelected()
    {
        DrawBoundingBox(new Rect(-ScreenSpace.ScreenSizeWorldUnits / 2, ScreenSpace.ScreenSizeWorldUnits), Color.blue);
        DrawBoundingBox(_pooRectWorld, Color.blue);
        DrawBoundingBox(new Rect(Vector2.zero - Vector2.one * 0.5f, Vector2.one), Color.blue);
        DrawBoundingBox(new Rect(GetTexturePositionNormalized(Vector2.zero)- Vector2.one * 0.5f, GetTextureSizeNormalized(Vector2.one)), Color.blue);
        
        if (_newHoleCenterPixels == null)
        {
            return;
        }
        
        Gizmos.color = Color.red;
        DrawTouchSpot(_holeRectWorld, Color.red);
        var normalizedScreenPosition = _holeRectWorld.position.WorldToViewportPosition();
        var normalizedScreenSize = _holeRectWorld.size.WorldToViewportUnits();
        DrawTouchSpot(new Rect(normalizedScreenPosition- Vector2.one * 0.5f, normalizedScreenSize), Color.red);
        DrawTouchSpot(new Rect(GetTexturePositionNormalized(normalizedScreenPosition)- Vector2.one * 0.5f, GetTextureSizeNormalized(normalizedScreenSize)), Color.red);
    }

    private void DrawBoundingBox(Rect boundingRect, Color color)
    {
        Gizmos.color = color;
        Gizmos.DrawWireCube(boundingRect.center, boundingRect.size);

        //draw poo image center lines (pixels)
        Gizmos.DrawLine(boundingRect.center + Vector2.up * boundingRect.height / 2, boundingRect.center - Vector2.up * boundingRect.height / 2);
        Gizmos.DrawLine(boundingRect.center - Vector2.right * boundingRect.width / 2, boundingRect.center + Vector2.right * boundingRect.width / 2);
    }

    private void DrawTouchSpot(Rect touchRect, Color color)
    {
        Gizmos.color = color;
        Gizmos.DrawWireSphere(touchRect.center, touchRect.size.x / 2);
        Gizmos.DrawWireCube(touchRect.center, touchRect.size);
    }
    #endregion

    private void Awake()
    {
        SetRenderTexture();
        _firstFrame = true;
        
        TouchInputManager.Instance.OnTouchWorldHeld += OnTouchWorldHeld;
        TouchInputManager.Instance.OnTouchWorldEnd += OnTouchWorldEnd;
    }
    
    private void SetRenderTexture()
    {
        var renderTextSize = _pooSizeTexturePixels;
        var newRenderTex = new RenderTexture((int) renderTextSize.x, (int) renderTextSize.y, 0, GraphicsFormat.R8G8B8A8_UNorm);
        _myCam.targetTexture = newRenderTex;
        _pooSlide.SetRenderTexture(newRenderTex);
    }

    private void OnDestroy()
    {
        if (!TouchInputManager.Instance)
        {
            return;
        }

        TouchInputManager.Instance.OnTouchWorldHeld -= OnTouchWorldHeld;
        TouchInputManager.Instance.OnTouchWorldEnd -= OnTouchWorldEnd;
    }

    private void OnTouchWorldHeld(int fingerId, Vector2 touchWorldPosition)
    {
        _newHoleCenterPixels = touchWorldPosition.WorldPositionToPixels();
        
        if (!_pooRectWorld.Overlaps(new Rect(touchWorldPosition, _holeSizePixels.PixelsToWorldUnits())))
        {
            _newHoleCenterPixels = null;
        }
    }

    private void OnTouchWorldEnd(int fingerId, Vector2 touchWorldPosition)
    {
        _newHoleCenterPixels = null;
    }

    private void OnPostRender()
    {
        if (_firstFrame)
        {
            _firstFrame = false;
            GL.Clear(false, true, new Color(0.0f, 0.0f, 0.0f, 0.0f));
        }

        if (_newHoleCenterPixels.HasValue)
        {
            CutHole(_holeRectPixels.center.PixelsToViewport(), 0.15f, _eraserMaterial);
            
            if (_holeLastFrameCenterPixels.HasValue)
            {
                FillHoleGaps();
            }
        }

        _holeLastFrameCenterPixels = _newHoleCenterPixels;
    }

    private void FillHoleGaps()
    {
        var holeIncrement = new Range(_holeRectPixels.size).Average / 3;
        Vector2 toDest = (_holeRectPixels.center - _holeLastFrameCenterPixels.Value).normalized * holeIncrement;
        for (var iteratedPoint = _holeLastFrameCenterPixels.Value + toDest; Vector2.Distance(iteratedPoint, _holeRectPixels.center) > holeIncrement; iteratedPoint += toDest)
        {
            CutHole(iteratedPoint.PixelsToViewport(), 0.15f, _eraserMaterial);
        }
    }

    //values can exist below 0 and above 1, as the image may expand beyond the 0,1 screen bounds
    private Vector2 GetTexturePositionNormalized(Vector2 normalizedScreenPosition)
    {
        return normalizedScreenPosition * _screenToTextureFactor + _screenToTextureOffset;
    }
    
    private Vector2 GetTextureSizeNormalized(Vector2 normalizedScreenSize)
    {
        return normalizedScreenSize * _screenToTextureFactor;
    }

    private void CutHole(Vector2 holeCenterScreenNormalized, float screenPercentage, Material eraserMaterial)
    {
        const float squareRootTwo = 1.4142f;
        float texturePercentage = screenPercentage * new Range(_screenToTextureFactor).Average;
        var holeRectImageSpace = new Rect
        {
            position = GetTexturePositionNormalized(holeCenterScreenNormalized),
            size = squareRootTwo * texturePercentage * (Vector2.one / _pooSizeTexturePixels).normalized
        };
        holeRectImageSpace.position -= holeRectImageSpace.size / 2;
        
        var unitRect = new Rect(Vector2.zero, Vector2.one);
        
        GL.PushMatrix();
        GL.LoadOrtho();
        for (var i = 0; i < eraserMaterial.passCount; i++)
        {
            eraserMaterial.SetPass(i);
            GL.Begin(GL.QUADS);
            GL.Color(Color.white);
            GL.TexCoord2(unitRect.xMin, unitRect.yMax);//top left
            GL.Vertex3(holeRectImageSpace.xMin, holeRectImageSpace.yMax, 0.0f);
            
            GL.TexCoord2(unitRect.xMax, unitRect.yMax);//top right
            GL.Vertex3(holeRectImageSpace.xMax, holeRectImageSpace.yMax, 0.0f);
            
            GL.TexCoord2(unitRect.xMax, unitRect.yMin);//bottom right
            GL.Vertex3(holeRectImageSpace.xMax, holeRectImageSpace.yMin, 0.0f);
            
            GL.TexCoord2(unitRect.xMin, unitRect.yMin);//bottom left
            GL.Vertex3(holeRectImageSpace.xMin, holeRectImageSpace.yMin, 0.0f);
            GL.End();
        }

        GL.PopMatrix();
    }
}