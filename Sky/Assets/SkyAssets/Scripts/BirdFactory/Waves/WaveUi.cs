﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GenericFunctions;
using TMPro;
using UnityEngine.SceneManagement;

public interface IWaveUi
{
    IEnumerator AnimateWaveStart(WaveName waveName);
    IEnumerator AnimateWaveEnd(WaveName waveName);
    IEnumerator AnimateStoryStart();
    IEnumerator AnimateStoryEnd();
}
public class WaveUi : MonoBehaviour, IWaveUi
{
    private static class TextAnimState
    {
        public const int IdleOffscreen = -1;
        public const int IdleOnScreen = 0;
        public const int LeftAcross = 1;
        public const int RightAcross = 2;
        public const int RightCenter = 4;
    }
    private static class PointAnimState
    {
        public const int Idle = 0;
        public const int Shine = 1;
        public const int Poof = 2;
    }
    private static class PointBackDrop
    {
        public const int IdleOffScreen = 0;
        public const int ComeIn = 1;
        public const int GetOut = 2;
    }

    [SerializeField] private GameObject _pointsParent;
    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private TextMeshProUGUI _subTitle;
    [SerializeField] private TextMeshProUGUI _pointTotal;
    [SerializeField] private TextMeshProUGUI _combo;
    [SerializeField] private TextMeshProUGUI _streak;
    [SerializeField] private Animator _titleA;
    [SerializeField] private Animator _subTitleA;
    [SerializeField] private Animator _pointTotalA;
    [SerializeField] private Animator _streakA;
    [SerializeField] private Animator _comboA;
    [SerializeField] private Animator _scoreBackDrop;
    
    private bool _hasWeapon;
    private List<Tip> _newTips = System.Enum.GetValues(typeof(Tip)).Cast<Tip>().ToList();

    #region Load Level

    private void Awake()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        StopAllCoroutines();
        ClearText();
        _pointsParent.SetActive(false);
    }

    private void ClearText()
    {
        _titleA.SetInteger(Constants.AnimState, TextAnimState.IdleOffscreen);
        _subTitleA.SetInteger(Constants.AnimState, TextAnimState.IdleOffscreen);

        _pointTotal.text = "";
        _streak.text = "";
        _combo.text = "";
    }

    #endregion

    #region Animate Story Start

    

    #endregion

    IEnumerator IWaveUi.AnimateWaveStart(WaveName waveName)
    {
        yield return StartCoroutine(DisplayWaveName(waveName));
    }

    IEnumerator IWaveUi.AnimateWaveEnd(WaveName waveName)
    {
        yield return StartCoroutine(DisplayWaveComplete());
        yield return null;
        yield return StartCoroutine(DisplayPoints(true));
        yield return null;
        yield return StartCoroutine(DisplayTip());
    }

    #region AnimateWaveStart

    private IEnumerator DisplayTip()
    {
        var nextTip = Random.Range(0, _newTips.Count);
        _title.text = "Tip: " + _newTips[nextTip].ToString();
        _subTitle.text = Tips.GetTip(_newTips[nextTip]);
        _newTips.RemoveAt(nextTip);
        if (_newTips.Count == 0)
        {
            _newTips = System.Enum.GetValues(typeof(Tip)).Cast<Tip>().ToList();
        }

        _titleA.SetInteger(Constants.AnimState, TextAnimState.IdleOnScreen);
        _subTitleA.SetInteger(Constants.AnimState, TextAnimState.IdleOnScreen);

        yield return new WaitForSeconds(4f);
        _titleA.SetInteger(Constants.AnimState, TextAnimState.IdleOffscreen);
        _subTitleA.SetInteger(Constants.AnimState, TextAnimState.IdleOffscreen);
    }

    private IEnumerator DisplayWaveName(WaveName waveName)
    {
        _title.text = waveName.ToString() + " Wave";
        _subTitle.text = WaveLabels.GetWaveSubtitle(waveName);
        _titleA.SetInteger(Constants.AnimState, TextAnimState.RightAcross);
        _subTitleA.SetInteger(Constants.AnimState, TextAnimState.LeftAcross);
        while (_titleA.GetInteger(Constants.AnimState) == TextAnimState.RightAcross)
        {
            yield return null;
        }
    }

    #endregion

    #region AnimateWaveEnd

    private IEnumerator DisplayWaveComplete()
    {
        _title.text += " Complete";
        _titleA.SetInteger(Constants.AnimState, TextAnimState.RightCenter);
        //move progress sprites to the center 
        while (_titleA.GetInteger(Constants.AnimState) == TextAnimState.RightCenter)
        {
            yield return null;
        }
    }

    public IEnumerator DisplayPoints(bool isWaveScore)
    {
        _pointsParent.SetActive(true);
        var scoreType = isWaveScore ? "Wave" : "Total";
        
        var pointTotalPrefix = scoreType + " Score: ";
        const string streakPrefix = "Streak Points: ";
        const string comboPrefix = "Combo Points: ";
        int peakCharLength = Mathf.Max(pointTotalPrefix.Length, streakPrefix.Length, comboPrefix.Length);
        
        _streak.text = streakPrefix.PadRight(peakCharLength) + ScoreSheet.Reporter.GetScore(ScoreType.Streak, isWaveScore, BirdType.All).ToString();
        _combo.text = comboPrefix.PadRight(peakCharLength) + ScoreSheet.Reporter.GetScore(ScoreType.Combo, isWaveScore, BirdType.All).ToString();
        _pointTotal.text = pointTotalPrefix.PadRight(peakCharLength) + ScoreSheet.Reporter.GetScore(ScoreType.Total, isWaveScore, BirdType.All).ToString();

        _scoreBackDrop.SetInteger(Constants.AnimState, PointBackDrop.ComeIn);
        _pointTotalA.SetInteger(Constants.AnimState, PointAnimState.Shine);
        _streakA.SetInteger(Constants.AnimState, PointAnimState.Shine);
        _comboA.SetInteger(Constants.AnimState, PointAnimState.Shine);

        yield return new WaitForSeconds(4f);

        _scoreBackDrop.SetInteger(Constants.AnimState, PointBackDrop.GetOut);
        _pointTotalA.SetInteger(Constants.AnimState, PointAnimState.Poof);
        _streakA.SetInteger(Constants.AnimState, PointAnimState.Poof);
        _comboA.SetInteger(Constants.AnimState, PointAnimState.Poof);
        yield return new WaitForSeconds(2f);
        _scoreBackDrop.SetInteger(Constants.AnimState, PointBackDrop.IdleOffScreen);
    }

    #endregion

    IEnumerator IWaveUi.AnimateStoryStart()
    {
        yield return null;
    }

    IEnumerator IWaveUi.AnimateStoryEnd()
    {
        _title.text = "Story Complete";
        _titleA.SetInteger(Constants.AnimState, TextAnimState.RightCenter);

        while (_titleA.GetInteger(Constants.AnimState) == TextAnimState.RightCenter)
        {
            yield return null;
        }
    }
}