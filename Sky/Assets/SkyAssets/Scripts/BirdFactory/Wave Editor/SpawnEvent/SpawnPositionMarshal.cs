using UnityEngine;

namespace BRM.Sky.WaveEditor
{
    public class SpawnPositionMarshal : MonoBehaviour
    {
        [SerializeField] private Transform _prefab;

        public Vector2 NormalizedPosition
        {
            get => _prefab.position.WorldToViewportPosition();
            set => _prefab.position = value.ViewportToWorldPosition();
        }

        public string Text => NormalizedPosition.ToString("0.00");
    }
}