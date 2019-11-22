using System.Collections.Generic;
using UnityEngine;

public class DeleteButton : Selector
{
    private List<GameObject> _toDelete;
    public void SetGameObjectsToDelete(List<GameObject> toDelete)
    {
        _toDelete = toDelete;
    }

    protected override void OnClick()
    {
        base.OnClick();
        _toDelete.ForEach(Destroy);
    }
}
