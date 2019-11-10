using UnityEngine;
using System.Collections;
using GenericFunctions;

public class PooSlide : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _mySpriteRenderer;
    [SerializeField] private Animator _pooAnimator;
    [SerializeField] private Material[] _pooMaterials;
    [SerializeField] private Sprite[] _lastPooSprites;

    private const float _slideSpeed = .08f * 60f / 4f;
    private bool _isStationary;
    private Coroutine _slideDownRoutine;

    private void Awake()
    {
        _mySpriteRenderer.material = _pooMaterials[Constants.TargetPooInt];
        
        StartCoroutine(AnimateSplat());
        if (!_isStationary)
        {
            _slideDownRoutine = StartCoroutine(SlideDown());
        }
        //Destroy(transform.parent.gameObject, 15f);
        ScoreSheet.Tallier.TallyThreat(Threat.Poop);

        Constants.TargetPooInt++;
        Seagull.LogPooCam(true);
    }

    public void KeepStationary()
    {
        _isStationary = true;
        if (_slideDownRoutine != null)
        {
            StopCoroutine(_slideDownRoutine);
        }

        transform.localPosition = Vector3.zero;
    }

    private IEnumerator AnimateSplat()
    {
        while (_pooAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }

        Destroy(_pooAnimator);
        _mySpriteRenderer.sprite = _lastPooSprites[1];
    }

    private IEnumerator SlideDown()
    {
        while (true)
        {
            transform.position += Time.deltaTime * _slideSpeed * Vector3.down;
            yield return new WaitForSeconds(Random.Range(0.3f, 0.6f));
        }
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