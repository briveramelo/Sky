using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GenericFunctions;
using TMPro;
using UnityEngine;

public class TipUi : MonoBehaviour
{
    private static class TextAnimState
    {
        public const int IdleOffscreen = -1;
        public const int IdleOnScreen = 0;
        public const int LeftAcross = 1;
        public const int RightAcross = 2;
        public const int RightCenter = 4;
    }
    
    [SerializeField] private GameObject _titleParent;
    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private TextMeshProUGUI _subTitle;
    [SerializeField] private Animator _titleA;
    [SerializeField] private Animator _subTitleA;
    
    private List<Tip> _newTips = System.Enum.GetValues(typeof(Tip)).Cast<Tip>().ToList();

    public IEnumerator DisplayTip()
    {
        var tip = GetUnusedTip();
        yield return StartCoroutine(DisplayTipRoutine(tip));
    }
    public void DisplayTip(Tip tip)
    {
        StartCoroutine(DisplayTipRoutine(tip));
    }

    private Tip GetUnusedTip()
    {
        var nextTip = Random.Range(0, _newTips.Count);
        var tip = _newTips[nextTip];
        _newTips.Remove(tip);
        if (_newTips.Count == 0)
        {
            _newTips = System.Enum.GetValues(typeof(Tip)).Cast<Tip>().ToList();
        }

        return tip;
    }

    private IEnumerator DisplayTipRoutine(Tip tip)
    {
        _titleParent.SetActive(true);
        
        var tipText = Tips.GetTip(tip);
        _title.text = "Tip: " + tip.ToString();
        _subTitle.text = tipText;
        

        _titleA.SetInteger(Constants.AnimState, TextAnimState.IdleOnScreen);
        _subTitleA.SetInteger(Constants.AnimState, TextAnimState.IdleOnScreen);

        yield return new WaitForSeconds(4f);
        _titleA.SetInteger(Constants.AnimState, TextAnimState.IdleOffscreen);
        _subTitleA.SetInteger(Constants.AnimState, TextAnimState.IdleOffscreen);
        _titleParent.SetActive(true);
    }
}