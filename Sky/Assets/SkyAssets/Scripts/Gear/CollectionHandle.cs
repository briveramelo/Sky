using UnityEngine;
using System.Collections;

public interface ICollectable {
    WeaponType GetCollected();
}

public class CollectionHandle : MonoBehaviour, ICollectable {

    [SerializeField] WeaponType MyWeaponType;

	WeaponType ICollectable.GetCollected() {
        StartCoroutine(DestroySelf());
        return MyWeaponType;
    }

    IEnumerator DestroySelf() {
        yield return null;
        Destroy(transform.parent.gameObject);
    }
}
