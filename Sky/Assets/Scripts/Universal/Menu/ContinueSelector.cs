using UnityEngine;
using System.Collections;

public class ContinueSelector : Selector {

    protected override Vector2 TouchSpot {get { return InputManager.touchSpot; } }
    [SerializeField] Pauser pauser;

    protected override IEnumerator PressButton() {
        pauser.gameObject.SetActive(true);
        pauser.UnPause();
        yield return null;
    }
}
