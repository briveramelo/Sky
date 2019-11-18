using System;
using UnityEngine;

namespace BRM.Sky.WaveEditor
{
    public class BatchViewController : Selector, ISelectable
    {
        public event Action OnButtonClicked;
        
        [SerializeField] private BatchDataMarshal _dataMarshal;
        [SerializeField] private BatchUi _batchUi;
        
        private GameObject _spawnEventsParent;

        public void SetSpawnEventsParent(GameObject parent)
        {
            _spawnEventsParent = parent;
        }

        protected override void OnClick()
        {
            base.OnClick();
            OnButtonClicked?.Invoke();

            Select(!IsSelected);
            PopulateSpawnEventDataUi();
        }

        public void Select(bool isSelected)
        {
            _batchUi.Select(isSelected);
            
            if (isSelected)
            {
                _spawnEventsParent.SetActive(true);
                PopulateSpawnEventDataUi();
            }
        }
        
        private void PopulateSpawnEventDataUi()
        {
            
        }


        public bool IsSelected => _batchUi.IsSelected;
    }
}