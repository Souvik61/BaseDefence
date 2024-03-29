﻿using System;
using UnityEngine;
using Pathfinding;

namespace cmplx_statemachine
{
    public class NewApproachingBaseState : BaseState
    {
        protected TankAIScript3 tankAIScript;
        protected TankScript tankController;
        protected Transform selfTransform;

        float targetDistanceTolerance = 7;
        Vector2 dirToTarget;
        ArmyBaseScript_pt1 targetBase;

        Transform pathStartPoint;

        public float nextWayPointDistance = 1.5f;

        //Path seeker settings
        Path path;
        int currWaypoint = 0;
        bool hasReachedEndOfPath = false;

        Seeker seeker;

        float timer;
        float pathUpdateRate;//path update every n secs.

        Vector2 lastTimeCheckedPosition;

        SensorArrayScript sensorArray;

        Transform landingZoneTarget;

        int[] controlBits = new int[2];//Used to control tank movement

        public NewApproachingBaseState(TankAIStateMachine stM, TankAIScript3 tankAIScript) : base(stM)
        {
            stateName = "APPR_BASE";
            this.tankAIScript = tankAIScript;
            selfTransform = tankAIScript.transform;
            tankController = tankAIScript.GetComponent<NewTankScript>();
            targetBase = tankAIScript.targetBase;
            pathStartPoint = selfTransform.Find("tank_body/muzzle/firepoint");
            pathUpdateRate = 1.0f;
        }

        //----------------------
        //State methods 
        //----------------------

        public override void OnEnter()
        {
            lastTimeCheckedPosition = selfTransform.position;
            seeker = tankAIScript.GetSeekerModule.seeker;

            timer = pathUpdateRate;

            UpdatePath();
        }

        public override void OnUpdate()
        {
            Array.Clear(controlBits, 0, 2);//Clear control bits

           // ShowWayPoints();
            FollowWayPoints();
            AvoidLocalObstacles();

            //TryFaceMuzzleTowardsDirection();//While moving try face towards movement

            //Apply controlls
            tankController.Move(controlBits[0]);
            tankController.Rotate(controlBits[1]);

            CheckForStateTransition();
        }

        //----------------------
        //Other methods
        //----------------------

        //----------------------
        //Path functions
        //----------------------
        
        void UpdatePath()
        {
            landingZoneTarget = PickRandomEnemyLandingZone();

            if (seeker.IsDone())
                seeker.StartPath((Vector2)selfTransform.position, landingZoneTarget.position, OnPathComplete);
        }

        void OnPathComplete(Path p)
        {
            if (!p.error)
            {
                path = p;
                // Reset the waypoint counter so that we start to move towards the first point in the path
                currWaypoint = 0;
            }
        }

        //----------------------
        //Drive functions
        //----------------------

        void FollowWayPoints()
        {
            if (path == null)
                return;

            hasReachedEndOfPath = false;
            // The distance to the next waypoint in the path
            float distanceToWaypoint;
            //Check which waypoint we are in
            while (true)
            {
                distanceToWaypoint = Vector2.Distance(selfTransform.position, path.vectorPath[currWaypoint]);
                if (distanceToWaypoint < nextWayPointDistance)
                {
                    if (currWaypoint + 1 < path.vectorPath.Count)
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

                Vector3 dir = (path.vectorPath[currWaypoint] - selfTransform.position).normalized;

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

          //Control turn

            if (collisionStatus.LRay)//if lSide hit
            {
                    controlBits[1] = -1;
            }

            if (collisionStatus.RRay)//if rSide hit
            {
              
                    controlBits[1] = 1;
            }

            if (collisionStatus.almostBlocked)
            {
                controlBits[1] = 0;
            }

            //Control forward movement

            if (collisionStatus.RRay || collisionStatus.LRay)
            {
                controlBits[0] = 0;
            }

            if (collisionStatus.LARay || collisionStatus.RARay)
            {
                controlBits[0] = 1;
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

        void SetControlBitsTowardsDir(Vector2 dir,int [] cBits)
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
            if (currWaypoint < path.vectorPath.Count-1)
            {
                Vector2 dir = (path.vectorPath[currWaypoint + 1] - path.vectorPath[currWaypoint]).normalized;
                float a = Vector2.Dot(dir, Vector2.right);

                if (a > 0)//Facing right
                {
                    return selfTransform.position.x > path.vectorPath[currWaypoint].x;
                }
                else if (a < 0)//Facing left
                {
                    return selfTransform.position.x < path.vectorPath[currWaypoint].x;
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
            if (path!= null) {
                for (int i = 0; i < path.vectorPath.Count; i++)
                {
                    HelperScript.DrawPointDebug(path.vectorPath[i], Color.red);
                }
            }
        
        }

        //--------------------------
        //Garbage functions--might delete later
        //--------------------------

        void TryFaceTowardsDirection(Vector2 dir)
        {
            float angle = Vector2.SignedAngle(dir, selfTransform.up);
            if (Mathf.Abs(angle) > 7)
            {
                if (angle > 0)
                { controlBits[1] = -1; }
                else { controlBits[1] = 1; }
            }
        }

        void TryFaceMuzzleTowardsDirection(Vector2 dir)
        {
            float angle = Vector2.SignedAngle(selfTransform.up, tankController.muzzleTransform.up);
            if (Mathf.Abs(angle) > 2)
            {
                if (angle > 0)
                { tankController.MuzzleRotate(1); }
                else { tankController.MuzzleRotate(-1); }
            }
        }

        bool CheckPositionChangedSinceLast()
        {
            float delta = Vector2.Distance(selfTransform.position, lastTimeCheckedPosition);
            lastTimeCheckedPosition = selfTransform.position;
            return (delta > 1.5f);

        }

        bool IsFacingDirection(Vector2 dir)
        {
            float angle = Vector2.SignedAngle(dir, selfTransform.up);
            if (Mathf.Abs(angle) > 7)
            {
                return true;
            }
            return false;
        }
    }
}
