using UnityEngine;
using System.Collections;

public class GameClock : Singleton<GameClock>
{
    public void SlowTime(float slowDuration, float timeScale)
    {
        StopAllCoroutines();
        StartCoroutine(Wait4RealSeconds(slowDuration, timeScale));
    }

    private IEnumerator Wait4RealSeconds(float slowDuration, float timeScale)
    {
        TimeScale = timeScale;
        yield return new WaitForSeconds(slowDuration);
        TimeScale = 1f;
    }

    public static float TimeScale
    {
        get => Time.timeScale;
        set => Time.timeScale = value;
    }
}