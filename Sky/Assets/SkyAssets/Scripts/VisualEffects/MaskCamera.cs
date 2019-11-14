using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class MaskCamera : MonoBehaviour
{
    [SerializeField] private Material _eraserMaterial;
    [SerializeField] private Camera _myCam;
    [SerializeField] private PooSlide _pooSlide;
    [SerializeField] private SpriteRenderer _pooRenderer;
    
    private bool _firstFrame;

    #region Hole

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

    #endregion

    #region Poo

    private Vector2 _pooSizeTexturePixels => _pooRenderer.sprite.rect.size * 2;

    private Vector2 _pooSizeScreenPixels => _pooSizeTexturePixels;

    //private Vector2 _pooSizeTexturePixels => ScreenSpace.ScreenSizePixels;
    private Rect _pooRectPixels => new Rect(Vector2.zero, _pooSizeScreenPixels);
    private Rect _pooRectWorld => new Rect(-_pooRectPixels.size.PixelsToWorldUnits() / 2, _pooRectPixels.size.PixelsToWorldUnits());
    private Rect _holeRectWorld => new Rect(_holeRectPixels.position.PixelsToWorldPosition(), _holeRectPixels.size.PixelsToWorldUnits());

    #endregion

    private void OnDrawGizmosSelected()
    {
        DrawBoundingBox(_pooRectWorld);
        DrawBoundingBox(new Rect(Vector2.zero, Vector2.one));


        //draw holes
//        {
//            Vector2[] specialSpotsPixels = new Vector2[]
//            {
//                new Vector2(0,0),
//                new Vector2(ScreenSpace.ScreenSizePixels.x,ScreenSpace.ScreenSizePixels.y),
//                new Vector2(0,ScreenSpace.ScreenSizePixels.y),
//                new Vector2(ScreenSpace.ScreenSizePixels.x,0),
//                new Vector2(ScreenSpace.ScreenSizePixels.x,ScreenSpace.ScreenSizePixels.y) /2,
//            };
//            for (int i = 0; i < specialSpotsPixels.Length; i++)
//            {
//                DrawTouchSpot(new Rect(specialSpotsPixels[i].PixelsToWorldPosition(), _holeRectWorld.size), Color.red);
//                DrawTouchSpot(new Rect(specialSpotsPixels[i].PixelsToViewport(), Vector2.one.normalized * 0.05f), Color.red);
//            }
//        }

        if (_newHoleCenterPixels == null)
        {
            return;
        }
        
        //draw touch spots
        {
            Gizmos.color = Color.red;
            DrawTouchSpot(_holeRectWorld, Color.red);
            var normalizedScreenPosition = _holeRectWorld.position.WorldToViewportPosition();
            var normalizedScreenSize = _holeRectWorld.size.WorldToViewportUnits();
            DrawTouchSpot(new Rect(normalizedScreenPosition, normalizedScreenSize), Color.red);
            DrawTouchSpot(new Rect(GetImagePositionNormalized(normalizedScreenPosition), GetImageSizeNormalized(normalizedScreenSize)), Color.red);
            PrintTest();
        }
    }

    private void DrawBoundingBox(Rect boundingRect)
    {
        Gizmos.color = Color.blue;
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

    private void Awake()
    {
        var screenSize = ScreenSpace.ScreenSizePixels;
        var newRenderTex = new RenderTexture((int) screenSize.x, (int) screenSize.y, 0, GraphicsFormat.R8G8B8A8_UNorm);
        _myCam.targetTexture = newRenderTex;
        _pooSlide.SetRenderTexture(newRenderTex);
        
        _firstFrame = true;
        TouchInputManager.Instance.OnTouchWorldHeld += OnTouchWorldHeld;
        TouchInputManager.Instance.OnTouchWorldEnd += OnTouchWorldEnd;
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
            CutHole(_holeRectPixels.position.PixelsToViewport(), _eraserMaterial);
        }
    }

    private Range _xBoundsScreenNormalized => new Range(-0.5f * _pooSizeTexturePixels.x / ScreenSpace.ScreenSizePixels.x + 0.5f, 0.5f * _pooSizeTexturePixels.x / ScreenSpace.ScreenSizePixels.x + 0.5f);
    private Range _yBoundsScreenNormalized => new Range(-0.5f * _pooSizeTexturePixels.y / ScreenSpace.ScreenSizePixels.y + 0.5f, 0.5f * _pooSizeTexturePixels.y / ScreenSpace.ScreenSizePixels.y + 0.5f);
    private Vector2 _minScreenNormalized => new Vector2(_xBoundsScreenNormalized.Min, _yBoundsScreenNormalized.Min);
    private Vector2 _screenToImageFactor => new Vector2((0.5f - _xBoundsScreenNormalized.Min) / _xBoundsScreenNormalized.Diff, (0.5f - _yBoundsScreenNormalized.Min) / _yBoundsScreenNormalized.Diff);

    //values can exist below 0 and above 1, as the image may expand beyond the 0,1 screen bounds
    private Vector2 GetImagePositionNormalized(Vector2 normalizedScreenPosition)
    {
        return normalizedScreenPosition * _screenToImageFactor + _minScreenNormalized;
    }
    
    private Vector2 GetImageSizeNormalized(Vector2 normalizedScreenSize)
    {
        return normalizedScreenSize * _screenToImageFactor;
    }

    private void PrintTest()
    {
        Debug.Log(GetImagePositionNormalized(new Vector2(0, 0)));
        Debug.Log(GetImagePositionNormalized(new Vector2(1, 1)));
        Debug.Log(GetImagePositionNormalized(new Vector2(0.5f, 0.5f)));
    }

    private void CutHole(Vector2 holeRectPositionScreenNormalized, Material eraserMaterial)
    {
        var holeRectImageSpace = new Rect
        {
            position = GetImagePositionNormalized(holeRectPositionScreenNormalized),
            size = (Vector2.one / _pooSizeTexturePixels).normalized * 0.05f
        };
        
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