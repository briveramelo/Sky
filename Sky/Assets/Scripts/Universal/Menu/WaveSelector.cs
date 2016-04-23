using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public interface ILiftable {
    void LiftButton();
}
public enum Scenes {
    Menu = 0,
    Story =1,
    Endless =2
}

public class WaveSelector : MonoBehaviour, IEnd {

    [SerializeField] WaveType MyWaveType;
    [SerializeField] Animator buttonAnimator;
    [SerializeField] TextMesh myText;
    [SerializeField] AudioSource buttonNoise;
    [SerializeField] AudioClip buttonPress;
    [SerializeField] AudioClip buttonLift;

    float buttonRadius = 1f;
    IWaveSet waveManager;
    IFreezable inputManager;

    enum ButtonState {
        Up =0,
        Pressed =1
    }

    void Awake() {
        waveManager = FindObjectOfType<GameManager>().GetComponent<IWaveSet>();
        inputManager = FindObjectOfType<MenuInputHandler>().GetComponent<IFreezable>();
    }

    void IEnd.OnTouchEnd(){
        if (Vector2.Distance(MenuInputHandler.touchSpot, transform.position) < buttonRadius){
            if (buttonAnimator.GetInteger("AnimState") == (int)ButtonState.Up){
                StartCoroutine (PressButton());
            }
        }
    }

    IEnumerator PressButton() {
        buttonAnimator.SetInteger("AnimState", (int)ButtonState.Pressed);
        buttonNoise.PlayOneShot(buttonPress);
        waveManager.SetWaveType(MyWaveType);
        inputManager.IsFrozen = true;
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene((int)MyWaveType);
    }

    void AnimatePush() {
        myText.color = Color.red;
    }
}
