using UnityEngine;
using System.Collections;

public abstract class Selector : MonoBehaviour, IEnd {

	[SerializeField] protected Animator buttonAnimator;
    [SerializeField] protected TextMesh myText;
    [SerializeField] protected AudioSource buttonNoise;
    [SerializeField] protected AudioClip buttonPress;
    [SerializeField] protected AudioClip buttonLift;

    protected IFreezable inputManager;
    protected float buttonRadius;

    protected enum ButtonState {
        Up =0,
        Pressed =1
    }

    protected virtual void Awake() {
        inputManager = FindObjectOfType<MenuInputHandler>().GetComponent<IFreezable>();
    }


    void IEnd.OnTouchEnd(){
        if (Vector2.Distance(MenuInputHandler.touchSpot, transform.position) < buttonRadius){
            if (buttonAnimator.GetInteger("AnimState") == (int)ButtonState.Up){
                StartCoroutine (PressButton());
            }
        }
    }

    void AnimatePush() {
        myText.color = Color.red;
    }

    protected abstract IEnumerator PressButton();
}
