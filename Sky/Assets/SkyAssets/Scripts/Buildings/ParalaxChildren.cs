using UnityEngine;
using System.Collections.Generic;

public class ParalaxChildren : MonoBehaviour
{
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector2(_teleportXSpot, -10f), new Vector2(_teleportXSpot, 10f));
    }

    private List<Transform> children = new List<Transform>();

    [SerializeField] [Range(0.1f, .25f)] private float _moveSpeed;
    [SerializeField] private bool _toRight;
    [SerializeField] private float _teleportXSpot;

    private void Awake()
    {
        for (var i = 0; i < transform.childCount; i++)
        {
            children.Add(transform.GetChild(i));
        }
    }

    private void Update()
    {
        children.ForEach(child =>
        {
            child.position += (_toRight ? 1 : -1) * Vector3.right * Time.deltaTime * _moveSpeed;
            if (Mathf.Abs(child.position.x) > Mathf.Abs(_teleportXSpot))
            {
                child.position = new Vector3((_toRight ? 1 : -1) * (_teleportXSpot + 0.01f), child.position.y, 0f);
            }
        });
    }
}