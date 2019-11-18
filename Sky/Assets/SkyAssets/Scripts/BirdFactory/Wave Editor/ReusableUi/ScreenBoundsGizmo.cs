using UnityEngine;

public class ScreenBoundsGizmo : MonoBehaviour
{
    [SerializeField] private bool _showGizmos = true;
    private void OnDrawGizmos()
    {
        if (!_showGizmos)
        {
            return;
        }

        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(Vector2.zero, ScreenSpace.ScreenSizeWorldUnits);
    }
}
