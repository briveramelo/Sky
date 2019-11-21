using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GenericFunctions;
using TMPro;
using UnityEngine.SceneManagement;
using static ScoreSheet;

public class WaveUi : MonoBehaviour
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

    [SerializeField] private TipUi _tipUi;
    [SerializeField] private GameObject _pointsParent;
    [SerializeField] private GameObject _titleParent;
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

    private Dictionary<WavePhase, string> _phaseLabels = new Dictionary<WavePhase, string>
    {
        {WavePhase.AllTime, "All Time"},
        {WavePhase.CurrentPlaySession, "Current Play Session"},
        {WavePhase.CurrentRun, "Current Run"},
        {WavePhase.CurrentWave, "Current Wave"},
        {WavePhase.CurrentBatch, "Current Batch"},
    };

    private bool _hasWeapon;

    #region Load Level

    private void Awake()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
        _pointsParent.SetActive(false);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if (Scenes.IsMenu(scene.name))
        {
            StopAllCoroutines();
            ClearText();
            _pointsParent.SetActive(false);
        }
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

    public IEnumerator AnimateWaveStart(string waveName, string subtitle)
    {
        yield return StartCoroutine(DisplayWaveName(waveName, subtitle));
    }

    public IEnumerator AnimateWaveEnd()
    {
        yield return StartCoroutine(DisplayWaveComplete());
        yield return null;
        yield return StartCoroutine(DisplayPoints(WavePhase.CurrentWave));
        yield return null;
        yield return StartCoroutine(_tipUi.DisplayTip());
    }

    #region AnimateWaveStart

    private IEnumerator DisplayWaveName(string waveName, string waveSubtitle)
    {
        _titleParent.SetActive(true);
        _title.text = waveName;
        _subTitle.text = waveSubtitle;
        _titleA.SetInteger(Constants.AnimState, TextAnimState.RightAcross);
        _subTitleA.SetInteger(Constants.AnimState, TextAnimState.LeftAcross);
        while (_titleA.GetInteger(Constants.AnimState) == TextAnimState.RightAcross)
        {
            yield return null;
        }

        _titleParent.SetActive(false);
    }

    #endregion

    #region AnimateWaveEnd

    private IEnumerator DisplayWaveComplete()
    {
        _title.text = $"{_title.text} Complete";
        _titleA.SetInteger(Constants.AnimState, TextAnimState.RightCenter);
        //move progress sprites to the center 
        while (_titleA.GetInteger(Constants.AnimState) == TextAnimState.RightCenter)
        {
            yield return null;
        }
    }

    public IEnumerator DisplayPoints(WavePhase phase)
    {
        _pointsParent.SetActive(true);
        var scoreType = _phaseLabels.SafeGet(phase);

        var pointTotalPrefix = $"{scoreType} Score: ";
        const string streakPrefix = "Streak Points: ";
        const string comboPrefix = "Combo Points: ";
        int peakCharLength = Mathf.Max(pointTotalPrefix.Length, streakPrefix.Length, comboPrefix.Length);

        _streak.text = $"{streakPrefix.PadRight(peakCharLength)}{Reporter.GetScore(ScoreCounterType.ScoreStreak, phase)}";
        _combo.text = $"{comboPrefix.PadRight(peakCharLength)}{Reporter.GetScore(ScoreCounterType.ScoreCombo, phase)}";
        _pointTotal.text = $"{pointTotalPrefix.PadRight(peakCharLength)}{Reporter.GetScore(ScoreCounterType.ScoreTotal, phase)}";

        _pointTotalA.SetInteger(Constants.AnimState, PointAnimState.Shine);
        _streakA.SetInteger(Constants.AnimState, PointAnimState.Shine);
        _comboA.SetInteger(Constants.AnimState, PointAnimState.Shine);

        yield return new WaitForSeconds(4f);
        _pointTotalA.SetInteger(Constants.AnimState, PointAnimState.Poof);
        _streakA.SetInteger(Constants.AnimState, PointAnimState.Poof);
        _comboA.SetInteger(Constants.AnimState, PointAnimState.Poof);
        yield return new WaitForSeconds(2f);
        _pointsParent.SetActive(false);
    }

    #endregion

    public IEnumerator AnimateStoryStart()
    {
        yield return null;
    }

    public IEnumerator AnimateStoryEnd()
    {
        _title.text = "Story Complete";
        _titleA.SetInteger(Constants.AnimState, TextAnimState.RightCenter);

        while (_titleA.GetInteger(Constants.AnimState) == TextAnimState.RightCenter)
        {
            yield return null;
        }
    }
}