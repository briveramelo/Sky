using UnityEngine;
using System.Collections;

public class ContinueSelector : Selector
{
    [SerializeField] private Continuer _continuer;

    protected override IEnumerator OnClickRoutine()
    {
        yield return StartCoroutine(AdDisplayer.DisplayAd());
        
        Time.timeScale = 1f;
        _continuer.DisplayContinueMenu(false);
        if (Tentacles.Instance)
        {
            Tentacles.Releaser.ReleaseJai();
        }

        Basket.Instance.ComeBackToLife();
    }
}