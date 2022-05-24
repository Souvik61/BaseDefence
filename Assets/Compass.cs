using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Compass : MonoBehaviour
{
    public Transform target;
    public Vector2 startPoint;
    public float scale;
    // Update is called once per frame
    void Update()
    {
        startPoint = (target.position - transform.position).normalized;
        startPoint *= 1.1f;
        HelperScript.DrawArrowDebug(transform.position, (Vector2)transform.position + startPoint, Color.cyan);
    }
}
