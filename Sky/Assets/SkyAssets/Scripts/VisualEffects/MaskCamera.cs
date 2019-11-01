﻿using System;
using UnityEngine;
using GenericFunctions;

public class MaskCamera : MonoBehaviour
{
    [SerializeField] private Transform _pooSliderTransform;
    [SerializeField] private Material _eraserMaterial;
    [SerializeField] private Camera _myCam;
    [SerializeField] private RenderTexture[] _rts;
    
    private Vector3 _startingPoint;
    private bool _firstFrame;
    private Vector2? _newHolePosition;

    private void Awake()
    {
        _startingPoint = _pooSliderTransform.transform.position;
        _myCam.targetTexture = _rts[Constants.TargetPooInt];
        _firstFrame = true;
        TouchInputManager.Instance.OnTouchWorldHeld += OnTouchWorldHeld;
    }

    private void OnDestroy()
    {
        TouchInputManager.Instance.OnTouchWorldHeld -= OnTouchWorldHeld;
    }

    private void OnTouchWorldHeld(int fingerId, Vector2 worldPosition)
    {
        _newHolePosition = null;
        var pooPos = _pooSliderTransform.position;
        var worldRect = new Rect(
            -Constants.ScreenSizeWorldUnits.x + pooPos.x - _startingPoint.x, 
            -Constants.ScreenSizeWorldUnits.y + pooPos.y - _startingPoint.y,
            Constants.ScreenSizeWorldUnits.x, 
            Constants.ScreenSizeWorldUnits.y);
        if (worldRect.Contains(worldPosition))
        {
            _newHolePosition = new Vector2(
                Constants.ScreenSizePixels.x * (worldPosition.x - worldRect.xMin) / worldRect.width,
                Constants.ScreenSizePixels.y * (worldPosition.y - worldRect.yMin) / worldRect.height);
        }
    }

    private void OnPostRender()
    {
        if (_firstFrame)
        {
            _firstFrame = false;
            GL.Clear(false, true, new Color(0.0f, 0.0f, 0.0f, 0.0f));
        }

        if (_newHolePosition != null)
        {
            CutHole(new Vector2(Constants.ScreenSizePixels.x, Constants.ScreenSizePixels.y), _newHolePosition.Value);
        }
    }

    private void CutHole(Vector2 imageSize, Vector2 imageLocalPosition)
    {
        var textureRect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
        var positionRect = new Rect(
            (imageLocalPosition.x - 0.5f * _eraserMaterial.mainTexture.width) / imageSize.x,
            (imageLocalPosition.y - 0.5f * _eraserMaterial.mainTexture.height) / imageSize.y,
            _eraserMaterial.mainTexture.width / imageSize.x,
            _eraserMaterial.mainTexture.height / imageSize.y
        );
        GL.PushMatrix();
        GL.LoadOrtho();
        for (var i = 0; i < _eraserMaterial.passCount; i++)
        {
            _eraserMaterial.SetPass(i);
            GL.Begin(GL.QUADS);
            GL.Color(Color.white);
            GL.TexCoord2(textureRect.xMin, textureRect.yMax);
            GL.Vertex3(positionRect.xMin, positionRect.yMax, 0.0f);
            GL.TexCoord2(textureRect.xMax, textureRect.yMax);
            GL.Vertex3(positionRect.xMax, positionRect.yMax, 0.0f);
            GL.TexCoord2(textureRect.xMax, textureRect.yMin);
            GL.Vertex3(positionRect.xMax, positionRect.yMin, 0.0f);
            GL.TexCoord2(textureRect.xMin, textureRect.yMin);
            GL.Vertex3(positionRect.xMin, positionRect.yMin, 0.0f);
            GL.End();
        }

        GL.PopMatrix();
    }
}