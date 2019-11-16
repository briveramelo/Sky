using TMPro;
using UnityEngine;

namespace BRM.Sky.WaveEditor
{
    public class BatchTriggerInputField : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _inputField;
        
        public float? Amount
        {
            get
            {
                if (float.TryParse(_inputField.text, out var amount))
                {
                    return amount;
                }
                return null;
            }
        }
    }
}