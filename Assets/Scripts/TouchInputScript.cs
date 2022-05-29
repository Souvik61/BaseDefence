using UnityEngine;

public class TouchInputScript : MonoBehaviour
{
    public Transform target;

    public float camPositiveXClampVal;
    public float camNegativeXClampVal;

    Vector3 dragOrigin;

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
