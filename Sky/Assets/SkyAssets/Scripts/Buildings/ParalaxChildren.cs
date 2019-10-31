using UnityEngine;
using System.Collections.Generic;

public class ParalaxChildren : MonoBehaviour
{
    [SerializeField] [Range(0.1f, .25f)] private float _moveSpeed;
    [SerializeField] private bool _toRight;
    [SerializeField] private float _teleportXSpot;
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector2(_teleportXSpot, -10f), new Vector2(_teleportXSpot, 10f));
    }

    private List<Transform> _children = new List<Transform>();
    private int _childCount;
    private float _moveSign;
    private void Awake()
    {
        for (var i = 0; i < transform.childCount; i++)
        {
            _children.Add(transform.GetChild(i));
        }

        _childCount = _children.Count;
        _moveSign = _toRight ? 1 : -1;
    }

    private void Update()
    {
        for (int i = 0; i < _childCount; i++)
        {
            var child = _children[i];
            var tempPosition = child.position;
            tempPosition += _moveSign * _moveSpeed * Time.deltaTime * Vector3.right;
            if (Mathf.Abs(tempPosition.x) > Mathf.Abs(_teleportXSpot))
            {
                tempPosition = new Vector3((_toRight ? 1 : -1) * (_teleportXSpot + 0.01f), tempPosition.y, 0f);
            }

            child.position = tempPosition;
        }
    }
}