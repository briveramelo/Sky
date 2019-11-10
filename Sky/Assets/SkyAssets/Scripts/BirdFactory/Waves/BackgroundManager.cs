using UnityEngine;

public interface ISwitchBackgrounds
{
    void UpdateBackground();
}

internal enum Background
{
    City = 0,
    Sea = 1,
    Mountain = 2,
    Complete
}

public class BackgroundManager : MonoBehaviour, ISwitchBackgrounds
{
    [SerializeField] private GameObject[] _backgrounds;
    private Background _currentBackground = Background.City;

    void ISwitchBackgrounds.UpdateBackground()
    {
        _backgrounds[(int) _currentBackground].SetActive(false);
        _currentBackground++;
        _backgrounds[(int) _currentBackground].SetActive(true);
    }
}