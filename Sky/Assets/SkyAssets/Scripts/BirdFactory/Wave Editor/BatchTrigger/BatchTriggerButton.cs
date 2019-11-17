using UnityEngine;

namespace BRM.Sky.WaveEditor
{
    public class BatchTriggerButton : Selector
    {
        [SerializeField] private BatchTriggerDropdown _dropdown;
        [SerializeField] private BatchTriggerInputField _inputField;


        private void Start()
        {
            
        }

        protected override void OnClick()
        {
            base.OnClick();
            _inputField.ToggleDisplay(!_inputField.gameObject.activeInHierarchy);
            _dropdown.ToggleDisplay(!_dropdown.gameObject.activeInHierarchy);
        }
    }
}

