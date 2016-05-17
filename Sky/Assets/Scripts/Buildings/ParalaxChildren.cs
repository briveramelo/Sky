using UnityEngine;
using System.Collections.Generic;

public class ParalaxChildren : MonoBehaviour {

    
    void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector2(teleportXSpot, -10f), new Vector2(teleportXSpot, 10f));
    }

    List<Transform> children = new List<Transform>();

    [SerializeField, Range(0.1f,.25f)] float moveSpeed;
    [SerializeField] bool toRight;
    [SerializeField] float teleportXSpot;

    void Awake() {
        for (int i=0; i<transform.childCount; i++) {
            children.Add(transform.GetChild(i));
        }
    }
	
	void Update () {
        children.ForEach(child => { child.position += (toRight ? 1 : -1) * Vector3.right * Time.deltaTime * moveSpeed;
            if (Mathf.Abs(child.position.x)>Mathf.Abs(teleportXSpot)) {
                child.position = new Vector3( (toRight ? 1 :-1) * (teleportXSpot + 0.01f), child.position.y, 0f);
            }
        });
	}
}
