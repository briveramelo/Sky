using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public interface IWaveUI {
    IEnumerator AnimateWaveStart(WaveName waveName);
    IEnumerator AnimateWaveEnd(WaveName waveName);
    IEnumerator AnimateStoryStart();
    IEnumerator AnimateStoryEnd();
    void GrabbedWeapon();
}

#region enums
public enum WaveName {
    Intro = 0,
    Pigeon =1,
    Duck =2,
    Pigeuck =3,
    Seagull =4,
    Pelican = 5,
    Shoebill = 6,
    Bat = 7,
    Eagle =8,
    Complete = 9,
    Endless = 10,
}

public enum TextAnimState {
    Idle_Offscreen =-1,
    Idle_OnScreen =0,
    LeftAcross = 1,
    RightAcross = 2,
    RightCenter =4
}

public enum PointAnimState {
    Idle =0,
    Shine =1,
    Poof =2
}

internal enum PointBackDrop {
    IdleOffScreen=0,
    ComeIn =1,
    GetOut =2
}
#endregion

public class WaveUI : MonoBehaviour, IWaveUI {

	[SerializeField] private Text Title, SubTitle, PointTotal, Streak, Combo;
    [SerializeField] private Animator TitleA, SubTitleA, PointTotalA, StreakA, ComboA, ScoreBackDrop;
    [SerializeField] private GameObject joystickHelp, swipeHelp;

    #region Load Level

    private void Awake()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode) {
        if (scene.name ==Scenes.Menu) {
            StopAllCoroutines();
            ClearText();
        }
    }

    private void ClearText() {
        TitleA.SetInteger("AnimState", (int)TextAnimState.Idle_Offscreen);
        SubTitleA.SetInteger("AnimState", (int)TextAnimState.Idle_Offscreen);

        PointTotal.text = "";
        Streak.text = "";
        Combo.text = "";
        joystickHelp.SetActive(false);
        swipeHelp.SetActive(false);
    }
    #endregion

    #region Wave Subtitles

    private Dictionary<WaveName, string> WaveSubtitles = new Dictionary<WaveName, string>() {
        {WaveName.Intro,        "Rescue your son!" },
        {WaveName.Pigeon,       "Clear out the sky rats" },
        {WaveName.Duck,         "Carve through these meatsacks" },
        {WaveName.Pigeuck,      "Get bloody" },
        {WaveName.Seagull,      "Prepare for a shitstorm" },
        {WaveName.Pelican,      "Watch your head" },
        {WaveName.Shoebill,     "Hold on to your butts!" },
        {WaveName.Bat,          "Tread lightly" },
        {WaveName.Eagle,        "Make him pay..." },
        {WaveName.Endless,      "Indulge yourself" }
    };
    #endregion

    #region Animate Story Start

    private bool hasWeapon;
    void IWaveUI.GrabbedWeapon() {
        hasWeapon = true;
    }
    IEnumerator IWaveUI.AnimateStoryStart() {
        if (!hasWeapon) {
            yield return new WaitForSeconds(1f);
            yield return ShowHelpTip(activate=>joystickHelp.SetActive(activate));
            while (!hasWeapon) {
                yield return null;
            }
            yield return ShowHelpTip(activate=>swipeHelp.SetActive(activate));
        }
    }

    private IEnumerator ShowHelpTip(System.Action<bool> lambda) {
        lambda(true);
        yield return new WaitForSeconds(5.5f);
        lambda(false);
    }
    #endregion

    IEnumerator IWaveUI.AnimateWaveStart(WaveName waveName) {
        yield return StartCoroutine(DisplayWaveName(waveName));
    }
    IEnumerator IWaveUI.AnimateWaveEnd(WaveName waveName) {
        yield return StartCoroutine (DisplayWaveComplete());
        yield return null;
        yield return StartCoroutine (DisplayPoints(true));
        yield return null;
        yield return StartCoroutine(DisplayTip());
    }

    #region AnimateWaveStart

    private List<Tip> NewTips = System.Enum.GetValues(typeof(Tip)).Cast<Tip>().ToList();

    private IEnumerator DisplayTip() {
        int nextTip = Random.Range(0, NewTips.Count);
        Title.text = "Tip: " + NewTips[nextTip].ToString();
        SubTitle.text = Tips.GetTip(NewTips[nextTip]);
        NewTips.RemoveAt(nextTip);
        if (NewTips.Count==0) {
            NewTips = System.Enum.GetValues(typeof(Tip)).Cast<Tip>().ToList();
        }
        TitleA.SetInteger("AnimState", (int)TextAnimState.Idle_OnScreen);
        SubTitleA.SetInteger("AnimState", (int)TextAnimState.Idle_OnScreen);

        yield return new WaitForSeconds(4f);
        TitleA.SetInteger("AnimState", (int)TextAnimState.Idle_Offscreen);
        SubTitleA.SetInteger("AnimState", (int)TextAnimState.Idle_Offscreen);
    }

    private IEnumerator DisplayWaveName(WaveName waveName) {
        Title.text = waveName.ToString() + " Wave";
        SubTitle.text = WaveSubtitles[waveName];
        TitleA.SetInteger("AnimState", (int)TextAnimState.RightAcross);
        SubTitleA.SetInteger("AnimState", (int)TextAnimState.LeftAcross);
        while (TitleA.GetInteger("AnimState") == (int)TextAnimState.RightAcross) {
            yield return null;
        }
    }
    #endregion

    #region AnimateWaveEnd

    private IEnumerator DisplayWaveComplete() {
        Title.text += " Complete";
        TitleA.SetInteger("AnimState", (int)TextAnimState.RightCenter);
        //move progress sprites to the center 
        while (TitleA.GetInteger("AnimState") == (int)TextAnimState.RightCenter) {
            yield return null;
        }
    }
    public IEnumerator DisplayPoints(bool isWaveScore) {
        string scoretype = isWaveScore ? "WAVE" : "TOTAL";
        PointTotal.text = scoretype + " SCORE: " + ScoreSheet.Reporter.GetScore(ScoreType.Total, isWaveScore, BirdType.All).ToString();
        Streak.text = "Streaks: " + ScoreSheet.Reporter.GetScore(ScoreType.Streak, isWaveScore, BirdType.All).ToString();
        Combo.text = "Combos: " + ScoreSheet.Reporter.GetScore(ScoreType.Combo, isWaveScore, BirdType.All).ToString();

        ScoreBackDrop.SetInteger("AnimState", (int)PointBackDrop.ComeIn);
        PointTotalA.SetInteger("AnimState", (int)PointAnimState.Shine);
        StreakA.SetInteger("AnimState", (int)PointAnimState.Shine);
        ComboA.SetInteger("AnimState", (int)PointAnimState.Shine);

        yield return new WaitForSeconds(4f);

        ScoreBackDrop.SetInteger("AnimState", (int)PointBackDrop.GetOut);
        PointTotalA.SetInteger("AnimState", (int)PointAnimState.Poof);
        StreakA.SetInteger("AnimState", (int)PointAnimState.Poof);
        ComboA.SetInteger("AnimState", (int)PointAnimState.Poof);
        yield return new WaitForSeconds(2f);
        ScoreBackDrop.SetInteger("AnimState", (int)PointBackDrop.IdleOffScreen);
    }
    #endregion

    IEnumerator IWaveUI.AnimateStoryEnd() {
        Title.text = "Story Complete";
        TitleA.SetInteger("AnimState", (int)TextAnimState.RightCenter);
         
        while (TitleA.GetInteger("AnimState") == (int)TextAnimState.RightCenter) {
            yield return null;
        }
    }
}
