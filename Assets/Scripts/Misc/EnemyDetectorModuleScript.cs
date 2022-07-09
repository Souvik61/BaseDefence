using System.Collections.Generic;
using UnityEngine;

public class EnemyDetectorModuleScript : MonoBehaviour
{
    public int detectAgainstId;

    public List<FOVObsCheckScript> listOfDetectors;

    public bool IsTankAheadAt(int indx)
    {
        if (listOfDetectors[indx].isObstaclesInRange)
        {
            var u = listOfDetectors[indx].obstaclesInRange[0].GetComponent<UnitComponent>();

            if (u)
                return u.teamID == detectAgainstId;
        
        }
        return false;
    }

    public int GetIndexWhereEnemyIsAt()
    {
        for (int i = 0; i < 3; i++)
        {
            if (IsTankAheadAt(i))
                return i;
        }

        return -1;
    }

}
