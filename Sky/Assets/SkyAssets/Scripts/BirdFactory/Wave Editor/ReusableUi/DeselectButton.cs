using UnityEngine;

namespace BRM.Sky.WaveEditor
{
    public class DeselectButton : Selector
    {
        [SerializeField] private GameObject _toHide;

        protected override void OnClick()
        {
            base.OnClick();
            _toHide.SetActive(false);
        }
    }
}