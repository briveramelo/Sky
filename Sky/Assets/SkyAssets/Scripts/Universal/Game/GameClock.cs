using UnityEngine;
using System.Collections;
using BRM.EventBrokers;
using BRM.EventBrokers.Interfaces;

public class GameClock : Singleton<GameClock>
{
    private static IPublishEvents _eventPublisher = new StaticEventBroker();
    
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
        set
        {
            if (!Mathf.Approximately(Time.timeScale,0) && Mathf.Approximately(value,0))
            {
                _eventPublisher.Publish(new PauseData {IsPaused = true});
            }
            else if (Mathf.Approximately(Time.timeScale,0) && !Mathf.Approximately(value,0))
            {
                _eventPublisher.Publish(new PauseData {IsPaused = false});
            }
            Time.timeScale = value;
        }
    }
}