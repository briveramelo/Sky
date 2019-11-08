using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class BackSelector : Selector
{
    protected override IEnumerator OnClickRoutine()
    {
        yield return null;
        SceneManager.LoadScene(Scenes.Menu);
    }
}