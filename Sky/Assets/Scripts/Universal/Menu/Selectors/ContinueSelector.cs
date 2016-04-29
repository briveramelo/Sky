using UnityEngine;
using System.Collections;

public class ContinueSelector : Selector {

	protected override Vector2 TouchSpot {get { return InputManager.touchSpot; } }
    [SerializeField] Continuer continuer;
    [SerializeField] GameObject continueMenu;
    bool buttonPressed;

    protected override IEnumerator PressButton() {
        yield return StartCoroutine(DisplayAd());

        Time.timeScale = 1f;
        continuer.DisplayContinueMenu(false);
        if (Tentacles.Instance) {
            Tentacles.Releaser.ReleaseJai();
        }
        Basket.Instance.ComeBackToLife();
    }

    IEnumerator DisplayAd() {
        Debug.Log("Ad Here");
        yield return null;
    }
}
