using UnityEngine;
using System.Collections;

public class ContinueSelector : Selector {

	protected override Vector2 TouchSpot {get { return InputManager.touchSpot; } }
    [SerializeField] Continuer continuer;

    protected override IEnumerator PressButton() {
        yield return StartCoroutine(AdDisplayer.DisplayAd());

        Time.timeScale = 1f;
        continuer.DisplayContinueMenu(false);
        if (Tentacles.Instance) {
            Tentacles.Releaser.ReleaseJai();
        }
        Basket.Instance.ComeBackToLife();
    }
}
