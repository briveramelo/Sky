using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BRM.Sky.WaveEditor
{
    public class SpawnEventUi : MonoBehaviour, IUpdateUi
    {
        [SerializeField] private Image _iconPreview;
        [SerializeField] private TextMeshProUGUI _spawnTypeText;
        [SerializeField] private TextMeshProUGUI _positionText;
        [SerializeField] private TextMeshProUGUI _timeText;

        [SerializeField] private SpawnTypeDropdown _spawnTypeDropdown;
        [SerializeField] private SpawnPositionMarshal _positionMarshal;
        [SerializeField] private SpawnTimeInput _spawnTime;

        private void Awake()
        {
            UpdateUi();
        }

        public void UpdateUi()
        {
            _iconPreview.sprite = SpawnPrefabFactory.Instance.GetSprite(_spawnTypeDropdown.SpawnPrefab);
            _spawnTypeText.text = _spawnTypeDropdown.Text;
            _positionText.text = _positionMarshal == null ? "(?, ?)" : _positionMarshal.Text;
            _timeText.text = _spawnTime.Text;
        }
    }
}