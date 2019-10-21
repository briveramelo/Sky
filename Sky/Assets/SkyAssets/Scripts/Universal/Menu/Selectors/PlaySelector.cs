using UnityEngine;
using System.Collections;

public class PlaySelector : Selector
{
    [SerializeField] private Pauser _pauser;

    protected override void OnClick()
    {
        _pauser.gameObject.SetActive(true);
        _pauser.UnPause();
    }
}