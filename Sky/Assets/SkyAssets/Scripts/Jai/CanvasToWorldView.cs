using UnityEngine;

public class CanvasToWorldView : MonoBehaviour
{
    private Camera _camera;

    private Camera _myCamera
    {
        get
        {
            if(_camera == null)
            {
                _camera = Camera.main;
            }
            return _camera;
        }
    }

    public Vector3 WorldPosition
    {
        get => TouchToWorld.GetWorldPosition(transform.position);
        set => transform.position = _myCamera.WorldToScreenPoint(value);
    }
    public Vector3 CanvasPosition
    {
        get => transform.position;
        set => transform.position = value;
    }
}