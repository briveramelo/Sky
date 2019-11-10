using BRM.EventBrokers;
using BRM.EventBrokers.Interfaces;
using UnityEngine;

public class WeaponCollector : MonoBehaviour
{
    private IPublishEvents _eventPublisher = new StaticEventBroker();

    private void OnTriggerEnter2D(Collider2D col)
    {
        var collectable = col.GetComponent<ICollectable>();
        _eventPublisher.Publish(new WeaponGrabbedData{Collectable = collectable});
    }
}