using UnityEngine;
using GenericFunctions;

public class MaskCamera : MonoBehaviour
{
    [SerializeField] private Transform _pooSliderTransform;
    [SerializeField] private Material _eraserMaterial;
    [SerializeField] private Camera _myCam;
    [SerializeField] private RenderTexture[] _rts;

    
    private bool _firstFrame;
    private Vector2 RenderSize => new Vector2(_myCam.targetTexture.width, _myCam.targetTexture.height);
    private Vector2 RenderUnitsPerWorldUnits => RenderSize / ScreenSpace.ScreenSizeWorldUnits;
    private Vector2 _newHoleWorldPos;
    private Vector2? _newHolePositionPixels;

    private void OnDrawGizmosSelected()
    {
        //draw world contents
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_newHoleWorldPos, .15f);
            Gizmos.color = Color.black;
            Vector2 pooPos = _pooSliderTransform.position;
            var worldSize = ScreenSpace.WorldEdge;
            var pooRectWorldUnits = new Rect(-worldSize + pooPos, 2 * worldSize);
            Gizmos.DrawWireCube(pooRectWorldUnits.center, pooRectWorldUnits.size);
            
            Gizmos.DrawLine(pooRectWorldUnits.center + Vector2.up * pooRectWorldUnits.height /2, pooRectWorldUnits.center - Vector2.up * pooRectWorldUnits.height /2);
            Gizmos.DrawLine(pooRectWorldUnits.center - Vector2.right * pooRectWorldUnits.width /2, pooRectWorldUnits.center + Vector2.right * pooRectWorldUnits.width /2);
        }
        
        //draw normalized contents
        if (_newHolePositionPixels.HasValue)
        {
            //draw normalized box
            Gizmos.color = Color.black;
            Gizmos.DrawWireCube(Vector2.one*.5f, Vector2.one);
            Gizmos.DrawLine(new Vector2(0.5f, 1f), new Vector2(0.5f, 0f));
            Gizmos.DrawLine(new Vector2(0f, 0.5f), new Vector2(1f, 0.5f));
            
            //draw normalized sphere
            Gizmos.color = Color.red;
            Vector2 holeLocalToImageWorldPos = _pooSliderTransform.InverseTransformPoint(_newHoleWorldPos);
            Vector2 holeSizeNormalized = GetRenderHoleSize();
            var normHole = holeLocalToImageWorldPos.WorldToViewport();
            Gizmos.DrawWireCube(normHole, holeSizeNormalized);
        }
    }

    private void Awake()
    {
        _myCam.targetTexture = _rts[Constants.TargetPooInt];
        _firstFrame = true;
        TouchInputManager.Instance.OnTouchWorldHeld += OnTouchWorldHeld;
    }

    private void OnDestroy()
    {
        if (!TouchInputManager.Instance)
        {
            return;
        }

        TouchInputManager.Instance.OnTouchWorldHeld -= OnTouchWorldHeld;
    }

    private void OnTouchWorldHeld(int fingerId, Vector2 touchWorldPosition)
    {
        _newHoleWorldPos = touchWorldPosition;
        Vector2 pooPos = _pooSliderTransform.position;
        var worldSize = ScreenSpace.WorldEdge;
        var pooRectWorldUnits = new Rect(-worldSize + pooPos, 2 * worldSize);
        
        if (pooRectWorldUnits.Contains(touchWorldPosition))
        {
            _newHolePositionPixels = touchWorldPosition.WorldPositionToPixels();
        }
        else
        {
            _newHolePositionPixels = null;
        }
    }

    private void OnPostRender()
    {
        if (_firstFrame)
        {
            _firstFrame = false;
            GL.Clear(false, true, new Color(0.0f, 0.0f, 0.0f, 0.0f));
        }

        if (_newHolePositionPixels.HasValue)
        {
            Vector2 holeLocalToImageWorldPos = _pooSliderTransform.InverseTransformPoint(_newHoleWorldPos);
            Vector2 holeSizeNormalized = GetRenderHoleSize();
            var normHole = holeLocalToImageWorldPos.WorldToViewport();
            Vector2 renderHoleBottomLeft = normHole - holeSizeNormalized/2;
            Rect renderHoleRectNormalized = new Rect(renderHoleBottomLeft, holeSizeNormalized);
            CutHole(renderHoleRectNormalized);
        }
    }

    //approximately normalizes hole size across resolutions
    private Vector2 GetRenderHoleSize()
    {
        const float zoomScalar = .16f/2;
        const float addend = 70f-zoomScalar;
        const float scalar = 1.5f;
        var holeSizeScaled = scalar * (addend + zoomScalar * ScreenSpace.ScreenZoom) * (Vector2.one / RenderSize);
        return holeSizeScaled;
    }

    private void CutHole(Rect holeRectNormalized)
    {
        var normalizedErasableImageRect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
        
        GL.PushMatrix();
        GL.LoadOrtho();
        for (var i = 0; i < _eraserMaterial.passCount; i++)
        {
            _eraserMaterial.SetPass(i);
            GL.Begin(GL.QUADS);
            GL.Color(Color.white);
            GL.TexCoord2(normalizedErasableImageRect.xMin, normalizedErasableImageRect.yMax);
            GL.Vertex3(holeRectNormalized.xMin, holeRectNormalized.yMax, 0.0f);
            GL.TexCoord2(normalizedErasableImageRect.xMax, normalizedErasableImageRect.yMax);
            GL.Vertex3(holeRectNormalized.xMax, holeRectNormalized.yMax, 0.0f);
            GL.TexCoord2(normalizedErasableImageRect.xMax, normalizedErasableImageRect.yMin);
            GL.Vertex3(holeRectNormalized.xMax, holeRectNormalized.yMin, 0.0f);
            GL.TexCoord2(normalizedErasableImageRect.xMin, normalizedErasableImageRect.yMin);
            GL.Vertex3(holeRectNormalized.xMin, holeRectNormalized.yMin, 0.0f);
            GL.End();
        }

        GL.PopMatrix();
    }
}