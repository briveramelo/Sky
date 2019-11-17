using TMPro;
using UnityEngine;

namespace BRM.Sky.WaveEditor
{
    public class BatchTriggerDropdown : MonoBehaviour, IDisplayable
    {
        [SerializeField] private TMP_Dropdown _dropdown;

        public BatchTriggerType TriggerType => (BatchTriggerType) _dropdown.value;

        public void ToggleDisplay(bool show)
        {
            gameObject.SetActive(show);
        }
    }
}

