using System;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

namespace cmplx_statemachine
{
    public class ST_APPR_BASE : BaseState
    {

        Transform selfTransform;
        TankAIScript3 tankAIScript;
        NewTankScript tankController;

        ArmyBaseScript_pt1 targetBase;

        int[] controlBits = new int[2];//Used to control tank movement
        SensorArrayScript sensorArray;

        //Path settings
        Path currPath;
        int currWaypoint;
        bool hasReachedEndOfPath;
        float nextWpDistance;

        public ST_APPR_BASE(TankAIStateMachine stM, TankAIScript3 tankAIScript) : base(stM)
        {
            stateName = "APPR_BASE1";
            this.tankAIScript = tankAIScript;
            selfTransform = tankAIScript.transform;
            tankController = tankAIScript.GetComponent<NewTankScript>();
            targetBase = tankAIScript.targetBase;
        }


        //----------------------
        //State methods 
        //----------------------

        public override void OnEnter()
        {
            var lst = new List<Vector3>();

            lst.Add(selfTransform.position);
            lst.Add(targetBase.enemyLandingZones[0].position);

            currPath = new ABPath();
            currPath.vectorPath = lst;
        }

        public override void OnUpdate()
        {
            Array.Clear(controlBits, 0, 2);//Clear control bits

            // ShowWayPoints();
            FollowWayPoints();
            AvoidLocalObstacles();

            //Apply controlls
            tankController.Move(controlBits[0]);
            tankController.Rotate(controlBits[1]);

            CheckForStateTransition();

            foreach (var item in currPath.vectorPath)
            {
                HelperScript.DrawPointDebug(item, Color.red);
            }
            Debug.DrawLine(currPath.vectorPath[0], currPath.vectorPath[1]);

        }

        //----------------------
        //Other methods
        //----------------------

        //----------------------
        //Path functions
        //----------------------

        public void UpdatePath(Path path)
        {
            currPath = path;
        }

        //----------------------
        //Drive functions
        //----------------------

        void FollowWayPoints()
        {
            if (currPath == null)
            {
                Debug.LogWarning("No path available");
                return;
            }

            hasReachedEndOfPath = false;
            // The distance to the next waypoint in the path
            float distanceToWaypoint;
            //Check which waypoint we are in
            while (true)
            {
                distanceToWaypoint = Vector2.Distance(selfTransform.position, currPath.vectorPath[currWaypoint]);
                if (distanceToWaypoint < nextWpDistance)
                {
                    if (currWaypoint + 1 < currPath.vectorPath.Count)
                    {
                        currWaypoint++;
                    }
                    else
                    {
                        hasReachedEndOfPath = true;
                        break;
                    }
                }
                else
                {
                    break;
                }
            }


            if (!hasReachedEndOfPath)//IF not reached end of path
            {
                //Incase we skip a waypoint while local avoidance
                if (HasSkippedCurrentWayPointNew() && !hasReachedEndOfPath) currWaypoint++;

                Vector3 dir = (currPath.vectorPath[currWaypoint] - selfTransform.position).normalized;

                HelperScript.DrawArrowDebug(selfTransform.position, selfTransform.position + dir, Color.blue);


                if (IsFacingDirection(dir, 7))
                { controlBits[0] = 1; }

                SetControlBitsTowardsDir(dir, controlBits);
            }
        }

        void AvoidLocalObstacles()
        {
            sensorArray = selfTransform.GetComponentInChildren<SensorArrayScript>();

            SensorArrayScript.CollisionStatus collisionStatus = sensorArray.CheckCollisionArray();

            //Control forward movement

            if (collisionStatus.RRay || collisionStatus.LRay)
            {
                controlBits[0] = 0;
            }

            if (collisionStatus.MRay && (collisionStatus.LRay || collisionStatus.RRay))
            {
                controlBits[0] = 0;
            }

            if (collisionStatus.isFullyBlocked)
            {
                controlBits[0] = 0;
            }

        }

        bool IsFacingDirection(Vector2 dir, float tolerance)
        {
            float angle = Vector2.Angle(dir, selfTransform.up);
            if (Mathf.Abs(angle) < tolerance)
            {
                return true;
            }
            return false;
        }

        void SetControlBitsTowardsDir(Vector2 dir, int[] cBits)
        {
            float angle = Vector2.SignedAngle(dir, selfTransform.up);
            if (Mathf.Abs(angle) > 7)
            {
                if (angle > 0)
                { cBits[1] = -1; }
                else { cBits[1] = 1; }
            }
        }

        bool HasSkippedCurrentWayPointNew()
        {
            if (currWaypoint < currPath.vectorPath.Count - 1)
            {
                Vector2 dir = (currPath.vectorPath[currWaypoint + 1] - currPath.vectorPath[currWaypoint]).normalized;
                float a = Vector2.Dot(dir, Vector2.right);

                if (a > 0)//Facing right
                {
                    return selfTransform.position.x > currPath.vectorPath[currWaypoint].x;
                }
                else if (a < 0)//Facing left
                {
                    return selfTransform.position.x < currPath.vectorPath[currWaypoint].x;
                }
            }
            return false;
        }

        Transform PickRandomEnemyLandingZone()
        {
            int a = UnityEngine.Random.Range(0, 3);
            tankAIScript.landZoneIndex = a;
            return (targetBase).enemyLandingZones[a];
        }

        void CheckForStateTransition()
        {
            if (tankAIScript.enemiesInSight.Count > 0)
            {
                stateMachineInstance.ChangeState("ATTK_ENEM");
            }

            //float distance = Vector2.Distance(selfTransform.position, .position);

            //if (distance < targetDistanceTolerance)
            //{
            //    stateMachineInstance.ChangeState("REAC_BASE");
            //}
            if (hasReachedEndOfPath)
            {
                stateMachineInstance.ChangeState("REAC_BASE");
            }
        }

        //--------------------------
        //Helper functions
        //--------------------------
        void ShowWayPoints()
        {
            if (currPath != null)
            {
                for (int i = 0; i < currPath.vectorPath.Count; i++)
                {
                    HelperScript.DrawPointDebug(currPath.vectorPath[i], Color.red);
                }
            }

        }

    }
}