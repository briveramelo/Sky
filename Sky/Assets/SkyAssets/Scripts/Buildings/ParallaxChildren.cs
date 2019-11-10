using UnityEngine;
using System.Collections.Generic;

public class ParallaxChildren : MonoBehaviour
{
    public float MoveSpeed { get; set; }
    
    [SerializeField] private bool _toRight;
    [SerializeField] private float _teleportXSpot;
    
    private List<PixelTransform> _children = new List<PixelTransform>();
    private int _childCount;
    private float _moveSign;
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector2(_teleportXSpot, -10f), new Vector2(_teleportXSpot, 10f));
    }

    private void Awake()
    {
        for (var i = 0; i < transform.childCount; i++)
        {
            PixelTransform tran = new PixelTransform(transform.GetChild(i));
            _children.Add(tran);
        }

        _childCount = _children.Count;
        _moveSign = _toRight ? 1 : -1;
    }

    private void Update()
    {
        for (int i = 0; i < _childCount; i++)
        {
            var child = _children[i];
            child.TargetPosition += _moveSign * MoveSpeed * Time.deltaTime * Vector2.right;
            if (Mathf.Abs(child.TargetPosition.x) > Mathf.Abs(_teleportXSpot))
            {
                child.TargetPosition = new Vector2(_moveSign * (_teleportXSpot + 0.01f), child.TargetPosition.y);
            }

            child.TryUpdate();
        }
    }

    private void LateUpdate()
    {
        for (int i = 0; i < _childCount; i++)
        {
            var child = _children[i];
            child.LastPosition = child.Position;
        }
    }
}