using UnityEngine;

namespace BRM.Sky.WaveEditor
{
    public class TriggerViewController : Selector
    {
        [SerializeField] private TriggerUi _ui;
        protected override void OnClick()
        {
            base.OnClick();
            _ui.UpdateUi();
        }
    }
}