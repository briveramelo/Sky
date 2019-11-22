using System.Collections.Generic;
using System.Linq;
using BRM.Sky.CustomWaveData;
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
        [SerializeField] private LoadBatchButton _loadBatchButton;
        [SerializeField] private SpawnEventButtonFactory _spawnEventButtonFactory;
        [SerializeField] private TMP_InputField _batchNameInput;

        private int _currentId = 0;
        private List<GameObject> _buttonGameObjects = new List<GameObject>();
        private List<TriggerDataMarshal> _triggerDataMarshals = new List<TriggerDataMarshal>();

        public void DestroyButtons()
        {
            _buttonGameObjects.ForEach(Destroy);
            _buttonGameObjects.Clear();
            _triggerDataMarshals.Clear();
        }

        public void CreateButtons(int numButtons)
        {
            for (int i = 0; i < numButtons; i++)
            {
                var batch = InitializeBatchButton();
                var trigger = InitializeBatchTrigger();
                _buttonGameObjects.Add(batch);
                _buttonGameObjects.Add(trigger);
                var toDelete = new List<GameObject> {batch, trigger};
                InitializeDeleteButton(batch, toDelete);
            }
            Reposition();
        }

        protected override void OnClick()
        {
            base.OnClick();
            CreateButtons(1);
            _triggerDataMarshals[_triggerDataMarshals.Count - 1].Data = new BatchTriggerData {Amount = 1, TriggerType = default};
        }

        private GameObject InitializeBatchButton()
        {
            var batchButton = Instantiate(_batchButtonPrefab, _prefabInstanceParentTran);
            batchButton.GetComponentsInChildren<UiMaskAvoider>(true).ToList().ForEach(item => item.Initialize(_maskAvoiderTargetParent));

            var viewController = batchButton.GetComponent<BatchViewController>();
            viewController.Initialize(_spawnEventsParent, _spawnEventsDirectParent, _spawnEventButtonFactory, _batchNameInput, _currentId++);
            viewController.OnButtonClicked += DeselectOthersAndSetMarshal;
            
            return batchButton;
        }

        private GameObject InitializeBatchTrigger()
        {
            var trigger = Instantiate(_batchTriggerButtonPrefab, _prefabInstanceParentTran);
            trigger.GetComponentsInChildren<UiMaskAvoider>(true).ToList().ForEach(item => item.Initialize(_maskAvoiderTargetParent));

            var viewController = trigger.GetComponent<TriggerViewController>();
            viewController.Initialize();
            var triggerDataMarshal = trigger.GetComponent<TriggerDataMarshal>();
            _triggerDataMarshals.Add(triggerDataMarshal);
            return trigger;
        }

        private void InitializeDeleteButton(GameObject deleteButton, List<GameObject> gos)
        {
            var button = deleteButton.GetComponentInChildren<DeleteButton>(true);
            button.SetGameObjectsToDelete(gos); 
        }

        private void Reposition()
        {
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
                    var marshal = item.GetComponent<BatchDataMarshal>();
                    _saveBatchButton.SetDataMarshal(marshal);
                    _loadBatchButton.SetDataMarshal(marshal);
                }
            });
        }
    }
}