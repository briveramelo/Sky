using UnityEngine;

namespace BRM.Sky.WaveEditor
{
    public class UiMaskAvoider : MonoBehaviour
    {
        [SerializeField] private Transform _intendedParent;
        [SerializeField] private Vector2 _offset = new Vector2(60, 0);
        
        private RectTransform _rectTran;

        private void Awake()
        {
            _rectTran = transform as RectTransform;
        }

        private void Update()
        {
            _rectTran.position = _intendedParent.TransformPoint(_offset);
        }

    }
}