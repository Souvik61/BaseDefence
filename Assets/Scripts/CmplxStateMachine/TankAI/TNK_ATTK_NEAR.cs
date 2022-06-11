using System.Collections;
using System.Collections.Generic;
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

        }

        //-------------------------
        //Other methods
        //-------------------------


        void MoveLogic()
        {
            dirToTarget = (targetPt.position - selfTransform.position).normalized;

            TryFaceTowardsDirection(dirToTarget,3.0f);

            if (IsFacingDirection(dirToTarget, 3))
            {
                tankController.Move(1);
            }
        }

        void AttackLogic()
        {





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
    }
}