using UnityEngine;
using System.Collections;

public class ContinueSelector : Selector {

	protected override Vector2 TouchSpot => InputManager.TouchSpot;
    [SerializeField] private Continuer _continuer;

    protected override IEnumerator PressButton() {
        yield return StartCoroutine(AdDisplayer.DisplayAd());

        Time.timeScale = 1f;
        _continuer.DisplayContinueMenu(false);
        if (Tentacles.Instance) {
            Tentacles.Releaser.ReleaseJai();
        }
        Basket.Instance.ComeBackToLife();
    }
}
