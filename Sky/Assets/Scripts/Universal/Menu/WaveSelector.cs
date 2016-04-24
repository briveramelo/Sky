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
    IWaveSet waveManager;
    
    protected override void Awake() {
        waveManager = FindObjectOfType<GameManager>().GetComponent<IWaveSet>();
        buttonRadius = 1.25f;
        base.Awake();
    }

    protected override IEnumerator PressButton() {
        buttonAnimator.SetInteger("AnimState", (int)ButtonState.Pressed);
        buttonNoise.PlayOneShot(buttonPress);
        waveManager.SetWaveType(MyWaveType);
        inputManager.IsFrozen = true;
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene((int)MyWaveType);
    }
}
