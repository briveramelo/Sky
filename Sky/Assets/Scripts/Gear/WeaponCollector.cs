using UnityEngine;
using System.Collections;
using GenericFunctions;

public class WeaponCollector : MonoBehaviour {

    [SerializeField] Jai MyJai;

	void OnTriggerEnter2D(Collider2D col) {
        StartCoroutine(MyJai.CollectNewWeapon(col.GetComponent<ICollectable>()));
        FindObjectOfType<WaveUI>().GetComponent<IWaveUI>().GrabbedWeapon();
    }
}
