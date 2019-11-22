using UnityEngine;
using System.Collections;

public class TeleportSideToSide : MonoBehaviour
{
    [SerializeField] private Collider2D _buddyCollider;
    [SerializeField] private Teleporter _teleporterType;
    [SerializeField, Range(-5f, 5f)] private float _distanceFromWorldEdge = 0.5f;
    
    private Vector2 _destination;

    private enum Teleporter
    {
        Pigeon,
        DuckLeader
    }

    private IEnumerator Start()
    {
        var xMult = transform.parent.GetChild(0) == transform ? 1f : -1f;
        transform.localPosition = new Vector2(xMult * (ScreenSpace.WorldEdge.x + _distanceFromWorldEdge), 0f);
        yield return null;
        _destination = new Vector2(_buddyCollider.transform.position.x, 0f);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (_teleporterType == Teleporter.Pigeon)
        {
            var pigeon = col.gameObject.GetComponent<Pigeon>();
            if (pigeon)
            {
                var isCorrectTeleporter = (int) Mathf.Sign(pigeon.MoveDirection.x) == (int) Mathf.Sign(transform.position.x);
                if (isCorrectTeleporter)
                {
                    //teleport pigeons across sides
                    StartCoroutine(TemporaryTeleport(col));
                }
            }
        }

        if (_teleporterType == Teleporter.DuckLeader)
        {
            var leadDuck = col.gameObject.GetComponent<LeadDuck>();
            if (leadDuck)
            {
                var isCorrectTeleporter = (int) Mathf.Sign(leadDuck.MoveDirection.x) == (int) Mathf.Sign(transform.position.x);
                if (isCorrectTeleporter)
                {
                    //teleport duck squads across sides
                    StartCoroutine(TemporaryTeleport(col));
                }
            }
        }
    }


    private IEnumerator TemporaryTeleport(Collider2D col)
    {
        var colTran = col.transform;
        colTran.position = _destination + Vector2.up * colTran.position.y;
        Physics2D.IgnoreCollision(col, _buddyCollider, true);
        yield return new WaitForSeconds(3f);
        if (col != null)
        {
            Physics2D.IgnoreCollision(col, _buddyCollider, false);
        }
    }
}