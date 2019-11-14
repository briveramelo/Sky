using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RootMenuGui : DebugGui
{
    private List<SubDebugGui> _guis;
    private const int _numSimulTouches = 4;

    public override bool CanGuiDisplay { get; set; }

    private void Awake()
    {
        if (Debug.isDebugBuild)
        {
            _guis = GetComponents<SubDebugGui>().ToList();
            _guis.ForEach(gui => gui.CanGuiDisplay = false);
        }
        else
        {
            //remove debug components in release build
            Destroy(gameObject);
        }
    }
    
    private void Update()
    {
        var touchPassed = (Input.touchCount == _numSimulTouches && Input.GetTouch(_numSimulTouches - 1).phase == TouchPhase.Began);
        var mousePressed = false;//Input.GetMouseButtonDown(2);//middle click
        if (touchPassed || mousePressed)
        {
            CanGuiDisplay = !CanGuiDisplay;
            if (!CanGuiDisplay)
            {
                _guis.ForEach(gui => gui.CanGuiDisplay = false);
            }
        }
    }

    protected override void OnGuiEnabled()
    {
        if (_guis.Any(gui => gui.CanGuiDisplay))
        {
            return;
        }

        if (GUILayout.Button("Close", ScreenSpace.LeftAlignedButtonStyle))
        {
            CanGuiDisplay = false;
        }
        GUILayout.Space(ScreenSpace.ScreenAdjustedFontSize);

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
                _guis[i].CanGuiDisplay = true;
                return;
            }
        }
    }
}
