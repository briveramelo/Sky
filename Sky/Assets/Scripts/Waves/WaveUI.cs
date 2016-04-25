using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public interface IWaveUI {
    IEnumerator AnimateWaveStart(WaveName waveName);
    IEnumerator AnimateWaveEnd(WaveName waveName);
}

public enum WaveName {
    Pigeon =0,
    Duck =1,
    Pigeuck =2,
    Seagull =3,
    Pelican = 4,
    Shoebill = 5,
    Bat = 6,
    Endless = 7
}

public enum TextAnimState {
    Idle_OnScreen=-1,
    Idle_Offscreen =0,
    LeftAcross = 1,
    RightAcross = 2,
    RightCenter =4
}

public enum PointAnimState {
    Idle =0,
    Shine =1,
    Poof =2
}

public class WaveUI : MonoBehaviour, IWaveUI {

    void OnLevelWasLoaded(int level) {
        if (level == (int)Scenes.Menu) {
            StopAllCoroutines();
            ClearText();
        }
    }

	[SerializeField] Text Title, SubTitle, PointTotal, Streak, Combo;
    [SerializeField] Animator TitleA, SubTitleA, PointTotalA, StreakA, ComboA;

    Dictionary<WaveName, string> WaveSubtitles = new Dictionary<WaveName, string>() {
        {WaveName.Pigeon,       "Rats of the sky" },
        {WaveName.Duck,         "Simple but foul beasts" },
        {WaveName.Pigeuck,      "There will be blood" },
        {WaveName.Seagull,      "Prepare for a shitstorm" },
        {WaveName.Pelican,      "Divebombing" },
        {WaveName.Shoebill,     "Dumb as rocks, and they hit just as hard" },
        {WaveName.Bat,          "Erratic " },
        {WaveName.Endless,      "Indulge yourself" }
    };

    IEnumerator IWaveUI.AnimateWaveStart(WaveName waveName) {
        Title.text = waveName.ToString() + " Wave";
        SubTitle.text = WaveSubtitles[waveName];
        TitleA.SetInteger("AnimState", (int)TextAnimState.RightAcross);
        SubTitleA.SetInteger("AnimState", (int)TextAnimState.LeftAcross);
        while (TitleA.GetInteger("AnimState") == (int)TextAnimState.RightAcross) {
            yield return null;
        }
    }

    IEnumerator IWaveUI.AnimateWaveEnd(WaveName waveName) {
        Title.text += " Complete";
        TitleA.SetInteger("AnimState", (int)TextAnimState.RightCenter);
        //move progress sprites to the center 
        while (TitleA.GetInteger("AnimState") == (int)TextAnimState.RightCenter) {
            yield return null;
        }
        yield return StartCoroutine (DisplayPoints());
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine (DisplayTip());
        TitleA.SetInteger("AnimState", (int)TextAnimState.Idle_Offscreen);
        SubTitleA.SetInteger("AnimState", (int)TextAnimState.Idle_Offscreen);
        yield return new WaitForSeconds(2f);
    }

    

    IEnumerator DisplayPoints() {
        PointTotal.text = "WAVE SCORE: " + ScoreSheet.Reporter.GetScore(ScoreType.Total, true, BirdType.All).ToString();
        Streak.text = "Streaks: " + ScoreSheet.Reporter.GetScore(ScoreType.Streak, true, BirdType.All).ToString();
        Combo.text = "Combos: " + ScoreSheet.Reporter.GetScore(ScoreType.Combo, true, BirdType.All).ToString();

        PointTotalA.SetInteger("AnimState", (int)PointAnimState.Shine);
        StreakA.SetInteger("AnimState", (int)PointAnimState.Shine);
        ComboA.SetInteger("AnimState", (int)PointAnimState.Shine);

        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(WaitForInput());

        PointTotalA.SetInteger("AnimState", (int)PointAnimState.Poof);
        StreakA.SetInteger("AnimState", (int)PointAnimState.Poof);
        ComboA.SetInteger("AnimState", (int)PointAnimState.Poof);
    }

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

        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(WaitForInput());
    }

    IEnumerator WaitForInput() {
        while (true) {
            if (Input.touches.Length>0) {
                break;
            }
            yield return null;
        }
    }

    void ClearText() {
        TitleA.SetInteger("AnimState", (int)TextAnimState.Idle_Offscreen);
        SubTitleA.SetInteger("AnimState", (int)TextAnimState.Idle_Offscreen);

        PointTotal.text = "";
        Streak.text = "";
        Combo.text = "";
    }
}
