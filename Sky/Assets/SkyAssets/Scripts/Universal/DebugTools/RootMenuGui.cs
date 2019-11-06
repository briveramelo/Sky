using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RootMenuGui : DebugGui
{
    private List<SubDebugGui> _guis;
    
    private void Awake()
    {
        _guis = GetComponents<SubDebugGui>().ToList();
        _guis.ForEach(gui => gui.enabled = false);
    }

    protected override void OnGuiEnabled()
    {
        if (_guis.Any(gui => gui.enabled))
        {
            return;
        }

        for (int i = 0; i < _guis.Count; i++)
        {
            var guiName = _guis[i].GetType().ToString();
            if (!_guis[i].AllDependenciesPresent)
            {
                GUILayout.Label($"{guiName} missing dependencies", ScreenSpace.LeftAlignedLabelStyle);
                continue;
            }

            if (GUILayout.Button(guiName, ScreenSpace.LeftAlignedButtonStyle))
            {
                _guis[i].enabled = true;
                return;
            }
        }
    }
}
