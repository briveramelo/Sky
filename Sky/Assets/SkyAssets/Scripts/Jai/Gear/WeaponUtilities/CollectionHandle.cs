using UnityEngine;
using System.Collections;

public interface ICollectable
{
    WeaponType Collect();
}

public class CollectionHandle : MonoBehaviour, ICollectable
{
    [SerializeField] private WeaponType _myWeaponType;

    WeaponType ICollectable.Collect()
    {
        StartCoroutine(DestroySelf());
        return _myWeaponType;
    }

    private IEnumerator DestroySelf()
    {
        yield return null;
        Destroy(transform.parent.gameObject);
    }
}