using UnityEngine;
using GenericFunctions;

public class Joyfulstick : MonoBehaviour, IBegin, IHold, IEnd
{
    [SerializeField] private Transform _stickBase;

    public static readonly Vector2 StartingJoystickSpot = new Vector2(-Constants.WorldDimensions.x * (2f / 3f), -Constants.WorldDimensions.y * (2f / 5f));
    public const float JoystickMaxStartDist = 1.25f;
    public const float JoystickMaxMoveDistance = .75f; //maximum distance you can move the joystick
    private IStickEngineId _inputManager;

    private void Awake()
    {
        _stickBase.position = StartingJoystickSpot;
    }

    private void Start()
    {
        _inputManager = FindObjectOfType<InputManager>().GetComponent<IStickEngineId>();
    }

    void IBegin.OnTouchBegin(int fingerId)
    {
        var distFromStick = Vector2.Distance(InputManager.TouchSpot, StartingJoystickSpot);
        if (distFromStick < JoystickMaxStartDist)
        {
            _inputManager.SetStickEngineId(fingerId);
            transform.position = SetStickPosition();
        }
    }

    void IHold.OnTouchHeld()
    {
        transform.position = SetStickPosition();
    }

    void IEnd.OnTouchEnd()
    {
        transform.position = StartingJoystickSpot;
    }

    private static Vector2 SetStickPosition()
    {
        return StartingJoystickSpot + Vector2.ClampMagnitude(InputManager.TouchSpot - StartingJoystickSpot, JoystickMaxMoveDistance);
    }
}