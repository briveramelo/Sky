using System.Linq;
using GenericFunctions;
using TMPro;
using UnityEngine;

namespace BRM.Sky.WaveEditor
{
    public class AddBatchButton : Selector
    {
        [SerializeField] private GameObject _spacerPrefab;
        [SerializeField] private GameObject _batchButtonPrefab;
        [SerializeField] private GameObject _batchTriggerButtonPrefab;
        [SerializeField] private GameObject _spawnEventsParent;
        [SerializeField] private GameObject _spawnEventsDirectParent;
        [SerializeField] private Transform _prefabInstanceParentTran;
        [SerializeField] private Transform _maskAvoiderTargetParent;
        [SerializeField] private SaveBatchButton _saveBatchButton;
        [SerializeField] private AddSpawnEventButton _spawnEventButton;
        [SerializeField] private TMP_InputField _batchNameText;
        
        private int _currentId = 0;

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
            viewController.Initialize(_spawnEventsParent, _spawnEventsDirectParent, _spawnEventButton, _batchNameText, _currentId++);
            viewController.OnButtonClicked += DeselectOthersAndSetMarshal;
        }

        private void DeselectOthersAndSetMarshal(int selectedId)
        {
            _prefabInstanceParentTran.GetComponentsRecursively<BatchViewController>(true).ForEach(item =>
            {
                if (item.Id != selectedId)
                {
                    item.Select(false);
                }
                else
                {
                    _saveBatchButton.SetDataMarshal(item.GetComponent<BatchDataMarshal>());
                }
            });
        }
    }
}