using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BRM.Sky.WaveEditor
{
    public class WaveTimelineView : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private GameObject _batchPrefab;
        [SerializeField] private Transform _batchParent;
        [SerializeField] private Transform _sliderHandle;
        
        public event Action<int> OnWaveSelected;

        public void SetLabels(List<string> labels)
        {
            _existingLabels.ForEach(label => Destroy(label.gameObject));
            _existingLabels.Clear();
            _slider.maxValue = labels.Count - 1;
            for (var i=0; i<labels.Count; i++)
            {
                var label = labels[i];
                var batchInstance = Instantiate(_batchPrefab, _batchParent);
                var textLabel = batchInstance.GetComponentInChildren<TextMeshProUGUI>();
                textLabel.text = label;
                _existingLabels.Add(batchInstance);

                SetCurrentBatch(i);
                batchInstance.transform.position = _sliderHandle.position;
            }
            SetCurrentBatch(0);
        }

        public void SetCurrentBatch(int wave)
        {
            _slider.onValueChanged.RemoveListener(OnValueChanged);
            _slider.value = wave;
            _slider.onValueChanged.AddListener(OnValueChanged);
        }

        private List<GameObject> _existingLabels=new List<GameObject>();
        private void Awake()
        {
            _slider.onValueChanged.AddListener(OnValueChanged);
        }

        private void OnValueChanged(float newValue)
        {
            OnWaveSelected?.Invoke((int) newValue);
        }
    }
}