using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GenericFunctions;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace BRM.Sky.WaveEditor
{
    public class SpawnTypeDropdown : MonoBehaviour
    {
        public SpawnPrefab SpawnPrefab
        {
            get => (SpawnPrefab) _dropdown.value;
            set => _dropdown.value = (int) value;
        }

        public string Text => SpawnPrefab.ToString();

        [SerializeField] private TMP_Dropdown _dropdown;

        private GameObject _prefabInstance;

        private void Start()
        {
            _dropdown.options = Enum.GetValues(typeof(SpawnPrefab)).Cast<SpawnPrefab>()
                .Select(prefabType => new TMP_Dropdown.OptionData(prefabType.ToString(), SpawnPrefabFactory.Instance.GetSprite(prefabType))).ToList();
            _dropdown.onValueChanged.AddListener(OnDropdownSelected);
        }

        private void OnDropdownSelected(int value)
        {
            if (_prefabInstance != null)
            {
                Destroy(_prefabInstance);
            }

            _prefabInstance = SpawnPrefabFactory.Instance.CreateInstance((SpawnPrefab) value);
            _prefabInstance.transform.position = new Vector2(-ScreenSpace.WorldEdge.x, 0);
            
            _prefabInstance.GetComponentsRecursively<Behaviour>().ForEach(b => b.enabled = false);
            _prefabInstance.GetComponentsRecursively<Rigidbody2D>().ForEach(Destroy);
            _prefabInstance.GetComponentsRecursively<SpriteRenderer>().ForEach(sr => sr.enabled = true);
        }
    }
}