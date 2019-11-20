using UnityEngine;

namespace BRM.Sky.WaveEditor
{
    public class TriggerViewController : Selector
    {
        [SerializeField] private TriggerView _ui;
        protected override void OnClick()
        {
            base.OnClick();
            _ui.UpdateDisplayText();
        }
    }
}