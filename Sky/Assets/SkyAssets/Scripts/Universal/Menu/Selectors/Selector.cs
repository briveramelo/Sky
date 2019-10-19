using UnityEngine;
using System.Collections;

public abstract class Selector : MonoBehaviour, IEnd {

    [SerializeField] protected AudioClip _buttonPress;

    [SerializeField, Range(0,2)] protected float _buttonRadius;
    abstract protected Vector2 TouchSpot {get;}

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _buttonRadius);
    }

    protected enum ButtonState {
        Up =0,
        Pressed =1
    }

    void IEnd.OnTouchEnd(){
        if (gameObject.activeInHierarchy) {
            if (Vector2.Distance(TouchSpot, transform.position) < _buttonRadius){
                StartCoroutine (PressButton());
            }
        }
    }

    protected abstract IEnumerator PressButton();
}
