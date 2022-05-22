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

        public float nextWayPointDistance = 5;

        //Path seeker settings
        Path path;
        int currWaypoint = 0;
        bool hasReachedEndOfPath = false;

        Seeker seeker;

        float timer;
        float pathUpdateRate;//path update every n secs.

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
            seeker = tankAIScript.GetComponent<Seeker>();

            timer = pathUpdateRate;

            UpdatePath();

        }

        public override void OnUpdate()
        {
            //Update path
            if (timer <= 0)
            {
                UpdatePath();
                timer = pathUpdateRate;
            }
            timer -= Time.deltaTime;


            DriveTank();
        }

        public override void OnExit()
        {
           
        }

        void UpdatePath()
        {
            if (seeker.IsDone())
                seeker.StartPath(pathStartPoint.position, targetBase.transform.position, OnPathComplete);
        }

        void OnPathComplete(Path p)
        {
            if (!p.error)
            {
                path = p;
                currWaypoint = 0;
            }

        }

        void DriveTank()
        {
            if (targetBase != null && path != null)//Check path and target not equal to null
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

                HelperScript.DrawArrowDebug(selfTransform.position, selfTransform.position + (Vector3)dirToWP, Color.red);

                float dist = Vector2.Distance(selfTransform.position, path.vectorPath[currWaypoint]);

                if (dist < nextWayPointDistance)
                {
                    currWaypoint++;
                }

                TryFaceTowardsDirection(dirToWP);

                tankController.Move(1);

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

        public override void OnPhysicsUpdate()
        {
            
        }
    }
}
