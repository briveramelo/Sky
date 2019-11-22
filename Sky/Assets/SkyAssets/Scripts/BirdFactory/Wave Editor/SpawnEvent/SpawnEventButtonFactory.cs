using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GenericFunctions;
using UnityEngine;

namespace BRM.Sky.WaveEditor.Ui
{
    //factory for making spawn event buttons for the wave editor ui
    public class SpawnEventButtonFactory : Selector
    {
        [SerializeField] private GameObject _spawnButtonPrefab;
        [SerializeField] private Transform _prefabInstanceParentTran;
        [SerializeField] private Transform _maskAvoiderTargetParent;

        private int _currentId = 0;
        private List<SpawnEventViewController> _viewControllers = new List<SpawnEventViewController>();

        public void DestroyButtons()
        {
            _viewControllers.RemoveAll(vc => !vc);//remove all nulls
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
            viewController.Id = _currentId++;
            viewController.OnButtonClicked += DeselectOthers;
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