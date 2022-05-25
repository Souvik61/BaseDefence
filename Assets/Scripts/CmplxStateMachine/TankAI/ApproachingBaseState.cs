using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

namespace cmplx_statemachine
{
    public class ApproachingBaseState : BaseState
    {
        protected TankAIScript3 tankAIScript;
        protected TankScript tankController;
        protected Transform selfTransform;

        float targetDistanceTolerance = 7;
        Vector2 dirToTarget;
        ArmyBaseScript targetBase;

        Transform pathStartPoint;

        public float nextWayPointDistance = 0.7f;

        //Path seeker settings
        Path path;
        int currWaypoint = 0;
        bool hasReachedEndOfPath = false;

        Seeker seeker;

        float timer;
        float pathUpdateRate;//path update every n secs.

        Vector2 lastTimeCheckedPosition;

        public ApproachingBaseState(TankAIStateMachine stM,TankAIScript3 tankAIScript) : base(stM)
        {
            stateName = "APPR_BASE";
            this.tankAIScript = tankAIScript;
            selfTransform = tankAIScript.transform;
            tankController = tankAIScript.GetComponent<TankScript>();
            targetBase = tankAIScript.targetBase;
            pathStartPoint = selfTransform.Find("tank_body/muzzle/firepoint");
            pathUpdateRate = 1.0f;
        }

        public override void OnEnter()
        {
            lastTimeCheckedPosition = selfTransform.position;
            seeker = tankAIScript.GetComponent<Seeker>();

            timer = pathUpdateRate;

            UpdatePath();
        }

        public override void OnUpdate()
        {
            //Update path
            if (timer <= 0)
            {
                TryUpdatePath();
                timer = pathUpdateRate;
            }
            timer -= Time.deltaTime;

            DriveTank();

            TryFaceMuzzleTowardsDirection(selfTransform.up);

            CheckForTransition();

            //HelperScript.DrawPointDebug(path.vectorPath[currWaypoint], Color.blue);

        }

        void TryUpdatePath()
        {
            if (CheckPositionChangedSinceLast())
                UpdatePath();
        }
        void UpdatePath()
        {
            if (seeker.IsDone())//If position not changed since last dont update
                seeker.StartPath((Vector2)selfTransform.position + tankAIScript.compass.startPoint, targetBase.transform.position, OnPathComplete);
        }

        void OnPathComplete(Path p)
        {
            path = null;
            if (!p.error)
            {
                path = p;
                currWaypoint = 0;

                //Debug draw waypoints
                for (int i = 0; i < path.vectorPath.Count; i++)
                {
                    HelperScript.DrawPointDebug(path.vectorPath[i], Color.red, 1);
                }
            }
            if (p.error)
            {
                Debug.Log("path blocked");
            
            }
        }

        /*
        void DriveTank()
        {
            if (targetBase != null && path != null && !hasReachedEndOfPath)//Check path and target not equal to null
            {
                //Check has reached end
                if (currWaypoint >= path.vectorPath.Count)
                {
                    hasReachedEndOfPath = true;
                    return;
                }
                else
                {
                    hasReachedEndOfPath = false;

                }

                Vector2 dirToWP = (path.vectorPath[currWaypoint] - selfTransform.position).normalized;
                //Draw debug arrow
                HelperScript.DrawArrowDebug(selfTransform.position, selfTransform.position + (Vector3)dirToWP, Color.red);
                //Draw current waypoint
                HelperScript.DrawPointDebug(path.vectorPath[currWaypoint], Color.blue);

                float dist = Vector2.Distance(selfTransform.position, path.vectorPath[currWaypoint]);


                TryFaceTowardsDirection(dirToWP);

                if (IsFacingDirection(dirToWP, 5))
                {
                    //tankController.Move(1);
                }

                if (dist < nextWayPointDistance)
                {
                    currWaypoint++;
                }

            }
        }
        */

        void DriveTank()
        {
            if (path == null)
            {
                return;
            }
            hasReachedEndOfPath = false;
            // The distance to the next waypoint in the path
            float distanceToWaypoint;
            while (true)
            {
                // If you want maximum performance you can check the squared distance instead to get rid of a
                // square root calculation. But that is outside the scope of this tutorial.
                distanceToWaypoint = Vector2.Distance(selfTransform.position, path.vectorPath[currWaypoint]);
                if (distanceToWaypoint < nextWayPointDistance)
                {
                    // Check if there is another waypoint or if we have reached the end of the path
                    if (currWaypoint + 1 < path.vectorPath.Count)
                    {
                        currWaypoint++;
                    }
                    else
                    {
                        // Set a status variable to indicate that the agent has reached the end of the path.
                        // You can use this to trigger some special code if your game requires that.
                        hasReachedEndOfPath = true;
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
            // Direction to the next waypoint
            // Normalize it so that it has a length of 1 world unit
            Vector3 dir = (path.vectorPath[currWaypoint] - selfTransform.position).normalized;
            float angle = Vector2.Angle(selfTransform.forward, dir);
            HelperScript.DrawArrowDebug(selfTransform.position, selfTransform.position + dir, Color.yellow);

            TryFaceTowardsDirection(dir);

            if (angle < 20)
            {

                if (IsFacingDirection(dir, 7.5f))
                {
                    tankController.Move(1);
                }
            }



        }

        void CheckForTransition()
        {
            if (tankAIScript.enemiesInSight.Count > 0)
            {
                stateMachineInstance.ChangeState("ATTK_ENEM");
            }

            float distance = Vector2.Distance(selfTransform.position, targetBase.transform.position);

            if (distance < targetDistanceTolerance)
            { 
                stateMachineInstance.ChangeState("REAC_BASE");
            }

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

        bool IsFacingDirection(Vector2 dir,float tolerance)
        {
            float angle = Vector2.SignedAngle(dir, selfTransform.up);
            if (Mathf.Abs(angle) < tolerance)
            {
                return true;
            }
            return false;
        }

        void TryFaceTowardsDirection(Vector2 dir)
        {
            float angle = Vector2.SignedAngle(dir, selfTransform.up);
            if (Mathf.Abs(angle) > 7)
            {
                if (angle > 0)
                { tankController.Rotate(-1); }
                else { tankController.Rotate(1); }
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
            return delta > 1.5f ? true : false;

        }

    }
}
