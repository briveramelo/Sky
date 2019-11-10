using System;
using GenericFunctions;
using UnityEngine;

public class Joystick : Singleton<Joystick>
{
    public event Action<Vector2> OnTouchDirectionHold;
    public event Action OnTouchEnded;

    [SerializeField] private Canvas _parentCanvas;
    [SerializeField] private RectTransform _joystickView;

    private const string _joystickName = nameof(Joystick);

    private const float _joystickMaxStartCanvUnits = 64f;
    private const float _joystickMaxMoveCanvUnits = 16f;
    private Vector2 _joystickOriginCanvasPosition => (Vector2)transform.position + (transform as RectTransform).sizeDelta/2 * _parentCanvas.transform.lossyScale;
    private int _currentFingerId;
    private bool _isFingerInUse => _currentFingerId != Constants.UnusedFingerId;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_joystickOriginCanvasPosition, _joystickMaxStartCanvUnits);
    }

    private void Start()
    {
        OrderedTouchEventRegistry.Instance.OnTouchWorldBegin(typeof(Joystick), OnTouchWorldBegin, true);
        OrderedTouchEventRegistry.Instance.OnTouchWorldHeld(typeof(Joystick), OnTouchWorldHeld, true);
        OrderedTouchEventRegistry.Instance.OnTouchWorldEnd(typeof(Joystick), OnTouchWorldEnd, true);
    }
    
    private void OnDestroy()
    {
        if (TouchInputManager.Instance == null)
        {
            return;
        }
        OrderedTouchEventRegistry.Instance.OnTouchWorldBegin(typeof(Joystick), OnTouchWorldBegin, false);
        OrderedTouchEventRegistry.Instance.OnTouchWorldHeld(typeof(Joystick), OnTouchWorldHeld, false);
        OrderedTouchEventRegistry.Instance.OnTouchWorldEnd(typeof(Joystick), OnTouchWorldEnd, false);
    }

    private void OnTouchWorldBegin(int fingerId, Vector2 touchWorldPosition)
    {
        if (_isFingerInUse)
        {
            return;
        }

        var touchCanvasPosition = touchWorldPosition.WorldPositionToCanvasPosition(_parentCanvas);
        var targetAnchoredPosition = (touchCanvasPosition - _joystickOriginCanvasPosition) / _parentCanvas.transform.lossyScale;//convert to local space for anchored position
        
        var distFromStickCanvUnits = Vector2.Distance(targetAnchoredPosition, Vector2.zero);
        if (distFromStickCanvUnits <= _joystickMaxStartCanvUnits && TouchInputManager.Instance.TryClaimFingerId(fingerId, _joystickName))
        {
            _currentFingerId = fingerId;
            _joystickView.anchoredPosition = Vector2.ClampMagnitude(targetAnchoredPosition, _joystickMaxMoveCanvUnits);
        }
    }

    private void OnTouchWorldHeld(int fingerId, Vector2 touchWorldPosition)
    {
        if (fingerId != _currentFingerId)
        {
            return;
        }

        var touchCanvasPosition = touchWorldPosition.WorldPositionToCanvasPosition(_parentCanvas);
        var targetAnchoredPosition = (touchCanvasPosition - _joystickOriginCanvasPosition) / _parentCanvas.transform.lossyScale;//convert to local space for anchored position
        
        float intensity = Mathf.Clamp01(targetAnchoredPosition.magnitude / _joystickMaxMoveCanvUnits);
        var joyDirection = targetAnchoredPosition.normalized;
        var moveDirectionJoystickNormalized = intensity * joyDirection;

        _joystickView.anchoredPosition = moveDirectionJoystickNormalized * _joystickMaxMoveCanvUnits;
        OnTouchDirectionHold?.Invoke(moveDirectionJoystickNormalized);
    }

    private void OnTouchWorldEnd(int fingerId, Vector2 worldPosition)
    {
        if (fingerId != _currentFingerId)
        {
            return;
        }

        _currentFingerId = Constants.UnusedFingerId;
        TouchInputManager.Instance.ReleaseFingerId(fingerId, _joystickName);
        _joystickView.anchoredPosition = Vector2.zero;
        OnTouchEnded?.Invoke();
    }
}