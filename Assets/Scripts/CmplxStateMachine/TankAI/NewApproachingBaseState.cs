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

        SensorArrayScript sensorArray;

        public NewApproachingBaseState(TankAIStateMachine stM, TankAIScript3 tankAIScript) : base(stM)
        {
            stateName = "APPR_BASE1";
            this.tankAIScript = tankAIScript;
            selfTransform = tankAIScript.transform;
            tankController = tankAIScript.GetComponent<TankScript>();
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
            seeker = tankAIScript.GetComponent<Seeker>();

            timer = pathUpdateRate;

            UpdatePath();
        }

        public override void OnUpdate()
        {
            ShowWayPoints();
            FollowWayPoints();
            AvoidLocalObstacles();
        }

        //----------------------
        //Other methods
        //----------------------

        //----------------------
        //Path functions
        //----------------------
        void TryUpdatePath()
        {
            UpdatePath();
        }

        void UpdatePath()
        {
            if (seeker.IsDone())
                seeker.StartPath((Vector2)selfTransform.position + tankAIScript.compass.startPoint, targetBase.transform.position, OnPathComplete);
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
            {
                // We have no path to follow yet, so don't do anything
                return;
            }
            hasReachedEndOfPath = false;
            // The distance to the next waypoint in the path
            float distanceToWaypoint;
            while (true)
            {
                distanceToWaypoint = Vector3.Distance(selfTransform.position, path.vectorPath[currWaypoint]);
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

            Vector3 dir = (path.vectorPath[currWaypoint] - selfTransform.position).normalized;

            HelperScript.DrawArrowDebug(selfTransform.position, selfTransform.position + dir, Color.blue);


            if (IsFacingDirection(dir, 7))
            { tankController.Move(1); }

            TryFaceTowardsDirection(dir);


        }

        void AvoidLocalObstacles()
        {
            sensorArray = selfTransform.GetComponentInChildren<SensorArrayScript>();

            SensorArrayScript.CollisionStatus collisionStatus = sensorArray.CheckCollisionArray();

            if (collisionStatus.isBlocked)
            {
                tankController.Move(-1);
            }

        }

        void DriveTank()
        {
            

        }

        void CheckLocalCollision()
        {
            sensorArray = selfTransform.GetComponentInChildren<SensorArrayScript>();

            SensorArrayScript.CollisionStatus collisionStatus = sensorArray.CheckCollisionArray();

            

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

        bool IsFacingDirection(Vector2 dir, float tolerance)
        {
            float angle = Vector2.Angle(dir, selfTransform.up);
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
            return (delta > 1.5f);

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

    }
}
