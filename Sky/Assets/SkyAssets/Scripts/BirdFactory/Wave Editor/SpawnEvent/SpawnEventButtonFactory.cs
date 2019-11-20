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

        public void RecreateButtons(int numButtons)
        {
            _viewControllers.Clear();
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
            button.GetComponentsInChildren<UiMaskAvoider>(true).ToList().ForEach(item => item.Initialize(_maskAvoiderTargetParent));

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