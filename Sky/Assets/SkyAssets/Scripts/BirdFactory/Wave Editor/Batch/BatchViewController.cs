using UnityEngine;

namespace BRM.Sky.WaveEditor
{
    public class BatchViewController : Selector
    {
        [SerializeField] private BatchDataMarshal _dataMarshal;
        
        private GameObject _spawnEventsParent;

        public void SetSpawnEventsParent(GameObject parent)
        {
            _spawnEventsParent = parent;
        }

        protected override void OnClick()
        {
            base.OnClick();
            _spawnEventsParent.SetActive(!_spawnEventsParent.activeInHierarchy);
            PopulateSpawnEventDataUi();
        }

        private void PopulateSpawnEventDataUi()
        {
            
        }
    }
}