using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInputScript : MonoBehaviour
{
    public Transform target;

    public float lerpModifier;
    public float maxInertia;
    public float camPositiveXClampVal;
    public float camNegativeXClampVal;

    float xVel;
    float yVel;

    Vector3 dragOrigin;
  
    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        PanCamera();
    }

    void PanCamera()
    {
        if (Input.GetMouseButtonDown(0))
            dragOrigin = target.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButton(0))
        {
            Vector3 diff = dragOrigin - target.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);

            target.position += new Vector3(diff.x, 0, 0);

            //Clamp to screen
            float a = Mathf.Clamp(target.position.x, camNegativeXClampVal, camPositiveXClampVal);
            target.position = new Vector3(a, target.position.y, target.position.z);

        }
    
    }
}
