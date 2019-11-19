using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GenericFunctions;
using UnityEngine;

namespace BRM.Sky.WaveEditor
{
    public class AddSpawnEventButton : Selector
    {
        [SerializeField] private GameObject _spawnButtonPrefab;
        [SerializeField] private Transform _prefabInstanceParentTran;
        [SerializeField] private Transform _maskAvoiderTargetParent;

        private IHoldData _lastDataHolder;
        private int _currentId = 0;

        public void CreateButtons(int numButtons, Action onComplete)
        {
            StartCoroutine(CreateButtonsRoutine(numButtons, onComplete));
        }

        private IEnumerator CreateButtonsRoutine(int numButtons, Action onComplete)
        {
            var viewControllers = new List<SpawnEventViewController>();
            for (int i = 0; i < numButtons; i++)
            {
                viewControllers.Add(CreateSpawnEventViewController());
            }
            yield return null;
            viewControllers.ForEach(vc => vc.Select(true));
            yield return null;
            DeselectOthers(-1);
            onComplete?.Invoke();
        }

        protected override IEnumerator OnClickRoutine()
        {
            DeselectOthers(-1);

            var viewController = CreateSpawnEventViewController();
            yield return null;

            viewController.Select(true);
        }

        private SpawnEventViewController CreateSpawnEventViewController()
        {
            var button = Instantiate(_spawnButtonPrefab, _prefabInstanceParentTran);
            transform.SetAsLastSibling();

            button.GetComponentsInChildren<UiMaskAvoider>(true).ToList().ForEach(item => item.SetTargetParent(_maskAvoiderTargetParent));

            _lastDataHolder = button.GetComponent<IHoldData>();

            var viewController = button.GetComponent<SpawnEventViewController>();
            viewController.Id = _currentId++;
            viewController.OnButtonClicked += DeselectOthers;
            return viewController;
        }

        private void DeselectOthers(int selectedId)
        {
            _prefabInstanceParentTran.GetComponentsRecursively<SpawnEventViewController>(true).ToList().ForEach(selector =>
            {
                if (selector.Id != selectedId)
                {
                    selector.Select(false);
                }
            });
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