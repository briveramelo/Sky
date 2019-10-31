using System;
using UnityEngine;

public class Joystick : Singleton<Joystick>
{
    public static Vector2 MoveDirection01 { get; private set; }
    public static int CurrentFingerId { get; private set; }

    [SerializeField] private CanvasToWorldView _joystickView;

    public event Action<Vector2> OnTouchHold;
    public event Action OnTouchEnded;
    
    private const float _joystickMaxStartDist = 1.25f;
    private const float _joystickMaxMoveDistance = .75f; //maximum distance you can move the joystick
    private const string _joystickName = nameof(Joystick);
    private static Vector2 _startingJoystickPos;

    protected override void Awake()
    {
        base.Awake();
        TouchInputManager.Instance.OnTouchBegin += OnTouchBegin;
        TouchInputManager.Instance.OnTouchHeld += OnTouchHeld;
        TouchInputManager.Instance.OnTouchEnd += OnTouchEnd;
    }

    private void Start()
    {
        _startingJoystickPos = _joystickView.WorldPosition;
    }
    
    private void OnDestroy()
    {
        if (TouchInputManager.Instance == null)
        {
            return;
        }
        TouchInputManager.Instance.OnTouchBegin -= OnTouchBegin;
        TouchInputManager.Instance.OnTouchHeld -= OnTouchHeld;
        TouchInputManager.Instance.OnTouchEnd -= OnTouchEnd;
    }

    private void OnTouchBegin(int fingerId, Vector2 fingerPosition)
    {
        var distFromStick = Vector2.Distance(fingerPosition, _startingJoystickPos);
        if (distFromStick < _joystickMaxStartDist && TouchInputManager.Instance.TryClaimFingerId(fingerId, _joystickName))
        {
            CurrentFingerId = fingerId;
            _joystickView.WorldPosition = GetStickPosition(fingerPosition);
            MoveDirection01 = GetClampedMoveDirection(fingerPosition, 1);
        }
    }

    private void OnTouchHeld(int fingerId, Vector2 fingerPosition)
    {
        if (fingerId != CurrentFingerId)
        {
            return;
        }
        _joystickView.WorldPosition = GetStickPosition(fingerPosition);
        MoveDirection01 = GetClampedMoveDirection(fingerPosition, 1);
        OnTouchHold?.Invoke(MoveDirection01);
    }

    private void OnTouchEnd(int fingerId, Vector2 fingerPosition)
    {
        if (fingerId != CurrentFingerId)
        {
            return;
        }

        CurrentFingerId = -1;
        TouchInputManager.Instance.ReleaseFingerId(fingerId, _joystickName);
        _joystickView.WorldPosition = _startingJoystickPos;
        OnTouchEnded?.Invoke();
    }

    private static Vector2 GetStickPosition(Vector2 fingerPosition)
    {
        return _startingJoystickPos + GetClampedMoveDirection(fingerPosition, _joystickMaxMoveDistance);
    }

    private static Vector2 GetClampedMoveDirection(Vector2 fingerPosition, float maxClamp)
    {
        return Vector2.ClampMagnitude(fingerPosition - _startingJoystickPos, maxClamp);
    }
}