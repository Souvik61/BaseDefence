using UnityEngine;

namespace cmplx_statemachine
{
    public class TNK_ATTK_NEAR : BaseState
    {

        enum SubStates { MOVING, ATTAK };

        SubStates subState;

        Transform selfTransform;
        TankAIScript3 tankAI;
        NewTankScript tankController;
        Transform targetPt;
        Vector2 dirToTarget;

        public TNK_ATTK_NEAR(TankAIStateMachine stM, TankAIScript3 tankAI) : base(stM)
        {
            stateName = "TNK_ATTK_NEAR";
            subState = SubStates.MOVING;
            this.tankAI = tankAI;
            selfTransform = tankAI.transform;
            tankController = tankAI.tankController;
        }

        public override void OnEnter()
        {
            targetPt = tankAI.targetBase.enemyLandingZones1[tankAI.landZoneIndex];
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            switch (subState)
            {
                case SubStates.MOVING:
                    {
                        MoveLogic();
                    }
                    break;
                case SubStates.ATTAK:
                    {
                        AttackLogic();
                    }
                    break;
                default:
                    break;
            }

            CheckSubStateTransition();

            CheckStateTransition();
        }

        //-------------------------
        //Other methods
        //-------------------------


        void MoveLogic()
        {

            if (Vector2.Distance(targetPt.position, selfTransform.position) <= 1)
                return;

            dirToTarget = (targetPt.position - selfTransform.position).normalized;

            TryFaceTowardsDirection(dirToTarget,3.0f);

            if (IsFacingDirection(dirToTarget, 3))
            {
                tankController.Move(1);
            }
        }

        void AttackLogic()
        {
            if (tankAI.enemiesInSight.Count > 0)
            {
                Transform target = tankAI.enemiesInSight[0].transform;
                dirToTarget = (target.position - selfTransform.position).normalized;

                TryFaceMuzzleTowardsDirection(dirToTarget);

                if (IsMuzzleFacingTowardDir(dirToTarget, 3))
                {
                    tankController.Shoot();
                }
            }
        }

        void CheckStateTransition()
        {
            if (Vector2.Distance(targetPt.position, selfTransform.position) <= 1)
                stateMachineInstance.ChangeState("ST_ATTK_CC");
        }

        void CheckSubStateTransition()
        {
            subState = (tankAI.enemiesInSight.Count > 0) ? SubStates.ATTAK : SubStates.MOVING;
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

        void TryFaceTowardsDirection(Vector2 dir,float tol)
        {
            float angle = Vector2.SignedAngle(dir, selfTransform.up);
            if (Mathf.Abs(angle) > tol)
            {
                if (angle > 0)
                { tankController.Rotate(-1); }
                else { tankController.Rotate(1); }
            }
        }

        void TryFaceMuzzleTowardsDirection(Vector2 dir)
        {
            float angle = Vector2.SignedAngle(dir, tankController.muzzleTransform.up);
            if (Mathf.Abs(angle) > 2)
            {
                if (angle > 0)
                { tankController.MuzzleRotate(1); }
                else { tankController.MuzzleRotate(-1); }
            }
        }

        bool IsMuzzleFacingTowardDir(Vector2 dir,float tol)
        {
            float angle = Vector2.Angle(dir, tankController.muzzleTransform.up);
            return angle < tol;

        }
    }
}