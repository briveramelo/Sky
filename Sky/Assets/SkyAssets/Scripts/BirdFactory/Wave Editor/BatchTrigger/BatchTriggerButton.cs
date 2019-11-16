using BRM.DebugAdapter;
using BRM.EventBrokers;
using BRM.EventBrokers.Interfaces;
using UnityEngine;

namespace BRM.Sky.WaveEditor
{
    public class BatchTriggerButton : Selector
    {
        [SerializeField] private BatchTriggerDropdown _triggerDropdown;

        protected override void OnClick()
        {
            base.OnClick();
            _triggerDropdown.ToggleDisplay(!_triggerDropdown.gameObject.activeInHierarchy);
        }
    }
}

