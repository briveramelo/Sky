using UnityEngine;

public interface IToggleable
{
    void ToggleSensor(bool active);
}

public interface IJaiDetector
{
    bool JaiInRange { get; }
}

public class TentaclesSensor : MonoBehaviour, IToggleable, IJaiDetector
{
    [SerializeField] private Tentacles _tentaclesScript;
    [SerializeField] private BoxCollider2D _sensor;

    private ISensorToTentacle _tentacle;
    private bool _jaiInRange;

    bool IJaiDetector.JaiInRange => _jaiInRange;

    private void Awake()
    {
        _tentacle = _tentaclesScript;
    }

    private void Start()
    {
        InitializeSensor();
    }

    private void InitializeSensor()
    {
        var worldSize = ScreenSpace.ScreenSizeWorldUnits;
        _sensor.size = worldSize;
        _sensor.offset = Vector2.up * (-worldSize.y + worldSize.y / 3);
    }

    private void OnTriggerEnter2D()
    {
        _jaiInRange = true;
        _tentacle.GoForTheKill();
    }

    private void OnTriggerExit2D()
    {
        _jaiInRange = false;
        _tentacle.ResetPosition(false);
    }

    void IToggleable.ToggleSensor(bool active)
    {
        _sensor.enabled = active;
    }
}