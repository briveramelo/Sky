using System.Linq;
using GenericFunctions;
using UnityEngine;

namespace BRM.Sky.WaveEditor
{
    public class AddBatchButton : Selector
    {
        [SerializeField] private GameObject _spacerPrefab;
        [SerializeField] private GameObject _batchButtonPrefab;
        [SerializeField] private GameObject _batchTriggerButtonPrefab;
        [SerializeField] private GameObject _spawnEventsParent;
        [SerializeField] private Transform _prefabInstanceParentTran;
        [SerializeField] private Transform _maskAvoiderTargetParent;

        private IHoldData _lastDataHolder;

        protected override void OnClick()
        {
            base.OnClick();
            var batchButton = Instantiate(_batchButtonPrefab, _prefabInstanceParentTran);
            var batchTrigger = Instantiate(_batchTriggerButtonPrefab, _prefabInstanceParentTran);
            Instantiate(_spacerPrefab, _prefabInstanceParentTran);
            
            transform.SetAsLastSibling();

            batchButton.GetComponentsInChildren<UiMaskAvoider>(true).ToList().ForEach(item => item.SetTargetParent(_maskAvoiderTargetParent));
            batchTrigger.GetComponentsInChildren<UiMaskAvoider>(true).ToList().ForEach(item => item.SetTargetParent(_maskAvoiderTargetParent));

            var viewController = batchButton.GetComponent<BatchViewController>();
            viewController.SetSpawnEventsParent(_spawnEventsParent);
            viewController.OnButtonClicked += DeselectAll;
            
            _lastDataHolder = batchButton.GetComponent<IHoldData>();
        }

        private void DeselectAll()
        {
            _prefabInstanceParentTran.GetComponentsRecursively<BatchViewController>().ForEach(item => item.Select(false));
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