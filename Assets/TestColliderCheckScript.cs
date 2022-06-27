using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestColliderCheckScript : MonoBehaviour
{
    public LayerMask layerMask;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(ColliderCheck), 1, 1);
    }

    void ColliderCheck()
    {
        Collider2D[] coll = new Collider2D[10];
        Vector2 pt = transform.position;
        Vector2 size = new Vector2(2.5f, 2.5f);
        coll = Physics2D.OverlapBoxAll(pt, size, 0, layerMask);

        HelperScript.DrawBoundDebug(new Bounds(pt, size), Color.green);
        Debug.Log(coll.Length);

    
    }
}
