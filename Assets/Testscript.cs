using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testscript : MonoBehaviour
{
    public float arrowHeadLen;
    public float arrowHeadAngle;
    //public Vector2 startPoint;
    //public Vector2 direction;

    public Transform endPoint;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HelperScript.DrawArrowDebug(transform.position, endPoint.position, Color.green, arrowHeadLen, arrowHeadAngle);
        //HelperScript.DrawArrow(transform.position, endPoint.position, Color.green);
    }
}
