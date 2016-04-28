using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public interface IWaveUI {
    IEnumerator AnimateWaveStart(WaveName waveName);
    IEnumerator AnimateWaveEnd(WaveName waveName);
    IEnumerator AnimateStoryEnd();
}

#region enums
public enum WaveName {
    Pigeon =0,
    Duck =1,
    Pigeuck =2,
    Seagull =3,
    Pelican = 4,
    Shoebill = 5,
    Bat = 6,
    Eagle =7,
    Complete = 8,
    Endless = 9,
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
#endregion

public class WaveUI : MonoBehaviour, IWaveUI {

	[SerializeField] Text Title, SubTitle, PointTotal, Streak, Combo;
    [SerializeField] Animator TitleA, SubTitleA, PointTotalA, StreakA, ComboA;

    #region Load Level
    void OnLevelWasLoaded(int level) {
        if (level == (int)Scenes.Menu) {
            StopAllCoroutines();
            ClearText();
        }
    }
    void ClearText() {
        TitleA.SetInteger("AnimState", (int)TextAnimState.Idle_Offscreen);
        SubTitleA.SetInteger("AnimState", (int)TextAnimState.Idle_Offscreen);

        PointTotal.text = "";
        Streak.text = "";
        Combo.text = "";
    }
    #endregion

    #region Wave Subtitles
    Dictionary<WaveName, string> WaveSubtitles = new Dictionary<WaveName, string>() {
        {WaveName.Pigeon,       "Rats of the sky" },
        {WaveName.Duck,         "Simple but foul beasts" },
        {WaveName.Pigeuck,      "There will be blood" },
        {WaveName.Seagull,      "Prepare for a shitstorm" },
        {WaveName.Pelican,      "Divebombing" },
        {WaveName.Shoebill,     "Dumb as rocks, and they hit just as hard" },
        {WaveName.Bat,          "Erratic " },
        {WaveName.Eagle,        "His time has come..." },
        {WaveName.Endless,      "Indulge yourself" }
    };
    #endregion

    IEnumerator IWaveUI.AnimateWaveStart(WaveName waveName) {
        yield return StartCoroutine(DisplayTip());
        yield return null;
        yield return StartCoroutine(DisplayWaveName(waveName));
    }
    IEnumerator IWaveUI.AnimateWaveEnd(WaveName waveName) {
        yield return StartCoroutine (DisplayWaveComplete());
        yield return null;
        yield return StartCoroutine (DisplayPoints(true));
    }

    #region AnimateWaveStart
    List<Tip> NewTips = System.Enum.GetValues(typeof(Tip)).Cast<Tip>().ToList();
    IEnumerator DisplayTip() {
        int nextTip = Random.Range(0, NewTips.Count);
        Title.text = "Tip: " + NewTips[nextTip].ToString();
        SubTitle.text = Tips.GetTip(NewTips[nextTip]);;
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
    IEnumerator DisplayWaveName(WaveName waveName) {
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
    IEnumerator DisplayWaveComplete() {
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

        PointTotalA.SetInteger("AnimState", (int)PointAnimState.Shine);
        StreakA.SetInteger("AnimState", (int)PointAnimState.Shine);
        ComboA.SetInteger("AnimState", (int)PointAnimState.Shine);

        yield return new WaitForSeconds(4f);

        PointTotalA.SetInteger("AnimState", (int)PointAnimState.Poof);
        StreakA.SetInteger("AnimState", (int)PointAnimState.Poof);
        ComboA.SetInteger("AnimState", (int)PointAnimState.Poof);
        yield return new WaitForSeconds(2f);
    }
    IEnumerator IWaveUI.AnimateStoryEnd() {
        Title.text = "Story Complete";
        TitleA.SetInteger("AnimState", (int)TextAnimState.RightCenter);
         
        while (TitleA.GetInteger("AnimState") == (int)TextAnimState.RightCenter) {
            yield return null;
        }
    }
    #endregion
}
