using UnityEngine;

public class AlwaysUpScript : MonoBehaviour
{
    public Vector2 offset;
    public Transform canvasTransform;
    public Transform baseTransform;
    public Transform progTransform;

    private void Awake()
    {
        Invoke(nameof(Set), 1);
    }

    void Set()
    {
        Vector2 a = baseTransform.position + (Vector3)offset;
        canvasTransform.position = a;
        var rot = Quaternion.LookRotation(Vector3.forward, Vector2.up);
        progTransform.rotation = rot;
    }


}
