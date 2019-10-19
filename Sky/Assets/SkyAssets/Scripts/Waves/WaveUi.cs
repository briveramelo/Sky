using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public interface IWaveUi
{
    IEnumerator AnimateWaveStart(WaveName waveName);
    IEnumerator AnimateWaveEnd(WaveName waveName);
    IEnumerator AnimateStoryStart();
    IEnumerator AnimateStoryEnd();
    void GrabbedWeapon();
}

#region enums

public enum WaveName
{
    Intro = 0,
    Pigeon = 1,
    Duck = 2,
    Pigeuck = 3,
    Seagull = 4,
    Pelican = 5,
    Shoebill = 6,
    Bat = 7,
    Eagle = 8,
    Complete = 9,
    Endless = 10,
}

public enum TextAnimState
{
    IdleOffscreen = -1,
    IdleOnScreen = 0,
    LeftAcross = 1,
    RightAcross = 2,
    RightCenter = 4
}

public enum PointAnimState
{
    Idle = 0,
    Shine = 1,
    Poof = 2
}

internal enum PointBackDrop
{
    IdleOffScreen = 0,
    ComeIn = 1,
    GetOut = 2
}

#endregion

public class WaveUi : MonoBehaviour, IWaveUi
{
    [SerializeField] private Text _title;
    [SerializeField] private Text _subTitle;
    [SerializeField] private Text _pointTotal;
    [SerializeField] private Text _streak;
    [SerializeField] private Text _combo;
    [SerializeField] private Animator _titleA;
    [SerializeField] private Animator _subTitleA;
    [SerializeField] private Animator _pointTotalA;
    [SerializeField] private Animator _streakA;
    [SerializeField] private Animator _comboA;
    [SerializeField] private Animator _scoreBackDrop;
    [SerializeField] private GameObject _joystickHelp, _swipeHelp;

    private bool _hasWeapon;
    private List<Tip> _newTips = System.Enum.GetValues(typeof(Tip)).Cast<Tip>().ToList();

    private Dictionary<WaveName, string> _waveSubtitles = new Dictionary<WaveName, string>()
    {
        {WaveName.Intro, "Rescue your son!"},
        {WaveName.Pigeon, "Clear out the sky rats"},
        {WaveName.Duck, "Carve through these meatsacks"},
        {WaveName.Pigeuck, "Get bloody"},
        {WaveName.Seagull, "Prepare for a shitstorm"},
        {WaveName.Pelican, "Watch your head"},
        {WaveName.Shoebill, "Hold on to your butts!"},
        {WaveName.Bat, "Tread lightly"},
        {WaveName.Eagle, "Make him pay..."},
        {WaveName.Endless, "Indulge yourself"}
    };

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
        if (scene.name == Scenes.Menu)
        {
            StopAllCoroutines();
            ClearText();
        }
    }

    private void ClearText()
    {
        _titleA.SetInteger("AnimState", (int) TextAnimState.IdleOffscreen);
        _subTitleA.SetInteger("AnimState", (int) TextAnimState.IdleOffscreen);

        _pointTotal.text = "";
        _streak.text = "";
        _combo.text = "";
        _joystickHelp.SetActive(false);
        _swipeHelp.SetActive(false);
    }

    #endregion

    #region Animate Story Start

    void IWaveUi.GrabbedWeapon()
    {
        _hasWeapon = true;
    }

    IEnumerator IWaveUi.AnimateStoryStart()
    {
        if (!_hasWeapon)
        {
            yield return new WaitForSeconds(1f);
            yield return ShowHelpTip(activate => _joystickHelp.SetActive(activate));
            while (!_hasWeapon)
            {
                yield return null;
            }

            yield return ShowHelpTip(activate => _swipeHelp.SetActive(activate));
        }
    }

    private IEnumerator ShowHelpTip(System.Action<bool> lambda)
    {
        lambda(true);
        yield return new WaitForSeconds(5.5f);
        lambda(false);
    }

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

        _titleA.SetInteger("AnimState", (int) TextAnimState.IdleOnScreen);
        _subTitleA.SetInteger("AnimState", (int) TextAnimState.IdleOnScreen);

        yield return new WaitForSeconds(4f);
        _titleA.SetInteger("AnimState", (int) TextAnimState.IdleOffscreen);
        _subTitleA.SetInteger("AnimState", (int) TextAnimState.IdleOffscreen);
    }

    private IEnumerator DisplayWaveName(WaveName waveName)
    {
        _title.text = waveName.ToString() + " Wave";
        _subTitle.text = _waveSubtitles[waveName];
        _titleA.SetInteger("AnimState", (int) TextAnimState.RightAcross);
        _subTitleA.SetInteger("AnimState", (int) TextAnimState.LeftAcross);
        while (_titleA.GetInteger("AnimState") == (int) TextAnimState.RightAcross)
        {
            yield return null;
        }
    }

    #endregion

    #region AnimateWaveEnd

    private IEnumerator DisplayWaveComplete()
    {
        _title.text += " Complete";
        _titleA.SetInteger("AnimState", (int) TextAnimState.RightCenter);
        //move progress sprites to the center 
        while (_titleA.GetInteger("AnimState") == (int) TextAnimState.RightCenter)
        {
            yield return null;
        }
    }

    public IEnumerator DisplayPoints(bool isWaveScore)
    {
        var scoretype = isWaveScore ? "WAVE" : "TOTAL";
        _pointTotal.text = scoretype + " SCORE: " + ScoreSheet.Reporter.GetScore(ScoreType.Total, isWaveScore, BirdType.All).ToString();
        _streak.text = "Streaks: " + ScoreSheet.Reporter.GetScore(ScoreType.Streak, isWaveScore, BirdType.All).ToString();
        _combo.text = "Combos: " + ScoreSheet.Reporter.GetScore(ScoreType.Combo, isWaveScore, BirdType.All).ToString();

        _scoreBackDrop.SetInteger("AnimState", (int) PointBackDrop.ComeIn);
        _pointTotalA.SetInteger("AnimState", (int) PointAnimState.Shine);
        _streakA.SetInteger("AnimState", (int) PointAnimState.Shine);
        _comboA.SetInteger("AnimState", (int) PointAnimState.Shine);

        yield return new WaitForSeconds(4f);

        _scoreBackDrop.SetInteger("AnimState", (int) PointBackDrop.GetOut);
        _pointTotalA.SetInteger("AnimState", (int) PointAnimState.Poof);
        _streakA.SetInteger("AnimState", (int) PointAnimState.Poof);
        _comboA.SetInteger("AnimState", (int) PointAnimState.Poof);
        yield return new WaitForSeconds(2f);
        _scoreBackDrop.SetInteger("AnimState", (int) PointBackDrop.IdleOffScreen);
    }

    #endregion

    IEnumerator IWaveUi.AnimateStoryEnd()
    {
        _title.text = "Story Complete";
        _titleA.SetInteger("AnimState", (int) TextAnimState.RightCenter);

        while (_titleA.GetInteger("AnimState") == (int) TextAnimState.RightCenter)
        {
            yield return null;
        }
    }
}