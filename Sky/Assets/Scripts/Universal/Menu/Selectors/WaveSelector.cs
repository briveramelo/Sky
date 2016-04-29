using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public enum Scenes {
    Menu = 0,
    Intro =1,
    Story =2,
    Endless =3,
    Scores =4,
}
public enum WaveType {
    Story = 2,
    Endless = 3
}

public class WaveSelector : Selector {

    [SerializeField] WaveType MyWaveType;
    IFreezable inputManager;

    protected override Vector2 TouchSpot {get { return MenuInputHandler.touchSpot; } }
    
    void Awake() {
        inputManager = FindObjectOfType<MenuInputHandler>().GetComponent<IFreezable>();
    }

    protected override IEnumerator PressButton() {
        AudioManager.PlayAudio(buttonPress);
        //inputManager.IsFrozen = true;
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene((int)MyWaveType);
    }
}
