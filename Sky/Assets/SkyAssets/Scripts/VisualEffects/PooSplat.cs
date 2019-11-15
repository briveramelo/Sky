using UnityEngine;
using System.Collections;
using GenericFunctions;

public class PooSplat : MonoBehaviour
{
    [SerializeField] private GameObject _parent;
    [SerializeField] private SpriteRenderer _mySpriteRenderer;
    [SerializeField] private Animator _pooAnimator;
    [SerializeField] private Sprite _lastPooSprite;

    public void SetRenderTexture(RenderTexture renderTexture)
    {
        _mySpriteRenderer.material.SetTexture("_MaskTex", renderTexture);
    }

    private void Awake()
    {
        StartCoroutine(AnimateSplat());
        Destroy(_parent, 10f);
        ScoreSheet.Tallier.TallyThreat(Threat.Poop);
        Seagull.LogPooCam(true);
    }

    private IEnumerator AnimateSplat()
    {
        while (_pooAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }

        Destroy(_pooAnimator);
        _mySpriteRenderer.sprite = _lastPooSprite;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        Seagull.LogPooCam(false);
        if (ScoreSheet.Tallier == null)
        {
            return;
        }

        ScoreSheet.Tallier.TallyThreat(Threat.PoopCleaned);
    }
}