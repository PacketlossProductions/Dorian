using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class DoorEditor : MonoBehaviour
{
    DoorController dc;

    private void OnDrawGizmos()
    {
        if(dc == null)
        {
            dc = GetComponent<DoorController>();
        }
        Vector2 source = transform.position;
        Gizmos.color = Color.cyan;
        foreach(GameObject gameObject in dc.switches)
        {
            Gizmos.DrawLine(source, gameObject.transform.position);
        }
    }
}
