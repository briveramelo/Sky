using UnityEngine;
using UnityEngine.SceneManagement;

public class RenderCameraFinder : MonoBehaviour
{
    [SerializeField] private Canvas _myCanvas;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        _myCanvas.worldCamera = Camera.main;
    }
}