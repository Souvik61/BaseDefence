﻿using UnityEngine;

public class HelperScript
{
    private static Vector2 RotateVectorBy(Vector2 inVec, float angle)
    {
        Matrix4x4 mat = new Matrix4x4(new Vector4(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0, 0), new Vector4(-Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad), 0, 0), Vector4.zero, Vector4.zero);
        return mat * inVec;
    }

    public static float Map(float x, float in_min, float in_max, float out_min, float out_max)
    {
        return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
    }

    public static bool IsInRange(float input, float min, float max)
    {
        return (input > min && input < max) ? true : false;
    }

    public static float RotationTo(float currentRotation, float desiredRotation, float stepSpeed, float topLimit, float bottomLimit)
    {

        float toRotate = desiredRotation - currentRotation;//Figure out to rotate

        toRotate = Mathf.Clamp(toRotate, -stepSpeed, stepSpeed) * 100 * Time.deltaTime;//Clamp rotation

        //Debug.Log(toRotate);
        currentRotation += toRotate;

        //If currentRotation almost equals target rotation snap to target rot. 
        if (HelperScript.IsInRange(currentRotation, desiredRotation - 0.0001f, desiredRotation + 0.00001f))
        {
            currentRotation = desiredRotation;
        }

        return currentRotation;
    }

    //To draw debug bounding box
    public static void DrawBoundDebug(Bounds bound, Color color, float time = 0.25f)
    {
        Vector2 A = bound.min;
        Vector2 B = new Vector2(bound.min.x, bound.max.y);
        Vector2 C = bound.max;
        Vector2 D = new Vector2(bound.max.x, bound.min.y);
        Debug.DrawLine(A, B, color, time);
        Debug.DrawLine(B, C, color, time);
        Debug.DrawLine(C, D, color, time);
        Debug.DrawLine(D, A, color, time);
    }

    public static void DrawPointDebug(Vector2 position, Color color, float time = 0.25f)
    {
        Vector2 v = new Vector2(0.1f, 0.1f);
        Vector2 A = position - v;
        Vector2 B = position + v;
        Vector2 C = position + new Vector2(0.1f, -0.1f);
        Vector2 D = position + new Vector2(-0.1f, +0.1f);
        Debug.DrawLine(A, B, color, time);
        Debug.DrawLine(C, D, color, time);
    }

   /* public static void DrawArrow(Vector2 start, Vector2 end, Color color, float time = 0.25f)
    {
        Vector2 C = new Vector2(end.x - 0.1f, end.y + 0.1f);
        Vector2 D = new Vector2(end.x - 0.1f, end.y - 0.1f);

        Debug.DrawLine(start, end, color, time);
        Debug.DrawLine(C, end, color, time);
        Debug.DrawLine(D, end, color, time);
    }
    */
    /*
    public static void DrawArrowDebug(Vector3 pos, Vector3 direction, Color color, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
    {
        Debug.DrawRay(pos, direction, color);

        Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 0, 1);
        Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, 0, 1);
        Debug.DrawRay(pos + direction, right * arrowHeadLength, color);
        Debug.DrawRay(pos + direction, left * arrowHeadLength, color);
    }
    */

    public static void DrawArrowDebug(Vector3 start, Vector3 end, Color color, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
    {
        Debug.DrawLine(start, end, color);//Draw base line

        Vector2 a = new Vector2(1, 0) * arrowHeadLength;
        Vector2 b = new Vector2(1, 0) * arrowHeadLength;

        float angle = Vector2.SignedAngle(Vector2.right, (end - start).normalized);//Calculate angle
        a = RotateVectorBy(a, angle);
        b = RotateVectorBy(b, angle);

        a = RotateVectorBy(a, 180 + arrowHeadAngle);
        b = RotateVectorBy(b, 180 - arrowHeadAngle);

        //Draw arrows
        Debug.DrawLine(end, (Vector2)end + a, color);
        Debug.DrawLine(end, (Vector2)end + b, color);
    }


}