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

    public class BatchTriggerData
    {
        public BatchTriggerData()
        {
        }

        public BatchTriggerData(BatchTriggerData other)
        {
            TriggerType = other.TriggerType;
            Amount = other.Amount;
        }

        public BatchTriggerType TriggerType;
        public float Amount;
    }

    public enum BatchTriggerType
    {
        AllDead=0,
        NumDead=1,
        NumSpears=2,
        Time=3
    }
}

