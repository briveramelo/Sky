using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public abstract class Selector : MonoBehaviour
{
    [SerializeField] protected Button _button;
    [SerializeField] protected AudioClipType _audioType;

    protected virtual void Awake()
    {
        _button.onClick.AddListener(OnClick);
    }

    protected virtual void OnClick()
    {
        AudioManager.PlayAudio(_audioType);
        StartCoroutine(OnClickRoutine());
    }

    protected virtual IEnumerator OnClickRoutine()
    {
        yield return null;
    }
}