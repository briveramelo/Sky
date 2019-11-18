using System.Collections;
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

        protected override IEnumerator OnClickRoutine()
        {
            DeselectAll();

            var button = Instantiate(_spawnButtonPrefab, _prefabInstanceParentTran);
            transform.SetAsLastSibling();

            button.GetComponentsInChildren<UiMaskAvoider>(true).ToList().ForEach(item => item.SetTargetParent(_maskAvoiderTargetParent));

            _lastDataHolder = button.GetComponent<IHoldData>();
            
            var viewController = button.GetComponent<SpawnEventViewController>();
            viewController.OnButtonClicked += DeselectAll;
            yield return null;
            
            viewController.Select(true);
        }

        private void DeselectAll()
        {
            _prefabInstanceParentTran.GetComponentsRecursively<SpawnEventViewController>().ToList().ForEach(selector => selector.Select(false));
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