using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private void Start()
    {
        Application.targetFrameRate = 60;
    }
}