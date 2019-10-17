using UnityEngine;
using System.Collections;

public interface ICollectable {
    WeaponType GetCollected();
}

public class CollectionHandle : MonoBehaviour, ICollectable {

    [SerializeField] private WeaponType MyWeaponType;

	WeaponType ICollectable.GetCollected() {
        StartCoroutine(DestroySelf());
        return MyWeaponType;
    }

    private IEnumerator DestroySelf() {
        yield return null;
        Destroy(transform.parent.gameObject);
    }
}
