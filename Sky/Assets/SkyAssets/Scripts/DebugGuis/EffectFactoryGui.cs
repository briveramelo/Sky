using UnityEngine;

public class EffectFactoryGui : SubDebugGui
{
    private GameObject _pooPrefab
    {
        get
        {
            var effectFactory = FindObjectOfType<EffectFactory>();
            if (effectFactory == null)
            {
                return null;
            }

            return effectFactory.PooEffectPrefab;
        }
    }


    public override bool AllDependenciesPresent => _pooPrefab != null;
    protected override void OnGuiEnabled()
    {
        base.OnGuiEnabled();

        if (GUILayout.Button("Splash Poo", ScreenSpace.LeftAlignedButtonStyle))
        {
            Instantiate(_pooPrefab, Vector2.zero, Quaternion.identity);
        }
    }
}