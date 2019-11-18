using System.Collections.Generic;
using UnityEngine;

namespace BRM.Sky.WaveEditor
{
    public class PlayTestButton : Selector
    {
        [SerializeField] private List<GameObject> _requiredPrefabs;

        private bool _isEditing;
        private List<GameObject> _requiredInstances = new List<GameObject>();

        protected override void OnClick()
        {
            base.OnClick();
            _isEditing = !_isEditing;

            if (_isEditing)
            {
                DestroyTest();
            }
            else
            {
                CreateTest();
            }
        }

        private void CreateTest()
        {
            _requiredPrefabs.ForEach(item =>
            {
                var instance = Instantiate(item);
                _requiredInstances.Add(instance);
            });
        }

        private void DestroyTest()
        {
            _requiredInstances.ForEach(Destroy);
            _requiredInstances.Clear();
        }
    }
}
