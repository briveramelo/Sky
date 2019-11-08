using System;
using System.Linq;
using UnityEngine;

public class BirdFactoryGui : SubDebugGui
{
    public override bool AllDependenciesPresent => BirdFactory.Instance != null;
    
    protected override void OnGuiEnabled()
    {
        base.OnGuiEnabled();
        
        var birdTypes = Enum.GetValues(typeof(BirdType)).Cast<BirdType>().OrderBy(item => item.ToString());
        
        foreach (var birdType in birdTypes)
        {
            if (birdType == BirdType.All)
            {
                continue;
            }
            //GUIStyle style = new GUIStyle("button");
            // or
            

            if (GUILayout.Button(birdType.ToString(), ScreenSpace.LeftAlignedButtonStyle))
            {
                BirdFactory.Instance.CreateNextBird(birdType);
            }
        }
    }
}