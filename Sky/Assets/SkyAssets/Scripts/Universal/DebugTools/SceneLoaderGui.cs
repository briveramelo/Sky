using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderGui : SubDebugGui
{
    public override bool AllDependenciesPresent => true;

    protected override void OnGuiEnabled()
    {
        base.OnGuiEnabled();
        var sceneCount = SceneManager.sceneCountInBuildSettings;
        for (int i = 0; i < sceneCount; i++)
        {
            var scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            var sceneRegexMatch = Regex.Match(scenePath, @"([\w]*)\.unity");
            var sceneName = sceneRegexMatch.Value.Substring(0, sceneRegexMatch.Value.LastIndexOf('.'));
            var sceneDisplayName = $"{i}. {sceneName}";
            if (GUILayout.Button(sceneDisplayName, ScreenSpace.LeftAlignedButtonStyle))
            {
                SceneManager.LoadScene(i);
            }
        }
    }
}
