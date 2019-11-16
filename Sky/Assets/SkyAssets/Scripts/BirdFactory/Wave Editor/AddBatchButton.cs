using UnityEngine;

namespace BRM.Sky.WaveEditor
{
    public class AddBatchButton : Selector
    {
        [SerializeField] private GameObject _spacerPrefab;
        [SerializeField] private GameObject _batchButtonPrefab;
        [SerializeField] private GameObject _batchTriggerButtonPrefab;
        [SerializeField] private Transform _prefabInstanceParentTran;

        private IHoldData _lastDataHolder;

        protected override void OnClick()
        {
            base.OnClick();
            Instantiate(_spacerPrefab, _prefabInstanceParentTran);
            var button = Instantiate(_batchButtonPrefab, _prefabInstanceParentTran);
            Instantiate(_batchTriggerButtonPrefab, _prefabInstanceParentTran);
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