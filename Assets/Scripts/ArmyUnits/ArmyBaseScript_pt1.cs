using System.Collections.Generic;
using UnityEngine;

public class ArmyBaseScript_pt1 : MonoBehaviour
{
    public int baseID;
    public List<WatchTowerAIScript> watchTowers;
    public CommandCenterScript commandCenter;
    public List<Transform> enemyLandingZones;
    public List<Transform> enemyLandingZones1;
    public Transform nearCCLandingZone;

}
