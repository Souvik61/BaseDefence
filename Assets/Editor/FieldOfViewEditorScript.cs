using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FOVObsCheckScript))]
public class FieldOfViewEditor : Editor
{
    private void OnSceneGUI()
    {
        FOVObsCheckScript fov = (FOVObsCheckScript)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fov.transform.position, Vector3.forward, Vector3.right, 360, fov.radius);

        //Vector3 viewAngle01 = DirectionFromAngle(fov.transform.eulerAngles.z, -fov.angle / 2);
        //Vector3 viewAngle02 = DirectionFromAngle(fov.transform.eulerAngles.z, fov.angle / 2);

        Vector3 a = Quaternion.AngleAxis(fov.angle, Vector3.forward) * fov.transform.up;
        Vector3 b = Quaternion.AngleAxis(-fov.angle, Vector3.forward) * fov.transform.up;

        Handles.color = Color.yellow;
        Handles.DrawLine(fov.transform.position, fov.transform.position + a * fov.radius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + b * fov.radius);


        if (fov.isObstaclesInRange)
        {
            Handles.color = Color.green;
            foreach (var item in fov.obstaclesInRange)
            {
                if (item != null)
                    Handles.DrawLine(fov.transform.position, item.transform.position);
            }
           // Handles.DrawLine(fov.transform.position, fov.playerRef.transform.position);
        }
        
    }

    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
