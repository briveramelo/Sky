using UnityEngine;

namespace BRM.Sky.WaveEditor
{
    public class UiMaskAvoider : MonoBehaviour
    {
        [SerializeField] private Transform _intendedParent;
        [SerializeField] private Vector2 _offset = new Vector2(60, 0);

        private Transform _targetParent;

        public void Initialize(Transform parent)
        {
            _targetParent = parent;
            transform.SetParent(_targetParent);
        }

        private void Update()
        {
            transform.position = _intendedParent.TransformPoint(_offset);
        }
    }
}