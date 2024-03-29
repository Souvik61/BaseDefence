﻿using System.Collections.Generic;
using UnityEngine;

public class NewArmyBaseScript : ArmyBaseScript
{
    public List<WatchTowerAIScript> watchTowers;
    public CommandCenterScript commandCenter;
    public List<Transform> enemyLandingZones;
    public Transform nearCCLandingZone;
}
