using UnityEngine;

public interface ISwitchBackgrounds {
    void UpdateBackground();
}

internal enum Background{
    City=0,
    Sea=1,
    Mountain=2,
    Complete
}

public class BackgroundManager : MonoBehaviour, ISwitchBackgrounds {

    [SerializeField] private GameObject[] backgrounds;
    private Background currentBackground = Background.City;

	void ISwitchBackgrounds.UpdateBackground() {
        backgrounds[(int)currentBackground].SetActive(false);
        currentBackground++;
        backgrounds[(int)currentBackground].SetActive(true);
    }
}
