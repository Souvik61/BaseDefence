using UnityEngine;

public class SensorArrayScript : MonoBehaviour
{
    public float rayDistance;
    public float rayAngle;
    public LayerMask collLayers;
    public Transform sensorStartPos;
    public Transform rSensorStartPos;
    public Transform lSensorStartPos;

    public struct CollisionStatus 
    {
        public bool LARay, LRay, MRay, RRay, RARay;
        public bool hasCollision
        {
            get
            {
                return (LARay || LRay || MRay || RRay || RARay);
            }
        }
        public bool isFullyBlocked
        {
            get { return (LARay && LRay && MRay && RRay && RARay); }
        }
        public bool almostBlocked
        {
            get{
                return (LRay && RRay && MRay);
            }
        }
    }

    private void Update()
    {
        //DrawRays();
        //CheckCollisionArray();
    }

    private void DrawRays()
    {
        Vector2 origin = sensorStartPos.position;
        Vector2 A = transform.up * rayDistance;
        Vector2 RDir = Quaternion.AngleAxis(-rayAngle, Vector3.forward) * transform.up * rayDistance;
        Vector2 LDir = Quaternion.AngleAxis(rayAngle, Vector3.forward) * transform.up * rayDistance;
        
        HelperScript.DrawArrowDebug(sensorStartPos.position, (Vector2)sensorStartPos.position + A, Color.green);
        HelperScript.DrawArrowDebug(rSensorStartPos.position, (Vector2)rSensorStartPos.position + A, Color.green);
        HelperScript.DrawArrowDebug(lSensorStartPos.position, (Vector2)lSensorStartPos.position + A, Color.green);

        //Add angled rays
        HelperScript.DrawArrowDebug(rSensorStartPos.position, (Vector2)rSensorStartPos.position + RDir, Color.green);
        HelperScript.DrawArrowDebug(lSensorStartPos.position, (Vector2)lSensorStartPos.position + LDir, Color.green);

    }

    public CollisionStatus CheckCollisionArray()
    {
        CollisionStatus status = new CollisionStatus();
        Vector2 A = transform.up * rayDistance;
        Vector2 RDir = Quaternion.AngleAxis(-rayAngle, Vector3.forward) * transform.up * rayDistance;
        Vector2 LDir = Quaternion.AngleAxis(rayAngle, Vector3.forward) * transform.up * rayDistance;

        RaycastHit2D hit;

        //Front middle raycast
        hit = Physics2D.Raycast(sensorStartPos.position, A, rayDistance, collLayers);
        if (hit.collider != null)
        {
            status.MRay = true;
            HelperScript.DrawArrowDebug(sensorStartPos.position, (Vector2)sensorStartPos.position + A, Color.red);
        }

        //Front right raycast
        hit = Physics2D.Raycast(rSensorStartPos.position, A, rayDistance,collLayers);
        if (hit.collider != null)
        {
            status.RRay = true;
            HelperScript.DrawArrowDebug(rSensorStartPos.position, (Vector2)rSensorStartPos.position + A, Color.red);
        }

        //Front left raycast
        hit = Physics2D.Raycast(lSensorStartPos.position, A, rayDistance,collLayers);
        if (hit.collider != null)
        {
            status.LRay = true;
            HelperScript.DrawArrowDebug(lSensorStartPos.position, (Vector2)lSensorStartPos.position + A, Color.red);
        }

        //Angled raycasts

        //Left Angled raycast
        hit = Physics2D.Raycast(lSensorStartPos.position, LDir, rayDistance,collLayers);
        if (hit.collider != null)
        {
            status.LARay = true;
            HelperScript.DrawArrowDebug(lSensorStartPos.position, (Vector2)lSensorStartPos.position + LDir, Color.red);
        }

        //Right Angled raycast
        hit = Physics2D.Raycast(rSensorStartPos.position, RDir, rayDistance, collLayers);
        if (hit.collider != null)
        {
            status.RARay = true;
            HelperScript.DrawArrowDebug(rSensorStartPos.position, (Vector2)rSensorStartPos.position + RDir, Color.red);
        }

        return status;
    }

}
