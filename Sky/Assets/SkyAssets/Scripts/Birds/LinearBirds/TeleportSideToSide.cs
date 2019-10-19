using UnityEngine;
using System.Collections;

public class TeleportSideToSide : MonoBehaviour
{
    [SerializeField] private Collider2D _buddyCollider;
    [SerializeField] private Teleporter _teleporterType;
    private Vector2 _destination;

    private enum Teleporter
    {
        Pigeon,
        DuckLeader
    }

    private void Awake()
    {
        _destination = new Vector2(_buddyCollider.transform.position.x, 0f);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (_teleporterType == Teleporter.Pigeon)
        {
            if (col.gameObject.GetComponent<Pigeon>())
            {
                //teleport pigeons across sides
                StartCoroutine(TemporaryTeleport(col));
            }
        }

        if (_teleporterType == Teleporter.DuckLeader)
        {
            if (col.gameObject.GetComponent<LeadDuck>())
            {
                //teleport duck squads across sides
                StartCoroutine(TemporaryTeleport(col));
            }
        }
    }

    private IEnumerator TemporaryTeleport(Collider2D col)
    {
        col.gameObject.transform.position = _destination + Vector2.up * col.gameObject.transform.position.y;
        Physics2D.IgnoreCollision(col, _buddyCollider, true);
        yield return new WaitForSeconds(3f);
        Physics2D.IgnoreCollision(col, _buddyCollider, false);
    }
}