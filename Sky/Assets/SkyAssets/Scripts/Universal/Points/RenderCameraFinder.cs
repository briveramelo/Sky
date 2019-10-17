using UnityEngine;
using UnityEngine.SceneManagement;

public class RenderCameraFinder : MonoBehaviour {

    [SerializeField] Canvas MyCanvas;

    void Awake()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }
	void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode) {
        MyCanvas.worldCamera = Camera.main;
    }
}
