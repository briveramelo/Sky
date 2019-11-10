using System.Collections.Generic;
using UnityEngine;

public class ParallaxParent : MonoBehaviour
{
    [SerializeField] private Range _moveSpeedRange;
    [SerializeField] private List<ParallaxChildren> _children;
    public List<ParallaxChildren> Children => _children;
    
    private void Start()
    {
        AssignMoveSpeeds();
    }

    public void AssignMoveSpeeds()
    {
        for (int i = 0; i < _children.Count; i++)
        {
            float percentage = (_children.Count - 1 - i) / (_children.Count - 1f);
            _children[i].MoveSpeed = GetMoveSpeed(percentage);
        }
    }

    private float GetMoveSpeed(float percentage)
    {
        percentage = Mathf.Clamp01(percentage);
        return _moveSpeedRange.Min + percentage * _moveSpeedRange.Diff;
    }
}