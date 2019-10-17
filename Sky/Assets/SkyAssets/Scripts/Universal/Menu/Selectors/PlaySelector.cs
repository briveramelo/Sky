using UnityEngine;
using System.Collections;

public class PlaySelector : Selector {

    protected override Vector2 TouchSpot => InputManager.touchSpot;
    [SerializeField] Pauser pauser;

    protected override IEnumerator PressButton() {
        pauser.gameObject.SetActive(true);
        pauser.UnPause();
        yield return null;
    }
}
