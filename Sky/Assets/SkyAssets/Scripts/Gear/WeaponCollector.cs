using UnityEngine;

public class WeaponCollector : MonoBehaviour
{
    [SerializeField] private Jai _myJai;

    private void OnTriggerEnter2D(Collider2D col)
    {
        StartCoroutine(_myJai.CollectNewWeapon(col.GetComponent<ICollectable>()));
        FindObjectOfType<WaveUi>().GetComponent<IWaveUi>().GrabbedWeapon();
    }
}