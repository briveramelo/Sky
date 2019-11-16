using UnityEngine;

namespace BRM.Sky.WaveEditor
{
    public class AddSpawnEventButton : Selector
    {
        [SerializeField] private GameObject _spawnEventButton;
        [SerializeField] private Transform _prefabInstanceParentTran;

        private IHoldData _lastDataHolder;
        
        protected override void OnClick()
        {
            base.OnClick();
            var button = Instantiate(_spawnEventButton, _prefabInstanceParentTran);
            transform.SetAsLastSibling();

            _lastDataHolder = button.GetComponent<IHoldData>();
        }

        private void Update()
        {
            if (_lastDataHolder == null || ReferenceEquals(null, _lastDataHolder))
            {
                return;
            }

            _button.interactable = _lastDataHolder.IsDataReady;
        }
    }
}

