using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class QuitSelector : Selector {

    [SerializeField] WaveUI waveUI;
    [SerializeField] Pauser pauser;
    protected override Vector2 TouchSpot { get { return InputManager.touchSpot; } }

    void Start() {
        if (waveUI == null) {
            waveUI = FindObjectOfType<WaveUI>();
        }
    }

    protected override IEnumerator PressButton() {
        buttonNoise.PlayOneShot(buttonPress);
        float startTime = Time.unscaledTime;
        while (Time.unscaledTime - startTime < 1f) {
            yield return null;
        }
        pauser.ResetPause();
        SceneManager.LoadScene((int)Scenes.Menu);
        yield return null;
    }
}
