using System;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace BRM.Sky.WaveEditor
{
    public class SpawnTypeDropdown : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown _dropdown;

        private void Awake()
        {
            _dropdown.options = Enum.GetValues(typeof(SpawnPrefab)).Cast<SpawnPrefab>().Select(item => new TMP_Dropdown.OptionData(item.ToString())).ToList();
        }

        public SpawnPrefab SpawnPrefab => (SpawnPrefab)_dropdown.value;
    }

    public static class SpawnPrefabFactory
    {

        private class SpawnPrefabData
        {
            /*todo: add data
             prefabType,
             prefab,
             file path,
            */
        }

        public static GameObject GetPrefab(SpawnPrefab prefabType)
        {
            return null;
        }
        public static Sprite GetSprite(SpawnPrefab prefabType)
        {
            //AssetDatabase.GetCachedIcon()
            //Sprite.Create(new Texture2D(0,0), new Rect)
            return null;
        }
    }
}