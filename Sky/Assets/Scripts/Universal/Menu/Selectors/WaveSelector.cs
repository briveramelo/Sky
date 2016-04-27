using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public enum Scenes {
    Menu = 0,
    Story =1,
    Endless =2,
    Scores =3,
}

public class WaveSelector : Selector {

    [SerializeField] WaveType MyWaveType;
    IFreezable inputManager;

    protected override Vector2 TouchSpot {get { return MenuInputHandler.touchSpot; } }
    
    void Awake() {
        inputManager = FindObjectOfType<MenuInputHandler>().GetComponent<IFreezable>();
    }

    protected override IEnumerator PressButton() {
        buttonNoise.PlayOneShot(buttonPress);
        //inputManager.IsFrozen = true;
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene((int)MyWaveType);
    }
}
