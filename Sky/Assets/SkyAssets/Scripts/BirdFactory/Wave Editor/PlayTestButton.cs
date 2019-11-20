using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BRM.Sky.WaveEditor
{
    public class PlayTestButton : Selector
    {
        [SerializeField] private List<GameObject> _requiredPrefabs;
        [SerializeField] private string _textDuringTest, _textDuringEditor;
        [SerializeField] private TextMeshProUGUI _buttonText;

        private bool _isEditing;
        private List<GameObject> _requiredInstances = new List<GameObject>();

        protected override void OnClick()
        {
            base.OnClick();
            _isEditing = !_isEditing;
            _buttonText.text = _isEditing ? _textDuringEditor : _textDuringTest;
            
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
            //todo: trigger wavemanager to spawn this wave
        }

        private void DestroyTest()
        {
            _requiredInstances.ForEach(Destroy);
            _requiredInstances.Clear();
        }
    }
}
