using System.Linq;
using UnityEngine;

namespace BRM.Sky.WaveEditor
{
    public class AddSpawnEventButton : Selector
    {
        [SerializeField] private GameObject _spawnEventButton;
        [SerializeField] private Transform _prefabInstanceParentTran;
        [SerializeField] private Transform _maskAvoiderTargetParent;
        
        private IHoldData _lastDataHolder;
        
        protected override void OnClick()
        {
            base.OnClick();
            var button = Instantiate(_spawnEventButton, _prefabInstanceParentTran);
            transform.SetAsLastSibling();
            
            button.GetComponentsInChildren<UiMaskAvoider>(true).ToList().ForEach(item => item.SetTargetParent(_maskAvoiderTargetParent));

            _lastDataHolder = button.GetComponent<IHoldData>();
        }

        private void Update()
        {
            if (_lastDataHolder == null || ReferenceEquals(null, _lastDataHolder))
            {
                _button.interactable = true;
                return;
            }

            _button.interactable = _lastDataHolder.IsDataReady;
        }
    }
}

