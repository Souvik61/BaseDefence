using UnityEngine;
using Pathfinding;

public class PathSeekerScript : MonoBehaviour
{
    [SerializeField]
    Seeker seeker;

    private void Awake()
    {
        seeker = GetComponent<Seeker>();
    }

    public void GeneratePath(Vector2 from,Vector2 to,OnPathDelegate onPath)
    {
        if (seeker == null) { Debug.LogError("No seeker attached"); return; }
        if (seeker.IsDone())
            seeker.StartPath(from, to, onPath);
    }


}
