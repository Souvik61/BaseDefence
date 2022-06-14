using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cmplx_statemachine
{
    public class ST_REAC_BASE : BaseState
    {
        Transform selfTransform;
        TankAIScript3 tankAI;
        NewTankScript tankController;
        Transform CCTransform;
        Vector2 dirToTarget;

        public ST_REAC_BASE(TankAIStateMachine stM, TankAIScript3 tankAIScript) : base(stM)
        {
            stateName = "REACH_BASE";
            tankAI = tankAIScript;
            selfTransform = tankAI.transform;
            tankController = tankAI.tankController;
        }

        public override void OnEnter()
        {
            CCTransform = tankAI.targetBase.commandCenter.transform;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            AttackLogic();

            CheckForTransition();
        }

        void AttackLogic()
        {
            Transform target = CCTransform;
            dirToTarget = (target.position - selfTransform.position).normalized;

            TryFaceMuzzleTowardsDirection(dirToTarget);

            if (IsMuzzleFacingTowardDir(dirToTarget, 3))
            {
                tankController.Shoot();
            }
        }

        void CheckForTransition()
        {




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

        bool IsMuzzleFacingTowardDir(Vector2 dir, float tol)
        {
            float angle = Vector2.Angle(dir, tankController.muzzleTransform.up);
            return angle < tol;
        }


    }
}