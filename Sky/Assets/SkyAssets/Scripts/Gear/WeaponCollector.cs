using UnityEngine;

public class WeaponCollector : MonoBehaviour {

    [SerializeField] private Jai MyJai;

    private void OnTriggerEnter2D(Collider2D col) {
        StartCoroutine(MyJai.CollectNewWeapon(col.GetComponent<ICollectable>()));
        FindObjectOfType<WaveUI>().GetComponent<IWaveUI>().GrabbedWeapon();
    }
}
