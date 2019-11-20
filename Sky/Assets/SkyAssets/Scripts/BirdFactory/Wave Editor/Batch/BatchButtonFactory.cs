using System.Linq;
using GenericFunctions;
using TMPro;
using UnityEngine;

namespace BRM.Sky.WaveEditor.Ui
{
    public class BatchButtonFactory : Selector
    {
        [SerializeField] private GameObject _spacerPrefab;
        [SerializeField] private GameObject _batchButtonPrefab;
        [SerializeField] private GameObject _batchTriggerButtonPrefab;
        [SerializeField] private GameObject _spawnEventsParent;
        [SerializeField] private GameObject _spawnEventsDirectParent;
        [SerializeField] private Transform _prefabInstanceParentTran;
        [SerializeField] private Transform _maskAvoiderTargetParent;
        [SerializeField] private SaveBatchButton _saveBatchButton;
        [SerializeField] private SpawnEventButtonFactory _spawnEventButtonFactory;
        [SerializeField] private TMP_InputField _batchNameInput;

        private int _currentId = 0;

        protected override void OnClick()
        {
            base.OnClick();
            InitializeBatchButton();
            InitializeBatchTrigger();
            Reposition();
        }

        private void InitializeBatchButton()
        {
            var batchButton = Instantiate(_batchButtonPrefab, _prefabInstanceParentTran);
            batchButton.GetComponentsInChildren<UiMaskAvoider>(true).ToList().ForEach(item => item.Initialize(_maskAvoiderTargetParent));
            
            var viewController = batchButton.GetComponent<BatchViewController>();
            viewController.Initialize(_spawnEventsParent, _spawnEventsDirectParent, _spawnEventButtonFactory, _batchNameInput, _currentId++);
            viewController.OnButtonClicked += DeselectOthersAndSetMarshal;
        }

        private void InitializeBatchTrigger()
        {
            var batchTrigger = Instantiate(_batchTriggerButtonPrefab, _prefabInstanceParentTran);
            batchTrigger.GetComponentsInChildren<UiMaskAvoider>(true).ToList().ForEach(item => item.Initialize(_maskAvoiderTargetParent));
        }
        
        private void Reposition()
        {
            Instantiate(_spacerPrefab, _prefabInstanceParentTran);
            transform.SetAsLastSibling();
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