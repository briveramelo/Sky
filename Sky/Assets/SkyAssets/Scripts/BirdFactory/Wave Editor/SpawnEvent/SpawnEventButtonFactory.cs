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

        public void CreateButtons(int numButtons, Action onComplete)
        {
            StartCoroutine(CreateButtonsRoutine(numButtons, onComplete));
        }

        private IEnumerator CreateButtonsRoutine(int numButtons, Action onComplete)
        {
            var viewControllers = new List<SpawnEventViewController>();
            for (int i = 0; i < numButtons; i++)
            {
                viewControllers.Add(CreateAndInitializeButton());
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

            var viewController = CreateAndInitializeButton();
            Reposition();
            yield return null;

            viewController.Select(true);
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
            _prefabInstanceParentTran.GetComponentsRecursively<SpawnEventViewController>(true).ToList().ForEach(selector =>
            {
                if (selector.Id != selectedId)
                {
                    selector.Select(false);
                }
            });
        }
    }
}