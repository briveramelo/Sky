using UnityEngine;

public interface IScoreMenu {
    void SetScoreMenu(GameMode ScoreMenu);
}

public class GameManager : MonoBehaviour, IScoreMenu {

    public static GameManager Instance;
    static GameMode scoreMenu; public static GameMode ScoreMenu {get { return scoreMenu; } }

    [SerializeField] WaveManager waveManager;

    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

    void IScoreMenu.SetScoreMenu(GameMode ScoreMenu) {
        scoreMenu = ScoreMenu;
    }
}
