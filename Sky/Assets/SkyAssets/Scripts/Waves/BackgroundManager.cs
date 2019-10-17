using UnityEngine;

public interface ISwitchBackgrounds {
    void UpdateBackground();
}

enum Background{
    City=0,
    Sea=1,
    Mountain=2,
    Complete
}

public class BackgroundManager : MonoBehaviour, ISwitchBackgrounds {

    [SerializeField] GameObject[] backgrounds;
    Background currentBackground = Background.City;

	void ISwitchBackgrounds.UpdateBackground() {
        backgrounds[(int)currentBackground].SetActive(false);
        currentBackground++;
        backgrounds[(int)currentBackground].SetActive(true);
    }
}
