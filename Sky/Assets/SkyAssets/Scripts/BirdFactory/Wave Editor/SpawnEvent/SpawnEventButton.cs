using UnityEngine;

namespace BRM.Sky.WaveEditor
{
    public class SpawnEventButton : Selector
    {
        [SerializeField] private SpawnEventUi _ui;

        protected override void OnClick()
        {
            base.OnClick();
            _ui.UpdateUi();
        }
    }
}