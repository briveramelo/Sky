using UnityEngine;
using System.Collections;
using GenericFunctions;

//todo: measure time to cross across resolutions
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
        transform.localPosition = new Vector2(xMult * (Constants.ScreenSizeWorldUnits.x + _distanceFromWorldEdge), 0f);
        yield return null;
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

    private float t1;

    private IEnumerator TemporaryTeleport(Collider2D col)
    {
        Debug.Log(Time.time - t1);
        t1 = Time.time;
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