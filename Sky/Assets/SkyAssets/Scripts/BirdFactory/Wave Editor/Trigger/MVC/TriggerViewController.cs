using UnityEngine;

namespace BRM.Sky.WaveEditor
{
    public class TriggerViewController : SelectorViewController<TriggerView>
    {
        [SerializeField] private TriggerView _ui;
        protected override TriggerView View => _ui;
        public void Initialize()
        {
            View.Initialize();
        }

        protected override void OnClick()
        {
            base.OnClick();
            View.UpdateDisplayText();
        }
    }
}