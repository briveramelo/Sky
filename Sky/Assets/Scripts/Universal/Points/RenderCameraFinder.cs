using UnityEngine;
using System.Collections;

public class RenderCameraFinder : MonoBehaviour {

    [SerializeField] Canvas MyCanvas;

	void OnLevelWasLoaded(int level) {
        MyCanvas.worldCamera = Camera.main;
    }
}
