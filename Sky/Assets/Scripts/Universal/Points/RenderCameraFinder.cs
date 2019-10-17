using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class RenderCameraFinder : MonoBehaviour {

    [SerializeField] Canvas MyCanvas;

	void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode) {
        MyCanvas.worldCamera = Camera.main;
    }
}
