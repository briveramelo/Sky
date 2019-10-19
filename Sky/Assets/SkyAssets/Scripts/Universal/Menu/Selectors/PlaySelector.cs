using UnityEngine;
using System.Collections;

public class PlaySelector : Selector {

    protected override Vector2 TouchSpot => InputManager.TouchSpot;
    [SerializeField] private Pauser _pauser;

    protected override IEnumerator PressButton() {
        _pauser.gameObject.SetActive(true);
        _pauser.UnPause();
        yield return null;
    }
}
