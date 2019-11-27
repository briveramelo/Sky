using System;
using System.Collections.Generic;
using System.Linq;
using BRM.EventBrokers;
using BRM.EventBrokers.Interfaces;
using BRM.Sky.CustomWaveData;
using TMPro;
using UnityEngine;

namespace BRM.Sky.WaveEditor.Ui
{
    //factory for making spawn event buttons for the wave editor ui
    public class SpawnEventButtonFactory : Selector
    {
        [SerializeField] private TMP_InputField _batchNameInput;
        [SerializeField] private GameObject _spawnButtonPrefab;
        [SerializeField] private Transform _prefabInstanceParentTran;
        [SerializeField] private Transform _maskAvoiderTargetParent;

        private int _currentId = 0;
        private List<SpawnEventViewController> _viewControllers = new List<SpawnEventViewController>();
        private List<BatchData> _customBatchData = new List<BatchData>();
        private IBrokerEvents _eventBroker = new StaticEventBroker();

        private void Start()
        {
            RestCustomBatchData();
            _eventBroker.Subscribe<BatchSavedData>(OnBatchSaved);
        }

        private void OnDestroy()
        {
            _eventBroker.Unsubscribe<BatchSavedData>(OnBatchSaved);
        }

        private void OnBatchSaved(BatchSavedData data)
        {
            RestCustomBatchData();
        }

        private void RestCustomBatchData()
        {
            _customBatchData = CustomBatchDataLoader.GetCustomBatchData();
            _viewControllers.ForEach(vc => vc.ResetCustomBatchData(_customBatchData));
        }

        public void DestroyButtons()
        {
            _viewControllers.RemoveAll(vc => vc == null);
            _viewControllers.ForEach(vc => Destroy(vc.gameObject));
            _viewControllers.Clear();
        }

        public void CreateButtons(int numButtons)
        {
            for (int i = 0; i < numButtons; i++)
            {
                _viewControllers.Add(CreateAndInitializeButton());
            }

            _viewControllers.ForEach(vc => vc.Select(false));
            Reposition();
        }

        protected override void OnClick()
        {
            base.OnClick();
            _viewControllers.ForEach(selector => selector.Select(false));
            _viewControllers.Add(CreateAndInitializeButton());
            Reposition();
        }

        private SpawnEventViewController CreateAndInitializeButton()
        {
            var button = Instantiate(_spawnButtonPrefab, _prefabInstanceParentTran);
            var deleteButton = button.GetComponentInChildren<DeleteButton>();
            var toDelete = new List<GameObject>();
            button.GetComponentsInChildren<UiMaskAvoider>(true).ToList().ForEach(item =>
            {
                item.Initialize(_maskAvoiderTargetParent);
            });
            toDelete.Add(button);
            deleteButton.SetGameObjectsToDelete(toDelete);
            var viewController = button.GetComponent<SpawnEventViewController>();
            viewController.Initialize(_currentId++, DeselectOthers, _customBatchData);
            
            var dataMarshal = button.GetComponent<SpawnEventDataMarshal>();
            dataMarshal.Initialize(_batchNameInput);
            
            return viewController;
        }

        private void Reposition()
        {
            transform.SetAsLastSibling();
        }

        private void DeselectOthers(int selectedId)
        {
            _viewControllers.ForEach(selector =>
            {
                if (selector.Id != selectedId)
                {
                    selector.Select(false);
                }
            });
        }
    }
}