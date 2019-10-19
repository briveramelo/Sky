using UnityEngine;
using System.Collections;
using GenericFunctions;

public class PooSlide : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _mySpriteRenderer;
    [SerializeField] private Animator _pooAnimator;
    [SerializeField] private Material[] _pooMaterials;
    [SerializeField] private Sprite[] _lastPooSprites;

    private float _slideSpeed = .08f;

    private void Awake()
    {
        _mySpriteRenderer.material = _pooMaterials[Constants.TargetPooInt];
        StartCoroutine(AnimateSplat(Constants.TargetPooInt));
        StartCoroutine(SlideDown());
        Destroy(transform.parent.gameObject, 15f);
        ScoreSheet.Tallier.TallyThreat(Threat.Poop);

        Constants.TargetPooInt++;
        Seagull.LogPooCam(true);
    }

    private IEnumerator AnimateSplat(int pooCount)
    {
        while (_pooAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }

        Destroy(_pooAnimator);
        var i = (pooCount + 2) % 2 == 0 ? 0 : 1;
        _mySpriteRenderer.sprite = _lastPooSprites[1];
    }

    private IEnumerator SlideDown()
    {
        while (true)
        {
            transform.position += Vector3.down * _slideSpeed;
            yield return new WaitForSeconds(Random.Range(0.3f, 0.6f));
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        Seagull.LogPooCam(false);
        ScoreSheet.Tallier.TallyThreat(Threat.PoopCleaned);
    }
}