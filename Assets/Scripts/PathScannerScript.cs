using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class PathScannerScript : MonoBehaviour
{
    public AstarPath astarPath;

    private void Start()
    {
        InvokeRepeating(nameof(UpdateGraph), 1, 1);
    }

    private void UpdateGraph()
    {
        // Recalculate all graphs
        AstarPath.active.Scan();
    }

}
