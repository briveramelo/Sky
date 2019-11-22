using System.Collections.Generic;
using BRM.EventBrokers;
using BRM.EventBrokers.Interfaces;
using TMPro;
using UnityEngine;

namespace BRM.Sky.WaveEditor
{
    public class PlayTestButton : Selector
    {
        [SerializeField] private WaveDataMarshal _waveDataMarshal;
        [SerializeField] private WaveTimelineDataMarshal _waveTimelineMarshal;
        [SerializeField] private GameObject _designView;
        [SerializeField] private List<GameObject> _requiredPrefabs;
        [SerializeField] private string _textDuringTest, _textDuringEditor;
        [SerializeField] private TextMeshProUGUI _buttonText;

        private bool _isEditing;
        private List<GameObject> _requiredInstances = new List<GameObject>();
        private IPublishEvents _eventPublisher = new StaticEventBroker();

        private void Start()
        {
            _isEditing = true;
        }

        protected override void OnClick()
        {
            base.OnClick();
            _isEditing = !_isEditing;
            _buttonText.text = _isEditing ? _textDuringEditor : _textDuringTest;
            _designView.SetActive(_isEditing);
            
            var eventData = new WaveEditorTestData {State = _isEditing ? WaveEditorState.Editing : WaveEditorState.Testing};
            if (_isEditing)
            {
                DestroyTest();
            }
            else
            {
                CreateTest();
                var waveData = _waveDataMarshal.Data;
                eventData.WaveData = waveData;
            }

            _eventPublisher.Publish(eventData);
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