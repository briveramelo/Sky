using UnityEngine;

namespace BRM.Sky.WaveEditor
{
    public class DisplayToggleButton : Selector
    {
        [SerializeField] private GameObject _display;

        protected override void OnClick()
        {
            base.OnClick();
            _display.SetActive(!_display.activeInHierarchy);
        }
    }
}

