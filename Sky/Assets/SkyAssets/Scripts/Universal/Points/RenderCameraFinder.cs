using UnityEngine;
using UnityEngine.SceneManagement;

public class RenderCameraFinder : MonoBehaviour {

    [SerializeField] private Canvas MyCanvas;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode) {
        MyCanvas.worldCamera = Camera.main;
    }
}
