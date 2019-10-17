﻿using System;
using UnityEngine.SceneManagement;

public class ScoreBoard : PointDisplay {
    private void Awake()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode) {
        if (scene.name == Scenes.Menu) {
            myText.text = "";
        }
    }

	protected override void DisplayPoints(int points){
		myText.text = points.ToString();
	}
}
