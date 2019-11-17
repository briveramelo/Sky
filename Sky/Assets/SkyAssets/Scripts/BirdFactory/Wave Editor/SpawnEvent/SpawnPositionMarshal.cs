using UnityEngine;

namespace BRM.Sky.WaveEditor
{
    public class SpawnPositionMarshal : MonoBehaviour
    {
        [SerializeField] private Transform _prefab;

        public Vector2 NormalizedPosition => _prefab.position.WorldToViewportPosition();
    }
}