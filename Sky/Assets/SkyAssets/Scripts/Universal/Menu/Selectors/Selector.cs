using UnityEngine;
using System.Collections;

public abstract class Selector : MonoBehaviour, IEnd
{
    [SerializeField] protected AudioClip _buttonPress;

    [SerializeField] [Range(0, 2)] protected float _buttonRadius;
    protected abstract Vector2 TouchSpot { get; }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _buttonRadius);
    }

    protected static class ButtonState
    {
        public const int Up = 0;
        public const int Pressed = 1;
    }

    void IEnd.OnTouchEnd()
    {
        if (gameObject.activeInHierarchy)
        {
            if (Vector2.Distance(TouchSpot, transform.position) < _buttonRadius)
            {
                StartCoroutine(PressButton());
            }
        }
    }

    protected abstract IEnumerator PressButton();
}