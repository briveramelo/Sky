using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private CanvasToggle _inputCanvas;
    [SerializeField] private CanvasToggle _waveCanvas;
    [SerializeField] private CanvasToggle _scoreboardCanvas;
    [SerializeField] private CanvasToggle _tutorialCanvas;
    
    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        var isGameplayScene = Scenes.IsGameplay(scene.name);
        
        _inputCanvas.ToggleDisplay(isGameplayScene);
        _waveCanvas.ToggleDisplay(isGameplayScene);
        _scoreboardCanvas.ToggleDisplay(isGameplayScene);

        var isTutorialScene = scene.name == Scenes.Intro;
        _tutorialCanvas.ToggleDisplay(isTutorialScene);
    }
}
