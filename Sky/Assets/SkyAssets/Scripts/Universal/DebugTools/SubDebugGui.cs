using UnityEngine;

public abstract class SubDebugGui : DebugGui
{ 
    public abstract bool AllDependenciesPresent { get; }

    protected override void OnGuiEnabled()
    {
        if (GUILayout.Button("Back", ScreenSpace.LeftAlignedButtonStyle))
        {
            enabled = false;
        }
        GUILayout.Space(ScreenSpace.ScreenAdjustedFontSize);
        GUILayout.Label(GetType().ToString(), ScreenSpace.CenterAlignedLabelStyle);
    }
}